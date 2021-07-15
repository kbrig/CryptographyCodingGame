using System.Collections.Generic;
using System.Linq;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreVigenereSolver : IVigenereSolver
    {
        private Dictionary<char, char[]> map;

        public CoreVigenereSolver()
        {
            InitMap();
        }

        private void InitMap()
        {
            map = new Dictionary<char, char[]>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var offset = c - 'A';

                map.Add(c, Enumerable.Range(c, 'Z' - c + 1)
                                     .ToList()
                                     .Concat(Enumerable.Range('A', offset))
                                     .Select(x => (char)x)
                                     .ToArray());
            }
        }

        public string Decrypt(string ciphertext, string key)
        {
            var plaintext = "";
            for (int i = 0; i < ciphertext.Length; i++)
            {
                var ci = ciphertext[i];
                var ki = key[i % key.Length];
                var kindex = ki - 'A';

                foreach (var pair in map)
                {
                    if (pair.Value[kindex] == ci)
                    {
                        plaintext += pair.Key;
                        break;
                    }
                }
            }
            return plaintext;
        }

        public string Encrypt(string plaintext, string key)
        {
            var ciphertext = "";

            for (int i = 0; i < plaintext.Length; i++)
            {
                var pi = plaintext[i];
                var ki = key[i % key.Length];
                var ci = map[pi][ki - 'A'];

                ciphertext += ci;
            }
            return ciphertext;
        }
    }
}
