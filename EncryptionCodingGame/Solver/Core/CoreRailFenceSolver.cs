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

        private string EncryptLine(string input, uint startIndex, uint downJump, uint upJump)
        {
            var output = "";
            var isDownJump = true;

            for (uint i = startIndex; i < input.Length; i += isDownJump ? downJump : upJump)
            {
                output += input[(int)i];
                isDownJump = !isDownJump;
            }
            return output;
        }

        public string Encrypt(string plaintext, uint depth)
        {
            var ciphertext = "";
            var indexedDepth = depth - 1;
            var maxJump = indexedDepth * 2;

            for (uint i = 0; i < depth; i++)
            {
                var down = maxJump - (i * 2);
                var up = maxJump - ((indexedDepth - i) * 2);

                ciphertext += EncryptLine(
                    plaintext,
                    i,
                    down == 0 ? maxJump : down,
                    up == 0 ? maxJump : up);
            }
            return ciphertext;
        }
    }
}
