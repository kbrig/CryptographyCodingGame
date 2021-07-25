﻿using System;
using System.Collections.Generic;

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
    }
}
