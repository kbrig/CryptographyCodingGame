using System;
using System.Collections;
using System.Collections.Generic;

namespace EncryptionCodingGame
{
    public static class BitArrayExtensions
    {
        public static string ToBinaryString(this BitArray bits)
        {
            var result = "";
            for (int i = 0; i < bits.Length; i++)
            {
                result += bits[i] ? "1" : "0";
            }
            return result;
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            var bytesCount = bits.Length / 8;

            if (bits.Length % 8 != 0)
            {
                bytesCount++;
            }
            var bytes = new byte[bytesCount];

            bits.CopyTo(bytes, 0);
            return bytes;
        }

        public static List<BitArray> Splice(this BitArray bits, int blocksize)
        {
            //Console.WriteLine($"Splicing: {bits.ToBinaryString()}");

            var arrays = new List<BitArray>();
            for (int i = 0; i < bits.Length; i += blocksize)
            {
                var array = new BitArray(blocksize);
                for (int j = 0; j < blocksize; j++)
                {
                    array[j] = bits[i + j];
                }
                arrays.Add(array);

                //Console.WriteLine($"\tBlock\t{i / blocksize}: {array.ToBinaryString()}");
            }
            return arrays;
        }

        public static BitArray FuseBlocks(this List<BitArray> blocks, int blocksize)
        {
            if (blocks == null || blocks.Count == 0)
            {
                return null;
            }

            var buffer = new bool[blocks.Count * blocksize];
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                block.CopyTo(buffer, i * blocksize);
            }
            var result = new BitArray(buffer);
            return result;
        }
    }
}
