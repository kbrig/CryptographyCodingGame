using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreCBCSolver : ICBCSolver
    {
        public string Decrypt(string ciphertext, string key, int blocksize)
        {
            var blocks = ciphertext.ToBitArrays(blocksize, isBase64: true, addPadding: false);

            var random = Tools.GetSeededRandomFromKeyString(key);
            var keyblock = random.NextBitArray(blocksize);
            var ivblock = random.NextBitArray(blocksize);

            var plainBlocks = new List<BitArray>();
            var lastCipher = ivblock;
            for (int i = 0; i < blocks.Count; i++)
            {
                var cipherBlock = new BitArray(blocks[i]);

                // 𝐷(𝐾,𝐶𝑖)⊕𝐶𝑖−1 where C0 = IV
                var tmp = DecryptFunction(cipherBlock, keyblock);
                var plainBlock = tmp.Xor(lastCipher);

                plainBlocks.Add(plainBlock);

                lastCipher = new BitArray(blocks[i]);
            }

            return plainBlocks.ConvertToString();
        }

        private BitArray DecryptFunction(BitArray block, BitArray key)
        {
            return new BitArray(block).Xor(key);
        }

        private BitArray EncryptFunction(BitArray block, BitArray key)
        {
            return new BitArray(block).Xor(key);
        }

        public string Encrypt(string plaintext, string key, int blocksize)
        {
            var blocks = plaintext.ToBitArrays(blocksize, addPadding: false);

            var random = Tools.GetSeededRandomFromKeyString(key);
            var keyblock = random.NextBitArray(blocksize);
            var ivblock = random.NextBitArray(blocksize);

            var cipherBlocks = new List<BitArray>();
            var lastCipher = ivblock;
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                // Ci = E(K, Ci-1 ^ Pi) where C0 = IV
                var tmp = new BitArray(lastCipher).Xor(block);
                var cipherBlock = EncryptFunction(tmp, keyblock);
                cipherBlocks.Add(cipherBlock);

                lastCipher = new BitArray(cipherBlock);
            }

            return cipherBlocks.ConvertToString(isBase64: true);
        }
    }
}
