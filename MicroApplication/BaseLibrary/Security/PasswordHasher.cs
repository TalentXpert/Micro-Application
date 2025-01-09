using System.Security.Cryptography;
using System.Text;

namespace BaseLibrary.Security
{
    public class PasswordHasher
    {
        private string _password;
        private int _salt;

        public PasswordHasher(string strPassword, int nSalt)
        {
            _password = strPassword;
            _salt = nSalt;
        }
        public string ComputeSaltedHash()
        {
            // Create Byte array of password string
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(_password);

            // Create a new salt
            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(_salt >> 24);
            _saltBytes[1] = (byte)(_salt >> 16);
            _saltBytes[2] = (byte)(_salt >> 8);
            _saltBytes[3] = (byte)(_salt);

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
            return RandomNumberGenerator.GetInt32(0, 100000);
        }
    }
}
