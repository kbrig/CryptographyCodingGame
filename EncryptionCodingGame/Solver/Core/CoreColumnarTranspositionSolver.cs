using System;
using System.Linq;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreColumnarTranspositionSolver : IColumnarTranspositionSolver
    {
        public CoreColumnarTranspositionSolver()
        {
        }

        private void TransposeChar(ref char[] destination, int index, char c)
        {
            destination[index] = c;
        }

        private string TransposeText(string input, string key, Action<char[], int, string, int> doTranspose)
        {
            var columnCount = key.Max() - '0';
            var buffer = new char[input.Length];
            var rowCount = input.Length / columnCount;

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    var colFactor = key[i] - '1';
                    var cindex = colFactor * rowCount + j;
                    var pindex = j * columnCount + i;

                    doTranspose(buffer, pindex, input, cindex);
                }
            }

            var output = new string(buffer).TrimEnd('X');
            return output;
        }

        public string Decrypt(string ciphertext, string key, uint transpositionCount)
        {
            for (int i = 0; i < transpositionCount; i++)
            {
                ciphertext = TransposeText(ciphertext, key, (buffer, pindex, input, cindex) => TransposeChar(ref buffer, pindex, input[cindex]));
            }
            var output = ciphertext.TrimEnd('X');
            return output;
        }

        public string Encrypt(string plaintext, string key, uint transpositionCount)
        {
            var columnCount = key.Max() - '0';
            var output = plaintext.PadRight((plaintext.Length / columnCount + 1) * columnCount, 'X');

            for (int i = 0; i < transpositionCount; i++)
            {
                output = TransposeText(output, key, (buffer, pindex, input, cindex) => TransposeChar(ref buffer, cindex, input[pindex]));
            }

            return output;
        }
    }
}
