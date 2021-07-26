using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionCodingGame
{
    public static class BitArrayExtensions
    {
        #region Manipulations

        public static BitArray MostSignificantBit(this BitArray block, int bitCount = 1)
        {
            return block.Subset(block.Length - bitCount, bitCount);
        }

        public static BitArray LeastSignificantBit(this BitArray block, int bitCount = 1)
        {
            return block.Subset(0, bitCount);
        }

        public static BitArray SwapHalves(this BitArray block)
        {
            var halves = block.Splice();
            halves.Reverse();
            var newBlock = halves.Fuse();
            return newBlock;
        }

        public static List<BitArray> Splice(this BitArray bits, int? blocksize = null)
        {
            var size = bits.Length / 2;
            if (blocksize.HasValue)
            {
                size = blocksize.Value;
            }
            var arrays = new List<BitArray>();
            for (int i = 0; i < bits.Length; i += size)
            {
                var array = bits.Subset(i, size);
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

        public static BitArray Fuse(this IEnumerable<BitArray> blocks)
        {
            if (blocks == null)
            {
                return null;
            }
            return new BitArray(0).Collate(blocks);
        }

        public static BitArray Collate(this BitArray blockA, params BitArray[] blocks)
        {
            return Collate(blockA, blocks.AsEnumerable());
        }

        public static BitArray Collate(this BitArray blockA, IEnumerable<BitArray> blocks)
        {
            var output = new BitArray(blockA);
            output.Length += blocks.Sum(b => b.Length);

            var currentIndex = blockA.Length;
            foreach (var block in blocks)
            {
                for (int i = 0; i < block.Length; i++)
                {
                    output[currentIndex++] = block[i];
                }
            }
            return output;
        }

        public static BitArray RotateLeft(this BitArray block, int bitCount)
        {
            var buffer = new bool[bitCount];
            var output = new BitArray(block);

            for (int i = 0; i < bitCount; i++)
            {
                buffer[buffer.Length - 1 - i] = block[block.Length - 1 - i];
            }

            output.LeftShift(bitCount);

            for (int i = 0; i < bitCount; i++)
            {
                output[i] = buffer[i];
            }
            return output;
        }

        public static BitArray RotateRight(this BitArray block, int bitCount)
        {
            var buffer = new bool[bitCount];
            var output = new BitArray(block);

            for (int i = 0; i < bitCount; i++)
            {
                buffer[i] = block[i];
            }

            output.RightShift(bitCount);

            for (int i = 0; i < bitCount; i++)
            {
                output[output.Length - 1 - i] = buffer[buffer.Length - 1 - i];
            }
            return output;
        }

        #endregion

        #region Tools

        public static string ToBinaryString(this BitArray bits, int groupSize = 0)
        {
            var result = "";
            for (int i = bits.Length - 1; i >= 0; i--)
            {
                result += bits[i] ? "1" : "0";
                if (groupSize != 0 && i % groupSize == 0 && i > 0)
                {
                    result += " ";
                }
            }
            return result;
        }

        #endregion

        #region Conversions

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

        public static List<BitArray> ToBitArrays(this byte[] bytes, int blocksize, bool addPadding = true)
        {
            var bits = new BitArray(bytes);

            if (addPadding)
            {
                var paddingNeeded = blocksize - (bits.Length % blocksize);
                bits.Length += paddingNeeded;
            }
            var arrays = bits.Splice(blocksize);
            return arrays;
        }

        public static int[] ToInt32Array(this BitArray block)
        {
            var array = new int[block.Length / sizeof(int)];

            var paddingNeeded = sizeof(int) - (block.Length % sizeof(int));
            //block
            block.CopyTo(array, 0);
            return array;
        }

        public static BitArray ToBitArray(this string s, bool isBase64 = false)
        {
            var output = isBase64
                ? Convert.FromBase64String(s).ToBitArray()
                : Encoding.ASCII.GetBytes(s).ToBitArray();
            return output;
        }

        public static BitArray ToBitArray(this byte[] bytes)
        {
            return new BitArray(bytes);
        }

        public static string ToBase64String(this BitArray block)
        {
            return block.ToByteArray().ToBase64String();
        }

        #endregion


    }
}
