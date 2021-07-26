using System.Text;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreVernamSolver : IVernamSolver
    {
        private byte[] DoVernam(byte[] inText, byte[] key)
        {
            var output = new byte[inText.Length];

            for (int i = 0; i < inText.Length; i++)
            {
                output[i] = (byte)(inText[i] ^ key[i % key.Length]);
            }
            return output;
        }

        public string Decrypt(string ciphertext, string key)
        {
            var cipherBytes = ciphertext.ToByteArray(isBase64: true);
            var keyBytes = key.ToByteArray();

            var vernamedBytes = DoVernam(cipherBytes, keyBytes);

            return Encoding.ASCII.GetString(vernamedBytes);
        }

        public string Encrypt(string plaintext, string key)
        {
            var plainBytes = plaintext.ToByteArray();
            var keyBytes = key.ToByteArray();

            var vernamedBytes = DoVernam(plainBytes, keyBytes);

            return vernamedBytes.ToBase64String();
        }
    }
}
