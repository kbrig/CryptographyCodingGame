namespace EncryptionCodingGame.Solver.Core
{
    public class CoreVernamSolver : IVernamSolver
    {
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
            return DoVernam(ciphertext.FromBase64(), key);
        }

        public string Encrypt(string plaintext, string key)
        {
            return DoVernam(plaintext, key).ToBase64();
        }
    }
}
