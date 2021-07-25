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
                for (uint i = startIndex; i < ciphertext.Length; i += isInJump ? inJump.ValueIfDefault(outJump) : outJump.ValueIfDefault(inJump))
                {
                    var ci = ciphertext[currentIndex++];
                    plain[i] = ci;
                    isInJump = !isInJump;
                }

                inJump -= 2;
                outJump += 2;
                startIndex++;
                isInJump = true;
            } while (currentIndex < ciphertext.Length);

            var plaintext = new string(plain);
            return plaintext;
        }

        private string EncryptLine(string input, uint startIndex, uint inJump, uint outJump)
        {
            var output = "";
            var isInJump = false;

            for (uint i = startIndex; i < input.Length; i += isInJump ? inJump : outJump)
            {
                output += input[(int)i];
                isInJump = !isInJump;
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
                var inJump = maxJump - (i * 2);
                var outJump = maxJump - ((indexedDepth - i) * 2);

                ciphertext += EncryptLine(
                    plaintext,
                    i,
                    inJump.ValueIfDefault(maxJump),
                    outJump.ValueIfDefault(maxJump));
            }
            return ciphertext;
        }
    }
}
