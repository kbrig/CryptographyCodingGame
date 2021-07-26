using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionCodingGame
{
    public static class BitArrayExtensions
    {
        public static string ToBinaryString(this BitArray bits, int groupSize = 0)
        {
            var result = "";
            for (int i = 0; i < bits.Length; i++)
            {
                result += bits[i] ? "1" : "0";
                if (groupSize != 0 && i % groupSize == groupSize - 1 && i > 0)
                {
                    result += " ";
                }
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

        public static List<BitArray> ToBitArrays(this byte[] bytes, int blocksize)
        {
            var bits = new BitArray(bytes);
            var paddingNeeded = blocksize - (bits.Length % blocksize);

            bits.Length += paddingNeeded;
            var arrays = bits.Splice(blocksize);
            return arrays;
        }

        public static BitArray SwapHalves(this BitArray block)
        {
            var halves = block.Splice();
            halves.Reverse();
            var newBlock = halves.Fuse();
            return newBlock;
        }

        public static List<BitArray> Splice(this BitArray bits, int? blocksizeOverride = null)
        {
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
            }
            return arrays;
        }

        public static BitArray Subset(this BitArray bits, int startIndex, int length)
        {
            var output = new BitArray(length);

            for (int i = 0; i < length && startIndex + i < bits.Length; i++)
            {
                output[i] = bits[startIndex + i];
            }
            return output;
        }

        public static BitArray Fuse(this IEnumerable<BitArray> blocks, int? blocksizeOverride = null)
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
            return blocks.Fuse();
        }

        public static BitArray ToBitArray(this string s)
        {
            return new BitArray(Encoding.ASCII.GetBytes(s));
        }

        public static BitArray ToBitArray(this byte[] bytes)
        {
            return new BitArray(bytes);
        }

        public static string ToBase64String(this BitArray block)
        {
            return Convert.ToBase64String(block.ToByteArray());
        }
    }
}
