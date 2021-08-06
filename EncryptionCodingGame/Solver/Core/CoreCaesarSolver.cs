using System;
using System.Linq;

namespace EncryptionCodingGame.Solver.Core
{
    /// <summary>
    /// Simple substitution cipher.
    /// Apply a "shift" for each letters of your message.
    /// i.e. "HELLO", with a shift of 3, will be "KHOOR"
    /// </summary>
    public class CoreCaesarSolver : BaseCoreSolver<ICaesarSolver, int>, ICaesarSolver
    {
        private char[] alphabet = Enumerable.Range('A', 26).Select(c => Convert.ToChar(c)).ToArray();

        /// <summary>
        /// Takes in an encrypted string and returns its plain text.
        /// </summary>
        /// <param name="ciphertext">Caesar encrypted input</param>
        /// <param name="shift">Shift applied to generate said input</param>
        /// <returns>Plain text</returns>
        public string Decrypt(string ciphertext, int shift)
        {
            string plaintext = "";
            foreach (var character in ciphertext)
            {
                // Adding this neat little thing to cater for punctuation.
                // Therefore we only apply substitution on handled characters.
                if (!alphabet.Contains(character))
                {
                    plaintext += character;
                }
                else
                {
                    var index = character - alphabet.First();

                    index = index - shift;
                    if (index < 0)
                    {
                        index = alphabet.Length + index % alphabet.Length;
                    }
                    plaintext += alphabet[index % alphabet.Length];
                }
            }
            return plaintext;
        }

        /// <summary>
        /// Encrypts plaintext with a Caesar Cipher.
        /// Applies a shift to the letter of the text and return the result.
        /// </summary>
        /// <param name="plaintext">Text to encrypt</param>
        /// <param name="shift">Shift to apply</param>
        /// <returns>Encrypted string</returns>
        public string Encrypt(string plaintext, int shift)
        {
            plaintext = plaintext.ToUpper();
            string ciphertext = "";
            foreach (var character in plaintext)
            {
                if (!alphabet.Contains(character))
                {
                    ciphertext += character;
                }
                else
                {
                    var index = character - alphabet.First();
                    index = (index + shift) % alphabet.Length;

                    ciphertext += alphabet[index];
                }
            }
            return ciphertext;
        }
    }
}
