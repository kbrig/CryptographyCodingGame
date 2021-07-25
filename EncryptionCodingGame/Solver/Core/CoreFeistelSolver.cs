using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreFeistelSolver : IFeistelSolver
    {
        private const string DUMMY_STRING_TO_TRIM = @"||<END";
        private const int ROUNDS_COUNT = 3;
        private List<BitArray> keys = new List<BitArray>();

        private void GenerateKeys(string secret, int keyCount, int blocksize)
        {
            keys.Clear();

            var secretBytes = Encoding.ASCII.GetBytes(secret);
            var seed = secretBytes.Sum(x => x);
            var random = new Random(seed);

            for (int i = 0; i < keyCount; i++)
            {
                var key = new BitArray(blocksize / 2);

                for (int j = 0; j < key.Length; j++)
                {
                    key[j] = random.Next() % 2 == 1;
                }
                keys.Add(key);

                Console.WriteLine($"\t Key {i}: {key.ToBinaryString()}");
            }
        }

        /// <summary>
        /// Returns key for round i
        /// </summary>
        /// <param name="round">Which round of encryption are we in? (starts at 0 for round 1)</param>
        /// <returns>The key as a long</returns>
        private BitArray GetKeyForRound(int round)
        {
            return keys[round];
        }

        /// <summary>
        /// Converts input text as a two-dimensional byte array
        /// </summary>
        /// <param name="text">Text to convert</param>
        /// <param name="blocksize">size of blocks (in bits)</param>
        /// <returns>List of blocks</returns>
        private List<BitArray> TextAsBlocks(string text, int blocksize)
        {
            var textBytes = Encoding.ASCII.GetBytes(text);
            return BytesAsBlocks(textBytes, blocksize);
        }

        private List<BitArray> BytesAsBlocks(byte[] bytes, int blocksize)
        {
            var bits = new BitArray(bytes);
            var paddingNeeded = blocksize - (bits.Length % blocksize);

            bits.Length += paddingNeeded;
            var blocks = bits.Splice(blocksize);
            return blocks;
        }

        private BitArray Feistel(BitArray block, int roundsCount)
        {
            var halves = block.Splice();
            var left = halves[0];
            var right = halves[1];

            for (int i = 0; i < roundsCount; i++)
            {
                /*
                 * Li+1 = Ri
                 * Ri+1 = Li ^ F(REi ; Ki)
                 */
                var key = GetKeyForRound(i);
                var tmpLeft = new BitArray(left);
                var tmpRound = RoundFunction(right, key);
                var tmpRight = tmpLeft.Xor(tmpRound);

                Console.Write($"R{i} = {left.ToBinaryString()} ^ F({right.ToBinaryString()},{key.ToBinaryString()}) = {left.ToBinaryString()} ^ {tmpRound.ToBinaryString()} = ");
                Console.WriteLine(tmpRight.ToBinaryString());

                left = right;
                right = tmpRight;
            }

            var cipherBlock = new BitArray(right.Concat(left));
            return cipherBlock;
        }

        private BitArray RoundFunction(BitArray block, BitArray key)
        {
            var tmp = new BitArray(block);
            tmp.Xor(key);
            return tmp;
        }

        public string Encrypt(string plaintext, string key, int blocksize)
        {
            plaintext += DUMMY_STRING_TO_TRIM;
            GenerateKeys(key, ROUNDS_COUNT, blocksize);

            // Read text as blocks
            var blocks = TextAsBlocks(plaintext, blocksize);
            var cipheredBlocks = new List<BitArray>();

            // Run feistel cipher for ROUNDS_COUNT rounds.
            foreach (var block in blocks)
            {
                var cipheredBlock = Feistel(block, ROUNDS_COUNT);
                cipheredBlocks.Add(cipheredBlock);
            }
            
            var cipheredBits = cipheredBlocks.FuseBlocks(blocksize);
            var cipheredBytes = cipheredBits.ToByteArray();

            var ciphertext = Convert.ToBase64String(cipheredBytes);
            return ciphertext;
        }

        public string Decrypt(string base64CipherText, string key, int blocksize)
        {
            GenerateKeys(key, ROUNDS_COUNT, blocksize);
            keys.Reverse();

            var ciphertextBytes = Convert.FromBase64String(base64CipherText);

            // Read text as blocks
            var blocks = BytesAsBlocks(ciphertextBytes, blocksize);
            var plainBlocks = new List<BitArray>();

            // Run feistel cipher for ROUNDS_COUNT rounds.
            foreach (var block in blocks)
            {
                var plainBlock = Feistel(block, ROUNDS_COUNT);
                plainBlocks.Add(plainBlock);
            }

            // Read ciphered blocks into a string
            var plainBits = plainBlocks.FuseBlocks(blocksize);
            var plainBytes = plainBits.ToByteArray();
            var plaintext = Encoding.ASCII.GetString(plainBytes);

            // Return string
            return plaintext.Split(DUMMY_STRING_TO_TRIM)[0];
        }
    }
}
