using System;
namespace EncryptionCodingGame.Solver.Core
{
    public class CoreRailFenceSolver : IRailFenceSolver
    {
        public string Decrypt(string ciphertext, uint depth)
        {
            var plain = new char[ciphertext.Length];

            var startIndex = 0u;
            var inJump = (depth - 1) * 2;
            var outJump = 0u;
            var isInJump = true;
            var currentIndex = 0;

            do
            {
                var jumpingIndex = startIndex;
                while (jumpingIndex < ciphertext.Length)
                {
                    var ci = ciphertext[currentIndex++];
                    plain[jumpingIndex] = ci;

                    if (inJump == 0)
                    {
                        jumpingIndex += outJump;
                    }
                    else if (outJump == 0)
                    {
                        jumpingIndex += inJump;
                    }
                    else
                    {
                        jumpingIndex += isInJump ? inJump : outJump;
                        isInJump = !isInJump;
                    }
                }
                inJump -= 2;
                outJump += 2;
                startIndex++;
                isInJump = true;
            } while (currentIndex < ciphertext.Length);

            var plaintext = new string(plain);
            return plaintext;
        }

        public string Encrypt(string plaintext, uint depth)
        {
            var rail = new string[depth];
            for (int i = 0; i < depth; i++)
            {
                rail[i] = "";
            }

            var currentDepth = 0;
            var dir = 1;
            for (int i = 0; i < plaintext.Length; i++)
            {
                rail[currentDepth] += plaintext[i];

                if (currentDepth == depth - 1 || (currentDepth == 0 && i != 0))
                {
                    dir *= -1;
                }
                currentDepth += dir;
            }

            var ciphertext = string.Concat(rail);
            return ciphertext;
        }
    }
}
