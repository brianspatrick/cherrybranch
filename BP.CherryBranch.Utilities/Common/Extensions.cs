namespace Lucet.CherryBranch.Utilities.Common
{
    using System.Security.Cryptography;
    using System.Text;

    public static class Extensions
    {
        public static string? Clean(this string? textValue) => textValue?.Replace(@"(", string.Empty).Replace(@")", string.Empty).Replace(@"-", string.Empty).Replace(@" ", string.Empty);

        public static string GenerateHash(this string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = SHA256.HashData(bytes);

            return Convert.ToHexString(hashBytes);
        }

        #region Encode/Decode Text

        public static string Encode(this string? text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return Convert.ToHexString(text.ToBytes());
        }

        public static string Decode(this string? text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return Encoding.UTF8.GetString(Convert.FromHexString(text));
        }

        #endregion Encode/Decode Text

        #region Encrypt/Decrypt Text

        public static string Encrypt(this string? text)
        {
            return EncryptString(text);
        }

        public static string Decrypt(this string? text)
        {
            return DecryptString(text);
        }

        #region Private

        private static readonly string EncryptionPassword = "mthr33$ixtyd@t@!";
        private static readonly byte[] InitVector =
        [
            0x09, 0x10, 0x02, 0x12, 0x13, 0x14, 0x15, 0x07,
            0x01, 0x05, 0x03, 0x04, 0x11, 0x06, 0x16, 0x08,
        ];

        private static string EncryptString(string? text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            using Aes aes = AesCreate();
            using MemoryStream memstream = new();
            using CryptoStream cryptoStream = new(memstream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            cryptoStream.Write(text.ToBytes());
            cryptoStream.FlushFinalBlock();

            // Swap slashes with pipes to be Uri safe
            return memstream.ToBase64String().Replace(@"/", @"|");
        }

        private static string DecryptString(string? base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return string.Empty;

            var bytes = Convert.FromBase64String(base64String.Replace(@"|", @"/"));

            using Aes aes = AesCreate();
            using MemoryStream inputStream = new(bytes);
            using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream outputStream = new();

            cryptoStream.CopyTo(outputStream);

            return outputStream.ToCleanString();
        }

        private static Aes AesCreate()
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16;
            var hashMethod = HashAlgorithmName.SHA384;

            Aes aes = Aes.Create();
            aes.Key = Rfc2898DeriveBytes.Pbkdf2(EncryptionPassword.AsSpan(), emptySalt.AsSpan(), iterations, hashMethod, desiredKeyLength);
            aes.IV = InitVector;

            return aes;
        }

        private static byte[] ToBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        private static string ToBase64String(this MemoryStream stream)
        {
            return Convert.ToBase64String(stream.ToArray());
        }

        private static string ToCleanString(this MemoryStream stream)
        {
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        #endregion Private

        #endregion Encrypt/Decrypt Text
    }
}
