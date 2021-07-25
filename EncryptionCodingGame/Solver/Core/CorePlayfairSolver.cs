using System;
using System.Linq;

namespace EncryptionCodingGame.Solver.Core
{
    public class CorePlayfairSolver : IPlayfairSolver
    {
        private const int ROW_LENGTH = 5;
        private char[] orderedValues = new char[25];
        private char filler = 'X';

        public CorePlayfairSolver(char fillerCharacter = 'X')
        {
            filler = fillerCharacter;
        }

        private void PrintTable()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("| Cipher  Table |");
            Console.WriteLine("-----------------");

            for (int i = 0; i < orderedValues.Length; i++)
            {
                if (i % ROW_LENGTH == 0)
                {
                    Console.Write($"|");
                }
                Console.Write($" {orderedValues[i]} ");
                if (i % ROW_LENGTH == ROW_LENGTH - 1)
                {
                    Console.WriteLine($"|");
                }
            }
            Console.WriteLine("-----------------");
        }

        private void InitTable(string key)
        {
            Array.Copy(key.ToCharArray(), orderedValues, key.Length);

            var ignore = (key + "J").ToCharArray();
            var remainingLetters = Enumerable.Range('A', 26)
                                             .Select(x => Convert.ToChar(x))
                                             .Where(c => !ignore.Contains(c))
                                             .ToArray();
            Array.Copy(remainingLetters, 0, orderedValues, key.Length, remainingLetters.Length);
            PrintTable();
        }

        public string Decrypt(string ciphertext, string key)
        {
            InitTable(key);
            var plaintext = "";

            for (int i = 0, j = i + 1; i < ciphertext.Length; i += 2, j += 2)
            {
                char c1 = ciphertext[i];
                char c2 = ciphertext[j];

                var pos1 = Array.IndexOf(orderedValues, c1);
                var pos2 = Array.IndexOf(orderedValues, c2);

                var coord1 = new Tuple<int, int>(pos1 % ROW_LENGTH, pos1 / ROW_LENGTH);
                var coord2 = new Tuple<int, int>(pos2 % ROW_LENGTH, pos2 / ROW_LENGTH);
                char p1, p2;

                // SAME ROW?
                if (coord1.Item2 == coord2.Item2)
                {
                    if (coord1.Item1 == 0)
                    {
                        p1 = orderedValues[pos1 + 4];
                    }
                    else
                    {
                        p1 = orderedValues[pos1 - 1];
                    }

                    if (coord2.Item1 == 0)
                    {
                        p2 = orderedValues[pos2 + 4];
                    }
                    else
                    {
                        p2 = orderedValues[pos2 - 1];
                    }
                }
                // SAME COLUMN?
                else if (coord1.Item1 == coord2.Item1)
                {
                    if (coord1.Item2 == 0)
                    {
                        p1 = orderedValues[pos1 + 20];
                    }
                    else
                    {
                        p1 = orderedValues[pos1 - 5];
                    }

                    if (coord2.Item2 == 0)
                    {
                        p2 = orderedValues[pos2 + 20];
                    }
                    else
                    {
                        p2 = orderedValues[pos2 - 5];
                    }
                }
                // OTHERWISE
                else
                {
                    p1 = orderedValues[coord1.Item2 * ROW_LENGTH + coord2.Item1];
                    p2 = orderedValues[coord2.Item2 * ROW_LENGTH + coord1.Item1];
                }

                plaintext += p1;
                plaintext += p2;

            }

            plaintext = plaintext.Trim(filler);
            return plaintext;
        }

        public string Encrypt(string plaintext, string key)
        {
            plaintext = plaintext.Replace(" ", "")
                                 .Replace('J', 'I')
                                 .ToUpper();
            InitTable(key);
            var ciphertext = "";

            for (int i = 0, j = i + 1; i < plaintext.Length; i += 2, j += 2)
            {
                char p1 = plaintext[i], p2;

                // IF WE NEED AN EXTRA CHARACTER
                if (j < plaintext.Length)
                {
                    p2 = plaintext[j];
                }
                else
                {
                    p2 = filler;
                }

                // FIX COMPARISON IF DUPPLICATE
                if (p1 == p2)
                {
                    p2 = filler;
                    plaintext.Insert(j, new string(new [] { filler }));
                }

                var pos1 = Array.IndexOf(orderedValues, p1);
                var pos2 = Array.IndexOf(orderedValues, p2);

                // Tuple.Item1 == x coordinates ; Tuple.Item2 == y coordinates
                var coord1 = new Tuple<int, int>(pos1 % ROW_LENGTH, pos1 / ROW_LENGTH);
                var coord2 = new Tuple<int, int>(pos2 % ROW_LENGTH, pos2 / ROW_LENGTH);
                char c1, c2;
                // SAME ROW?
                if (coord1.Item2 == coord2.Item2)
                {
                    if (coord1.Item1 == 4)
                    {
                        c1 = orderedValues[pos1 - 4];
                    }
                    else
                    {
                        c1 = orderedValues[pos1 + 1];
                    }

                    if (coord2.Item1 == 4)
                    {
                        c2 = orderedValues[pos2 - 4];
                    }
                    else
                    {
                        c2 = orderedValues[pos2 + 1];
                    }
                }
                // SAME COLUMN?
                else if (coord1.Item1 == coord2.Item1)
                {
                    if (coord1.Item2 == 4)
                    {
                        c1 = orderedValues[pos1 - 20];
                    }
                    else
                    {
                        c1 = orderedValues[pos1 + 5];
                    }

                    if (coord2.Item2 == 4)
                    {
                        c2 = orderedValues[pos2 - 20];
                    }
                    else
                    {
                        c2 = orderedValues[pos2 + 5];
                    }
                }
                // OTHERWISE
                else
                {
                    c1 = orderedValues[coord1.Item2 * ROW_LENGTH + coord2.Item1];
                    c2 = orderedValues[coord2.Item2 * ROW_LENGTH + coord1.Item1];
                }

                ciphertext += c1;
                ciphertext += c2;
            }
            return ciphertext;
        }
    }
}
