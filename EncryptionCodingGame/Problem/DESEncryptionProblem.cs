using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class DESEncryptionProblem : BaseKeyEncryptionProblem<IDESSolver, string>
    {
        private static readonly IDESSolver DEFAULT_SOLVER = new CoreDESSolver();
        private const string DEFAULT_KEY = "AABB09182736CCDD";

        private IDESSolver solver = DEFAULT_SOLVER;
        private string key;

        protected override IDESSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        public DESEncryptionProblem(string key = DEFAULT_KEY)
        {
            this.key = key;
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();

            this.key = Tools.ReadInputOrDefault("Key?", key);
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, key);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, key);
        }

        protected override SolverResult _SolverRun(IDESSolver solver)
        {
            var plaintext = "Since this is DES as a block cipher let's have a more complicated plain text to encrypt/decrypt!";
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
