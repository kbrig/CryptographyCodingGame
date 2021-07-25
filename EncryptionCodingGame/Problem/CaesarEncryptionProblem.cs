using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class CaesarEncryptionProblem : BaseKeyEncryptionProblem<ICaesarSolver, int>
    {
        private static readonly ICaesarSolver DEFAULT_SOLVER = new CoreCaesarSolver();
        private const int DEFAULT_KEY = 3;

        private int shift;
        private ICaesarSolver solver = DEFAULT_SOLVER;

        protected override ICaesarSolver DefaultSolver => DEFAULT_SOLVER;
        protected override int DefaultKey => DEFAULT_KEY;

        public CaesarEncryptionProblem(int shift = DEFAULT_KEY)
        {
            this.shift = shift;
        }

        public override string Encrypt(string plainText)
        {
            return solver.Encrypt(plainText, shift);
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, shift);
        }

        protected override void _ToolSetup()
        {
            this.shift = Tools.ReadInputOrDefault("Shift?", shift);
        }

        protected override SolverResult _SolverRun(ICaesarSolver solver)
        {
            var plaintext = "GAMEPLAINTEXT".ToUpper();
            this.shift = 5;

            var cipher = solver.Encrypt(plaintext, 5);
            var newplain = solver.Decrypt(cipher, 5);

            var result = new SolverResult
            {
                Original = plaintext,
                Cipher = cipher,
                Plain = newplain
            };
            return result;
        }
    }
}
