using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EncryptionCodingGame
{
    public static class GeneralExtensions
    {
        public static T[] GetValues<T>(this T[] array, int startIndex, int length)
        {
            startIndex %= array.Length;
            length = length > array.Length ? array.Length : length;

            var subset = new T[length];

            for (int i = 0; i < length; i++)
            {
                subset[i] = array[startIndex + i];
            }
            return subset;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return s == null || s.Length == 0;
        }

        public static T ValueIfDefault<T>(this T val, T newDefault = default(T)) where T : IConvertible
        {
            if (EqualityComparer<T>.Default.Equals(val, default(T)))
            {
                return newDefault;
            }
            return val;
        }

        public static string FromBase64String(this string s)
        {
            var bytes = Convert.FromBase64String(s);
            var output = Encoding.ASCII.GetString(bytes);
            return output;
        }

        public static string ToBase64String(this string s)
        {
            return s.ToByteArray().ToBase64String();
        }

        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] ToByteArray(this string s, bool isBase64 = false)
        {
            if (isBase64)
            {
                return Convert.FromBase64String(s);
            }
            return Encoding.ASCII.GetBytes(s);
        }

        public static BitArray NextBitArray(this Random random, int blocksize)
        {
            var outputBuffer = new byte[blocksize];
            random.NextBytes(outputBuffer);

            var output = new BitArray(outputBuffer);
            output.Length = blocksize;
            return output;
        }

        public static List<BitArray> NextBitArrays(this Random random, int blocksize, int count)
        {
            var output = new List<BitArray>();

            while (count-- >= 0)
            {
                output.Add(random.NextBitArray(blocksize));
            }
            return output;
        }
    }
}
