using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionCodingGame.Solver.Core
{

    /*

    Encryption:         Decryption:	
    𝐼1=IV	            𝐼1=IV	
    𝐼𝑖=LSB𝑏−𝑠(𝐼𝑖−1)∥𝐶𝑖−1	𝐼𝑖=LSB𝑏−𝑠(𝐼𝑖−1)∥𝐶𝑖−1	for 𝑖=2,…,𝑁
    𝑂𝑖=𝐸(𝐾,𝐼𝑖)	        𝑂𝑖=𝐸(𝐾,𝐼𝑖)	        for 𝑖=1,…,𝑁
    𝐶𝑖=𝑃𝑖⊕MSB𝑠(𝑂𝑖)	    𝑃𝑖=𝐶𝑖⊕MSB𝑠(𝑂𝑖)	    for 𝑖=1,…,𝑁 
    */
    public class CoreCFBSolver : ICFBSolver
    {
        /// <summary>
        /// 𝐼1=IV	
        /// 𝐼𝑖=LSB𝑏−𝑠(𝐼𝑖−1)∥𝐶𝑖−1     for 𝑖=2,…,𝑁
        /// 𝑂𝑖=𝐸(𝐾,𝐼𝑖)	          for 𝑖=1,…,𝑁
        /// 𝑃𝑖=𝐶𝑖⊕MSB𝑠(𝑂𝑖)	      for 𝑖=1,…,𝑁
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="key"></param>
        /// <param name="blocksize"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext, string key, int blocksize)
        {
            var cipher = ciphertext.ToBitArray(isBase64: true);
            var cipherBlocks = cipher.Splice(blocksize);

            var random = Tools.GetSeededRandomFromKeyString(key);
            var ivblock = random.NextBitArray(blocksize + 1);


            var nextInput = ivblock;
            var plainBlocks = new List<BitArray>();
            for (int i = 0; i < cipherBlocks.Count; i++)
            {
                var Ci = cipherBlocks[i];
                var Ii = new BitArray(nextInput);
                var Oi = EncryptBlock(Ii);
                var OiMSBs = new BitArray(Oi).MostSignificantBits(blocksize);
                var Pi = new BitArray(Ci).Xor(OiMSBs);

                plainBlocks.Add(Pi);

                var IiLSBs = Ii.LeastSignificantBits();
                nextInput = IiLSBs.Collate(Ci);
            }

            var plain = plainBlocks.Fuse();
            var plainBytes = plain.ToByteArray();
            var cipherText = Encoding.ASCII.GetString(plainBytes);
            return cipherText;
        }


        /// <summary>
        /// Encryption:       
        /// 𝐼1=IV	          
        /// 𝐼𝑖=LSB𝑏−𝑠(𝐼𝑖−1)∥𝐶𝑖−1   for 𝑖=2,…,𝑁
        /// 𝑂𝑖=𝐸(𝐾,𝐼𝑖)	        for 𝑖=1,…,𝑁
        /// 𝐶𝑖=𝑃𝑖⊕MSB𝑠(𝑂𝑖)        for 𝑖=1,…,𝑁
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <param name="blocksize"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext, string key, int blocksize)
        {
            var plain = plaintext.ToBitArray();

            var plainBlocks = plain.Splice(blocksize);

            var random = Tools.GetSeededRandomFromKeyString(key);
            var ivblock = random.NextBitArray(blocksize + 1);

            var nextInput = ivblock;
            var cipherBlocks = new List<BitArray>();
            for (int i = 0; i < plainBlocks.Count; i++)
            {
                var plainBlock = plainBlocks[i];
                var Ii = new BitArray(nextInput);
                var Oi = EncryptBlock(Ii);

                var msbs = new BitArray(Oi).MostSignificantBits(blocksize);
                var cipherBlock = new BitArray(msbs).Xor(plainBlock);

                cipherBlocks.Add(cipherBlock);

                nextInput = Ii.LeastSignificantBits(1).Collate(cipherBlock);
            }

            var cipher = cipherBlocks.Fuse();
            var cipherText = cipher.ToBase64String();
            return cipherText;
        }

        private BitArray EncryptBlock(BitArray block)
        {
            return block.RotateLeft(1);
        }
    }
}
