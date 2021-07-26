using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class CBCEncryptionProblem : BaseBlockEncryptionProblem<ICBCSolver, string>
    {
        private static readonly ICBCSolver DEFAULT_SOLVER = new CoreCBCSolver();
        private const int DEFAULT_BLOCK_SIZE = 64;
        private const string DEFAULT_KEY = "Here's an interesting key!";

        private ICBCSolver solver = DEFAULT_SOLVER;
        private int blocksize = DEFAULT_BLOCK_SIZE;
        private string key = DEFAULT_KEY;

        public override int DefaultBlockSize => DEFAULT_BLOCK_SIZE;
        protected override ICBCSolver DefaultSolver => DEFAULT_SOLVER;
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

        protected override SolverResult _SolverRun(ICBCSolver solver)
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
