using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreCTRSolver : ICTRSolver
    {
        private const int COUNTER_DEFAULT = 1025;
        private int counter = COUNTER_DEFAULT;

        public string Decrypt(string ciphertext, int nonce, int blocksize)
        {
            var blocks = ciphertext.ToBitArrays(blocksize, isBase64: true, addPadding: false);
            var plainBlocks = new List<BitArray>();

            for (int i = 0; i < blocks.Count; i++)
            {
                var cipherblock_i = new BitArray(blocks[i]);
                var nonce_i = nonce + counter++;

                var tmp = EncryptFunction(nonce_i, blocksize);
                var plainBlock = tmp.Xor(cipherblock_i);

                plainBlocks.Add(plainBlock);
            }
            counter = COUNTER_DEFAULT;
            return plainBlocks.ConvertToString();
        }

        private BitArray DecryptFunction(BitArray block, BitArray key)
        {
            return new BitArray(block).Xor(key);
        }

        private BitArray EncryptFunction(int n, int blocksize)
        {
            var output = new BitArray(new[] { n });
            output.Length = blocksize;
            return output;
        }

        public string Encrypt(string plaintext, int nonce, int blocksize)
        {
            var blocks = plaintext.ToBitArrays(blocksize, addPadding: false);
            var cipherBlocks = new List<BitArray>();

            for (int i = 0; i < blocks.Count; i++)
            {
                var plainblock_i = blocks[i];
                var nonce_i = nonce + counter++;

                var tmp = EncryptFunction(nonce_i, blocksize);
                var cipherBlock = tmp.Xor(plainblock_i);
                cipherBlocks.Add(cipherBlock);
            }
            counter = COUNTER_DEFAULT;
            return cipherBlocks.ConvertToString(isBase64: true);
        }
    }
}
