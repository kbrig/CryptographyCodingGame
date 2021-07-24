using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class DESEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IDESSolver DEFAULT_SOLVER = new CoreDESSolver();

        private IDESSolver solver = DEFAULT_SOLVER;

        public DESEncryptionProblem()
        {
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, 64);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, 64);
        }
    }
}
