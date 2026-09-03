
using System.Security.Cryptography;
using System;
using System.IO;
using System.Text;


namespace BaseLibrary.Utilities
{
    public class Security
    {
        CryptoEngine? cryptoEngine;
        public CryptoEngine CryptoEngine { get { return cryptoEngine ?? (cryptoEngine = new CryptoEngine("sblw-3hn8-sqoy19")); } }

        PasswordHasher? passwordHasher;
        public PasswordHasher PasswordHasher { get { return passwordHasher ?? (passwordHasher = new PasswordHasher()); } }

    }
    public class CryptoEngine
    {
        string _key;
        public CryptoEngine(string key)
        {
            _key = key;
        }
        public string Encrypt(string input)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(_key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Decrypt(string input)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(_key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }



    public class AesGcmEncryption
    {
        // Key must be exactly 32 bytes (256 bits) for AES-256
        private static readonly byte[] Key = RandomNumberGenerator.GetBytes(32);

        public static (byte[] CipherText, byte[] Nonce, byte[] Tag) Encrypt(string plainText)
        {
            byte[] plainBytes = GetBytes(plainText);

            // AES-GCM requires a 12-byte (96-bit) Nonce (similar to an IV)
            byte[] nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            // Tag ensures data integrity (detects tampering)
            byte[] tag = new byte[16];
            byte[] cipherText = new byte[plainBytes.Length];

            using (AesGcm aesGcm = new AesGcm(Key, tagSizeInBytes: 16))
            {
                aesGcm.Encrypt(nonce, plainBytes, cipherText, tag);
            }

            return (cipherText, nonce, tag);
        }

        public static string Decrypt(byte[] cipherText, byte[] nonce, byte[] tag)
        {
            byte[] decryptedBytes = new byte[cipherText.Length];

            using (AesGcm aesGcm = new AesGcm(Key, tagSizeInBytes: 16))
            {
                aesGcm.Decrypt(nonce, cipherText, tag, decryptedBytes);
            }

            return GetString(decryptedBytes);
        }
        public static string GetString(byte[] bytes)
        {
            var text = Encoding.UTF8.GetString(bytes);
            return text;
        }
        public static byte[] GetBytes(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return bytes;
        }
    }


    public class PasswordHasher
    {
        public string ComputeSaltedHash(string password, int salt)
        {
            // Create Byte array of password string
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(password);

            // Create a new salt
            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(salt >> 24);
            _saltBytes[1] = (byte)(salt >> 16);
            _saltBytes[2] = (byte)(salt >> 8);
            _saltBytes[3] = (byte)(salt);

            // append the two arrays
            Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }

        public static int CreateRandomSalt()
        {
            Byte[] _saltBytes = new Byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_saltBytes);

            return ((((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) +
              (((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]));
        }
    }
}
