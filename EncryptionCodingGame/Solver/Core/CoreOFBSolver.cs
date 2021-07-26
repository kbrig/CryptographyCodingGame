using System;
using System.Collections;
using System.Collections.Generic;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreOFBSolver : IOFBSolver
    {
        private BitArray EncryptBlock(BitArray block, BitArray key)
        {
            return new BitArray(block).Xor(key);
        }

        /// <summary>
        /// 𝐼1=IV           a nonce
        /// 𝐼𝑖=𝑂𝑖−1         for 𝑖=2,…,𝑁
        /// 𝑂𝑖=𝐸(𝐾,𝐼𝑖)	    for 𝑖=1,…,𝑁
        /// 𝑃𝑖=𝐶𝑖⊕𝑂𝑖	        for 𝑖=1,…,𝑁−1
        /// 𝑃∗𝑁=𝐶∗𝑁⊕MSB𝑢(𝑂𝑁)
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="key"></param>
        /// <param name="blocksize"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext, string key, int blocksize)
        {
            var cipherBlocks = ciphertext.ToBitArrays(blocksize, isBase64: true, addPadding: false);

            var random = Tools.GetSeededRandomFromKeyString(key);
            var ivblock = random.NextBitArray(blocksize);
            var keyblock = random.NextBitArray(blocksize);

            var nextInput = ivblock;
            var plainBlocks = new List<BitArray>();
            for (int i = 0; i < cipherBlocks.Count; i++)
            {
                var Ci = cipherBlocks[i];
                var Ii = new BitArray(nextInput);
                var Oi = EncryptBlock(Ii, keyblock);

                var Pi = new BitArray(Ci).Xor(Oi);

                plainBlocks.Add(Pi);

                nextInput = new BitArray(Oi);
            }
            return plainBlocks.ConvertToString();
        }

        /// <summary>
        /// 𝐼1=IV	            
        /// 𝐼𝑖=𝑂𝑖−1	            
        /// 𝑂𝑖=𝐸(𝐾,𝐼𝑖)	        
        /// 𝐶𝑖=𝑃𝑖⊕𝑂𝑖	            
        /// 𝐶∗𝑁=𝑃∗𝑁⊕MSB𝑢(𝑂𝑁)
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <param name="blocksize"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext, string key, int blocksize)
        {
            var plainBlocks = plaintext.ToBitArrays(blocksize, addPadding: false);
            var random = Tools.GetSeededRandomFromKeyString(key);
            var ivblock = random.NextBitArray(blocksize);
            var keyblock = random.NextBitArray(blocksize);

            var nextInput = ivblock;
            var cipherBlocks = new List<BitArray>();
            for (int i = 0; i < plainBlocks.Count; i++)
            {
                var plainBlock = plainBlocks[i];
                var Ii = new BitArray(nextInput);
                var Oi = EncryptBlock(Ii, keyblock);

                var cipherBlock = new BitArray(Oi).Xor(plainBlock);

                cipherBlocks.Add(cipherBlock);
                nextInput = new BitArray(Oi);
            }
            return cipherBlocks.ConvertToString(isBase64: true);
        }
    }
}
