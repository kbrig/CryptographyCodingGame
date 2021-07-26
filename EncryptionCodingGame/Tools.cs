using System;
using System.Linq;

namespace EncryptionCodingGame
{
    public static class Tools
    {
        public static T ReadInputOrDefault<T>(string prompt = null, T defaultValue = default, Func<string, T> parseFunc = null)
            where T :IConvertible
        {
            T returnValue;
            if (!prompt.IsNullOrEmpty())
            {
                Console.WriteLine($"{prompt} [{defaultValue}] ");
            }
            var input = Console.ReadLine();

            if (input.IsNullOrEmpty())
            {
                returnValue = defaultValue;
            }
            else
            {
                returnValue = parseFunc == null ? (T)Convert.ChangeType(input, typeof(T)) : parseFunc(input);
            }
            return returnValue;
        }

        public static string MultiplyChar(char c, int length)
        {
            return "".PadRight(length, c);
        }

        public static Random GetSeededRandomFromKeyString(string key)
        {
            var keyBytes = key.ToByteArray();
            var seed = keyBytes.Sum(x => x);
            var random = new Random(seed);
            return random;
        }
    }
}
