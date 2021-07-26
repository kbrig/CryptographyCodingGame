﻿using System;
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
            var cipher = Convert.FromBase64String(ciphertext).ToBitArray();
            //Console.WriteLine($"Read Cipher:\t{cipher.ToBinaryString(blocksize)}");
            var blocks = cipher.Splice(blocksize);

            var keyBytes = key.ToByteArray();
            var seed = keyBytes.Sum(x => x);
            var random = new Random(seed);
            var cipherkeyBytes = new byte[blocksize];
            var vectorbytes = new byte[blocksize];
            random.NextBytes(cipherkeyBytes);
            random.NextBytes(vectorbytes);

            var keyblock = new BitArray(cipherkeyBytes);
            var ivblock = new BitArray(vectorbytes);
            keyblock.Length = ivblock.Length = blocksize;
            //Console.WriteLine($"Key:\t{keyblock.ToBinaryString()}");
            //Console.WriteLine($"IV:\t{ivblock.ToBinaryString()}");

            var plainBlocks = new List<BitArray>();
            var lastCipher = ivblock;
            for (int i = 0; i < blocks.Count; i++)
            {
                var cipherBlock = new BitArray(blocks[i]);

                // 𝐷(𝐾,𝐶𝑖)⊕𝐶𝑖−1 where C0 = IV
                var tmp = DecryptFunction(cipherBlock, keyblock);
                var plainBlock = new BitArray(tmp).Xor(lastCipher);

                plainBlocks.Add(plainBlock);


                //Console.WriteLine($"Decryption of ciphertext block {i}:");
                //Console.WriteLine($"\tP{i} = D(K, C{i}) ^ C{i-1} where C-1 = IV");
                //Console.WriteLine($"\tP{i} = {keyblock.ToBinaryString()} ^ {lastCipher.ToBinaryString()} ^ {cipherBlock.ToBinaryString()}");
                //Console.WriteLine($"\tP{i} = {keyblock.ToBinaryString()} ^ {tmp.ToBinaryString()}");
                //Console.WriteLine($"\tP{i} = {cipherBlock.ToBinaryString()}");

                lastCipher = new BitArray(blocks[i]);
            }

            var plain = plainBlocks.FuseBlocks();
            //Console.WriteLine($"Write Plain:\t{plain.ToBinaryString(blocksize)}");
            var plainbytes = plain.ToByteArray();
            var plaintext = Encoding.ASCII.GetString(plainbytes);
            return plaintext;
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
            var plain = plaintext.ToBitArray();
            var blocks = plain.Splice(blocksize);
            //Console.WriteLine($"Read Plain:\t{plain.ToBinaryString(blocksize)}");

            var keyBytes = key.ToByteArray();
            var seed = keyBytes.Sum(x => x);
            var random = new Random(seed);
            var cipherkeyBytes = new byte[blocksize];
            var vectorbytes = new byte[blocksize];
            random.NextBytes(cipherkeyBytes);
            random.NextBytes(vectorbytes);

            var keyblock = new BitArray(cipherkeyBytes);
            var ivblock = new BitArray(vectorbytes);
            keyblock.Length = ivblock.Length = blocksize;
            //Console.WriteLine($"Key:\t{keyblock.ToBinaryString()}");
            //Console.WriteLine($"IV:\t{ivblock.ToBinaryString()}");

            var cipherBlocks = new List<BitArray>();
            var lastCipher = ivblock;
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                // Ci = E(K, Ci-1 ^ Pi) where C0 = IV
                var tmp = new BitArray(lastCipher).Xor(block);
                var cipherBlock = EncryptFunction(tmp, keyblock);
                cipherBlocks.Add(cipherBlock);

                //Console.WriteLine($"Encryption of plaintext block {i}:");
                //Console.WriteLine($"\tC{i} = E(K, C{i - 1} ^ P{i}) where C-1 = IV");
                //Console.WriteLine($"\tC{i} = {keyblock.ToBinaryString()} ^ {lastCipher.ToBinaryString()} ^ {block.ToBinaryString()}");
                //Console.WriteLine($"\tC{i} = {keyblock.ToBinaryString()} ^ {tmp.ToBinaryString()}");
                //Console.WriteLine($"\tC{i} = {cipherBlock.ToBinaryString()}");
                lastCipher = new BitArray(cipherBlock);
            }

            var cipher = cipherBlocks.FuseBlocks();
            //Console.WriteLine($"Write Cipher:\t{cipher.ToBinaryString(blocksize)}");
            return cipher.ToBase64String();
        }
    }
}
