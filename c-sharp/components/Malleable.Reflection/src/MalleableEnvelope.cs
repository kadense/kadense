using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kadense.Malleable.Reflection
{
    [MalleableClass("Kadense.Malleable", "Reflection", "MalleableEnvelope")]
    public class MalleableEnvelope<T> : MalleableBase
        where T : MalleableBase
    {
        public MalleableEnvelope()
        {

        }

        public MalleableEnvelope(T message)
        {
            Message = message;
        }
        
        public MalleableEnvelope(T message, string? lineageId, string? step)
        {
            Message = message;
            LineageId = lineageId;
            Step = step;
        }

        /// <summary>
        /// The message that is being sent
        /// </summary>
        [JsonPropertyName("message")]
        public T? Message { get; set; }

        /// <summary>
        /// The message signature
        /// </summary>
        [JsonPropertyName("lsig")]
        public string? LineageSignature { get; set; }

        /// <summary>
        /// The processor signature
        /// </summary>
        [JsonPropertyName("psig")]
        public string? ProcessSignature { get; set; }

        /// <summary>
        /// The combined signature
        /// </summary>
        [JsonPropertyName("csig")]
        public string? CombinedSignature { get; set; }

        /// <summary>
        /// The raw signature
        /// </summary>
        [JsonPropertyName("rsig")]
        public string? RawSignature { get; set; }

        [JsonPropertyName("lineageId")]
        public string? LineageId { get; set; }

        [JsonPropertyName("step")]
        public string? Step { get; set; }


        protected string? GenerateSignature(Stream serializerStream)
        {
            string? sig = null;
            using (var sha256 = SHA256.Create())
            {
                using(var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        serializerStream.Position = 0;
                        serializerStream.CopyTo(stream);
                        writer.Flush();
                        stream.Position = 0;
                        var hashBytes = sha256.ComputeHash(stream);
                        sig = Convert.ToBase64String(hashBytes);
                        writer.Close();
                    }
                    stream.Close();
                }
            }
            return sig;
        }

        
        protected string? CreateCombinedSignature(string?[] signatures)
        {
            string? sig = null;
            using (var sha256 = SHA256.Create())
            {
                using(var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        foreach( var signature in signatures)
                        {
                            if(!string.IsNullOrEmpty(signature))
                                writer.Write(signature);
                            else
                                stream.Write(new byte[1] { 0 });
                        }
                            
                        writer.Flush();
                        stream.Position = 0;
                        var hashBytes = sha256.ComputeHash(stream);
                        sig = Convert.ToBase64String(hashBytes);    

                    }
                    stream.Close();
                }
            }
            return sig;
        }

        public bool GenerateSignatures(string? previousLineageSignature = null, string? previousProcessSignature = null)
        {
            if (Message == null)
                return false;

            using(var serializerStream = new MemoryStream())
            {
                using (var serializerWriter = new StreamWriter(serializerStream))
                {
                    serializerWriter.Write(JsonSerializer.Serialize(Message));
                    serializerWriter.Flush();
                    RawSignature = GenerateSignature(serializerStream);
                    LineageSignature = CreateCombinedSignature(new [] { RawSignature, previousLineageSignature, Step });
                    ProcessSignature = CreateCombinedSignature(new [] { RawSignature, previousProcessSignature, Step });
                }
                CombinedSignature = CreateCombinedSignature(new [] { LineageSignature, ProcessSignature, LineageId });
            }
            return true;
        }
    }
}