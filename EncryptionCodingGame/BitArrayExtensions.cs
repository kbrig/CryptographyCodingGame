using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public static BitArray SwapHalves(this BitArray block)
        {
            var halves = block.Splice();
            halves.Reverse();
            var newBlock = halves.FuseBlocks();
            //Console.WriteLine($"{block.ToBinaryString()} => {newBlock.ToBinaryString()}");
            return newBlock;
        }

        public static List<BitArray> Splice(this BitArray bits, int? blocksizeOverride = null)
        {
            //Console.WriteLine($"Splicing: {bits.ToBinaryString()}");
            var blocksize = bits.Length / 2;
            if (blocksizeOverride.HasValue)
            {
                blocksize = blocksizeOverride.Value;
            }
            var arrays = new List<BitArray>();
            for (int i = 0; i < bits.Length; i += blocksize)
            {
                var array = bits.Subset(i, blocksize);
                arrays.Add(array);

                //Console.WriteLine($"\tBlock\t{i / blocksize}: {array.ToBinaryString()}");
            }
            return arrays;
        }

        public static BitArray Subset(this BitArray bits, int startIndex, int length)
        {
            var output = new BitArray(length);

            for (int i = 0; i < length; i++)
            {
                output[i] = bits[startIndex + i];
            }
            return output;
        }

        public static BitArray FuseBlocks(this IEnumerable<BitArray> blocks, int? blocksizeOverride = null)
        {
            if (blocks == null)
            {
                return null;
            }
            var count = blocks.Count();
            if (count == 0)
            {
                return new BitArray(0);
            }
            var blocksize = blocks.First().Length;
            if (blocksizeOverride.HasValue)
            {
                blocksize = blocksizeOverride.Value;
            }

            var buffer = new bool[count * blocksize];
            for (int i = 0; i < count; i++)
            {
                var block = blocks.ElementAt(i);

                block.CopyTo(buffer, i * blocksize);
            }
            var result = new BitArray(buffer);
            return result;
        }

        public static int[] ToInt32Array(this BitArray block)
        {
            var array = new int[block.Length / sizeof(int)];
            block.CopyTo(array, 0);
            return array;
        }

        public static BitArray Concat(this BitArray blockA, BitArray blockB)
        {
            var blocks = new List<BitArray> { blockA, blockB };
            return blocks.FuseBlocks();
        }
    }
}
