using System;
using System.Text;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class FeistelEncryptionProblem : BaseBlockEncryptionProblem<IFeistelSolver, string>
    {
        private const int DEFAULT_BLOCKSIZE = 8;
        private static readonly IFeistelSolver DEFAULT_SOLVER = new CoreFeistelSolver();
        private const string DEFAULT_KEY = "MY SECRET KEY STAYS SECRET";

        private IFeistelSolver solver = DEFAULT_SOLVER;
        private int blocksize = DEFAULT_BLOCKSIZE;
        private string key = DEFAULT_KEY;

        public override int DefaultBlockSize => DEFAULT_BLOCKSIZE;
        protected override IFeistelSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, key, blocksize);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, key, blocksize);
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();
            this.key = Tools.ReadInputOrDefault("Key?", key);
            this.blocksize = Tools.ReadInputOrDefault("Block size?", blocksize);
        }

        protected override SolverResult _SolverRun(IFeistelSolver solver)
        {
            var plaintext = "GAMEPLAINTEXT".ToUpper();
            var key = DEFAULT_KEY;
            var blocksize = DefaultBlockSize;

            var cipher = solver.Encrypt(plaintext, key, blocksize);
            var plain = solver.Decrypt(cipher, key, blocksize);

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
