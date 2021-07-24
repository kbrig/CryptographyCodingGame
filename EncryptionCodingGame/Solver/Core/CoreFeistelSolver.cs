using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreFeistelSolver : IFeistelSolver
    {
        private const string DUMMY_STRING_TO_TRIM = @"||<END";
        private const int ROUNDS_COUNT = 3;
        private int secretSeed;
        private List<BitArray> keys = new List<BitArray>();

        public CoreFeistelSolver()
        {
            var secretBytes = Encoding.ASCII.GetBytes("MYSECRETISMINE");
            this.secretSeed = BitConverter.ToInt32(secretBytes);
        }

        private void GenerateKeys(int keyCount, int blocksize)
        {
            keys.Clear();
            var random = new Random(this.secretSeed);

            for (int i = 0; i < keyCount; i++)
            {
                var key = new BitArray(blocksize);

                for (int j = 0; j < key.Length; j++)
                {
                    key[j] = random.Next() % 2 == 0;
                }
                Console.WriteLine($"Key {i + 1}: {key.ToBinaryString()}");
                
                keys.Add(key);
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

        private BitArray SwapHalves(BitArray block, int blocksize)
        {
            var halves = block.Splice(blocksize / 2);
            halves.Reverse();

            var newBlock = halves.FuseBlocks(blocksize / 2);
            Console.WriteLine($"{block.ToBinaryString()} => {newBlock.ToBinaryString()}");

            return block;
        }

        private List<BitArray> Feistel(List<BitArray> blocks, int roundsCount, int blocksize)
        {
            var cipherBlocks = new List<BitArray>();

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                for (int j = 0; j < roundsCount; j++)
                {
                    var key = GetKeyForRound(j);

                    block = RoundFunction(block, key);
                }

                block = SwapHalves(block, blocksize);
                cipherBlocks.Add(block);
            }
            return cipherBlocks;
        }

        private BitArray RoundFunction(BitArray block, BitArray key)
        {
            var temp = block.Xor(key);

            return temp;
        }

        public string Encrypt(string plaintext, int blocksize)
        {
            plaintext += DUMMY_STRING_TO_TRIM;
            GenerateKeys(ROUNDS_COUNT, blocksize);

            // Read text as blocks
            var blocks = TextAsBlocks(plaintext, blocksize);

            // Run feistel cipher for ROUNDS_COUNT rounds.
            var cipheredBlocks = Feistel(blocks, ROUNDS_COUNT, blocksize);
            var cipheredBits = cipheredBlocks.FuseBlocks(blocksize);
            var cipheredBytes = cipheredBits.ToByteArray();

            var ciphertext = Convert.ToBase64String(cipheredBytes);
            return ciphertext;
        }

        public string Decrypt(string base64CipherText, int blocksize)
        {
            GenerateKeys(ROUNDS_COUNT, blocksize);

            var ciphertextBytes = Convert.FromBase64String(base64CipherText);

            // Read text as blocks
            var blocks = BytesAsBlocks(ciphertextBytes, blocksize);

            // Run feistel cipher for ROUNDS_COUNT rounds.
            var plainBlocks = Feistel(blocks, ROUNDS_COUNT, blocksize);

            // Read ciphered blocks into a string
            var plainBits = plainBlocks.FuseBlocks(blocksize);
            var plainBytes = plainBits.ToByteArray();
            var plaintext = Encoding.ASCII.GetString(plainBytes);

            // Return string
            return plaintext.Split(DUMMY_STRING_TO_TRIM)[0];
        }
    }
}
