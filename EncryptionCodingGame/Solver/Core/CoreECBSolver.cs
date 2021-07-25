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
            var cipher = ciphertext.FromBase64().ToBitArray();

            keyBits.Length = blocksize;
            var blocks = cipher.Splice(blocksize);

            foreach (var block in blocks)
            {
                block.Xor(keyBits);
            }

            var plain = blocks.FuseBlocks();
            var plainBytes = plain.ToByteArray();
            var plaintext = Encoding.ASCII.GetString(plainBytes);
            return plaintext;
        }

        public string Encrypt(string plaintext, string key, int blocksize)
        {
            var keyBits = key.ToBitArray();
            var plain = plaintext.ToBitArray();

            keyBits.Length = blocksize;
            var blocks = plain.Splice(blocksize);
            
            foreach (var block in blocks)
            {
                block.Xor(keyBits);
            }

            var cipher = blocks.FuseBlocks();
            var cipherBytes = cipher.ToByteArray();
            var cipherText = Convert.ToBase64String(cipherBytes);
            return cipherText;
        }
    }
}
