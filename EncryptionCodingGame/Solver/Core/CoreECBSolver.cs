using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreECBSolver : IECBSolver
    {
        public string Decrypt(string ciphertext, string key, int blocksize)
        {
            var keyBits = key.ToBitArray();
            keyBits.Length = blocksize;

            var blocks = ciphertext.ToBitArrays(blocksize);
            foreach (var block in blocks)
            {
                block.Xor(keyBits);
            }
            return blocks.ConvertToString();
        }

        public string Encrypt(string plaintext, string key, int blocksize)
        {
            var keyBits = key.ToBitArray();
            keyBits.Length = blocksize;

            var blocks = plaintext.ToBitArrays(blocksize);
            
            foreach (var block in blocks)
            {
                block.Xor(keyBits);
            }
            return blocks.ConvertToString(isBase64: true);
        }
    }
}
