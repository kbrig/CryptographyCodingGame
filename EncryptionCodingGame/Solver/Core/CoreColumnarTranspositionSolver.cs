using System;

namespace EncryptionCodingGame.Solver.Core
{
    public class CoreColumnarTranspositionSolver : IColumnarTranspositionSolver
    {
        private string TransposeText(string input, string key, Action<string, int, char[], int> doTranspose)
        {
            var columnCount = key.Length;
            var buffer = new char[input.Length];
            var rowCount = input.Length / columnCount;

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                var colFactor = key[columnIndex] - '0' - 1;

                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var pindex = rowIndex * columnCount + colFactor;
                    var cindex = columnIndex * rowCount + rowIndex;

                    doTranspose(input, pindex, buffer, cindex);
                }
            }

            var output = new string(buffer);
            return output;
        }

        public string Decrypt(string ciphertext, string key)
        {
            ciphertext = TransposeText(
                ciphertext,
                key,
                (input, pindex, buffer, cindex) => buffer[pindex] = input[cindex]
            );
            var output = ciphertext.TrimEnd('X');
            return output;
        }

        public string Encrypt(string plaintext, string key)
        {
            var columnCount = key.Length;
            var rowCount = plaintext.Length / columnCount;
            if (plaintext.Length % columnCount != 0)
            {
                rowCount++;
            }

            var output = plaintext.PadRight(rowCount * columnCount, 'X');

            output = TransposeText(
                output,
                key,
                (input, pindex, buffer, cindex) => buffer[cindex] = input[pindex]
            );
            return output;
        }
    }
}
