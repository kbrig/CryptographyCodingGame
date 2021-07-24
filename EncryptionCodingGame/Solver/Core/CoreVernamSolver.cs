using System;
using System.Text;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreVernamSolver : IVernamSolver
    {
        private static string EncodeTo64(string toEncode)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            var returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private static string DecodeFrom64(string encodedData)
        {
            var encodedDataAsBytes = Convert.FromBase64String(encodedData);
            var returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }



        private string DoVernam(string inText, string key)
        {
            string outText = string.Empty;

            for (int i = 0; i < inText.Length; i++)
            {
                outText += (char)(inText[i] ^ key[i % key.Length]);
            }
            return outText;
        }

        public string Decrypt(string ciphertext, string key)
        {
            return DoVernam(DecodeFrom64(ciphertext), key);
        }

        public string Encrypt(string plaintext, string key)
        {
            return EncodeTo64(DoVernam(plaintext, key));
        }
    }
}
