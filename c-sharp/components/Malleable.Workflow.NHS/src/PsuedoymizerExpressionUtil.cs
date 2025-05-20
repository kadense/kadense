using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.NHS
{
    public class PsuedoymizerExpressionUtil : MalleableExpressionUtil
    {
        public PsuedoymizerExpressionUtil() : base()
        {
            PseudoSalt = Environment.GetEnvironmentVariable("PSEUDO_SALT") ?? "1234567890abcdef1234567890abcdef";
            RePseudoSalt = Environment.GetEnvironmentVariable("REPSEUDO_SALT") ?? "1234567890abcdef1234567890abcdef";
        }

        protected string PseudoSalt { get; set; }
        protected string RePseudoSalt { get; set; }

        public void SetPsuedoSalt(string salt)
        {
            PseudoSalt = salt;
        }

        public void SetRePseudoSalt(string salt)
        {
            RePseudoSalt = salt;
        }

        public string NhsNumber(string input)
        {
            var firstParse = Parse($"nhs_number{input}", PseudoSalt);
            var secondParse = Parse($"pseudoid_{firstParse}", RePseudoSalt);
            return secondParse.Substring(0, 16);
        }
        public string BirthDataAndPostcode(string birthDate, string postcode)
        {
            var firstParse = Parse($"birthdate_postcode_{birthDate}_{postcode.Replace(" ", "")}", PseudoSalt);
            var secondParse = Parse($"pseudoid_{firstParse}", RePseudoSalt);
            return secondParse.Substring(0, 16);
        }

        protected string Parse(string input, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                using(var stream = new MemoryStream())
                {
                    using(var writer = new StreamWriter(stream))
                    {
                        writer.Write(input.ToLower());
                        writer.Write(salt);
                        writer.Flush();
                        stream.Position = 0;
                        var hash = sha256.ComputeHash(stream);
                        return Convert.ToHexString(hash).ToLower();
                    }
                }
            }
        }

    }
}