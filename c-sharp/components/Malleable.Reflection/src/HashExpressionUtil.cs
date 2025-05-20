namespace Kadense.Malleable.Reflection
{
    public class MalleableHashUtil : MalleableExpressionUtil 
    {
        public MalleableHashUtil() : base()
        {
            Salt = Environment.GetEnvironmentVariable("HASH_SALT") ?? "1234567890abcdef1234567890abcdef";
        }

        protected string Salt { get; set; }

        public string HashSha256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                using(var stream = new MemoryStream())
                {
                    using(var writer = new StreamWriter(stream))
                    {
                        writer.Write(input);
                        writer.Write(Salt);
                        writer.Flush();
                        stream.Position = 0;
                        var hash = sha256.ComputeHash(stream);
                        return Convert.ToBase64String(hash);
                    }
                }
            }
        }
    }
}