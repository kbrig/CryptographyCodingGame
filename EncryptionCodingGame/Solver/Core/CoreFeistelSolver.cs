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
            keys = Tools.GetSeededRandomFromKeyString(secret)
                        .NextBitArrays(blocksize / 2, keyCount);
        }

        private BitArray Feistel(BitArray block, int roundsCount)
        {
            var left = block.MostSignificantBits(block.Length / 2);          // RIGHTMOST IN MEMORY
            var right = block.LeastSignificantBits(block.Length / 2);        // LEFTMOST IN MEMORY

            for (int i = 0; i < roundsCount; i++)
            {
                /*
                 * Li+1 = Ri
                 * Ri+1 = Li ^ F(REi ; Ki)
                 */
                var key = keys[i];
                var Li = right;
                var tmpRound = RoundFunction(right, key);
                var Ri = new BitArray(left).Xor(tmpRound);

                left = Li;
                right = Ri;
            }

            var cipherBlock = new BitArray(right).Collate(left); // RIGHTMOST BITS + LEFTMOST BITS
            return cipherBlock;
        }

        private BitArray RoundFunction(BitArray block, BitArray key)
        {
            return new BitArray(block).Xor(key);
        }

        public string Encrypt(string plaintext, string key, int blocksize)
        {
            plaintext += DUMMY_STRING_TO_TRIM;
            GenerateKeys(key, ROUNDS_COUNT, blocksize);

            // Read text as blocks
            var plainBlocks = plaintext.ToBitArrays(blocksize);
            var cipheredBlocks = new List<BitArray>();

            // Run feistel cipher for ROUNDS_COUNT rounds.
            foreach (var block in plainBlocks)
            {
                var cipheredBlock = Feistel(block, ROUNDS_COUNT);
                cipheredBlocks.Add(cipheredBlock);
            }
            
            return cipheredBlocks.ConvertToString(isBase64: true);
        }

        public string Decrypt(string base64CipherText, string key, int blocksize)
        {
            GenerateKeys(key, ROUNDS_COUNT, blocksize);
            keys.Reverse();

            // Read text as blocks
            var blocks = base64CipherText.ToBitArrays(blocksize, isBase64: true);
            var plainBlocks = new List<BitArray>();

            // Run feistel cipher for ROUNDS_COUNT rounds.
            foreach (var block in blocks)
            {
                var plainBlock = Feistel(block, ROUNDS_COUNT);
                plainBlocks.Add(plainBlock);
            }

            // Read ciphered blocks into a string
            var plaintext = plainBlocks.ConvertToString();

            // Return string
            return plaintext.Split(DUMMY_STRING_TO_TRIM)[0];
        }
    }
}
