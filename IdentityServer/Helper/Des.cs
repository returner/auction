using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Helper
{
    public class AESEncryption
    {
        private readonly string _cipherKey;

        public AESEncryption(string cipherKey) => _cipherKey = cipherKey;

        public string Encrypt(string input)
        {
            try
            {
                var aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.KeySize = 128;
                aes.BlockSize = 128;
                var pwdBytes = Encoding.UTF8.GetBytes(_cipherKey);
                var keyBytes = new byte[16];

                var len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }

                Array.Copy(pwdBytes, keyBytes, len);
                aes.Key = keyBytes;
                aes.IV = keyBytes;
                var transform = aes.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(input);

                return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Decrypt(string encodedString)
        {
            try
            {
                var aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.KeySize = 128;
                aes.BlockSize = 128;

                var encryptedData = Convert.FromBase64String(encodedString);
                var pwdBytes = Encoding.UTF8.GetBytes(_cipherKey);
                var keyBytes = new byte[16];
                
                var len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }

                Array.Copy(pwdBytes, keyBytes, len);
                aes.Key = keyBytes;
                aes.IV = keyBytes;
                byte[] plainText = aes.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
