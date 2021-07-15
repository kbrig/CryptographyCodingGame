﻿using System;
using System.Linq;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreCaesarSolver : ICaesarSolver
    {
        private char[] alphabet = Enumerable.Range('A', 26).Select(c => Convert.ToChar(c)).ToArray();

        public string Decrypt(string ciphertext, int shift)
        {
            string plaintext = "";
            foreach (var character in ciphertext)
            {
                var index = character - alphabet.First();

                index = (index - shift);
                if (index < 0)
                {
                    index = alphabet.Length + index;
                }
                plaintext += alphabet[index];
            }
            return plaintext;
        }

        public string Encrypt(string plaintext, int shift)
        {
            plaintext = plaintext.Replace(" ", "").ToUpper();

            string ciphertext = "";
            foreach (var character in plaintext)
            {
                var index = character - alphabet.First();
                index = (index + shift) % alphabet.Length;

                ciphertext += alphabet[index];
            }
            return ciphertext;
        }
    }
}
