using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class VernamEncryptionProblem : BaseKeyEncryptionProblem<IVernamSolver, string>
    {
        private static readonly IVernamSolver DEFAULT_SOLVER = new CoreVernamSolver();
        private const string DEFAULT_KEY = "THISISKEY";

        private IVernamSolver solver = DEFAULT_SOLVER;
        private string key;

        protected override IVernamSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        public VernamEncryptionProblem(string key = DEFAULT_KEY)
        {
            this.key = key;
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, key);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, key);
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();
            this.key = Tools.ReadInputOrDefault("Key?", key);
        }

        protected override SolverResult _SolverRun(IVernamSolver solver)
        {
            var plaintext = "GAMEPLAINTEXT".ToUpper();
            key = "BRIE";

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
