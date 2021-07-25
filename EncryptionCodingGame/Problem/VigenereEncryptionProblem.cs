using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{

    public class VigenereEncryptionProblem : BaseKeyEncryptionProblem<IVigenereSolver, string>
    {
        private static readonly IVigenereSolver DEFAULT_SOLVER = new CoreVigenereSolver();
        private const string DEFAULT_KEY = "DECEPTIVE";

        private string key;
        private IVigenereSolver solver = DEFAULT_SOLVER;

        protected override IVigenereSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        public VigenereEncryptionProblem(string key = DEFAULT_KEY)
        {
            this.key = key;
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, this.key);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, this.key);
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();
            this.key = Tools.ReadInputOrDefault("Key?", key);
        }

        protected override SolverResult _SolverRun(IVigenereSolver solver)
        {
            var plaintext = "THISTEXTISNOTCHEESYENOUGH".ToUpper();
            this.key = "BRIE";

            var cipher = solver.Encrypt(plaintext, key);
            var plain = solver.Decrypt(cipher, key);

            var result = new SolverResult
            {
                Original = plaintext,
                Cipher = cipher,
                Plain = plain
            };
            return result;
        }
    }
}
