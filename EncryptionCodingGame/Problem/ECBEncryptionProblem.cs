using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class ECBEncryptionProblem : BaseBlockEncryptionProblem<IECBSolver, string>
    {
        private static readonly IECBSolver DEFAULT_SOLVER = new CoreECBSolver();
        private const int DEFAULT_BLOCK_SIZE = 64;
        private const string DEFAULT_KEY = "MikeyIsMyKey";

        public override int DefaultBlockSize => DEFAULT_BLOCK_SIZE;
        protected override IECBSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        private IECBSolver solver = DEFAULT_SOLVER;
        private string key = DEFAULT_KEY;
        private int blocksize = DEFAULT_BLOCK_SIZE;

        public override string Decrypt(string ciphertext)
        {
            return this.solver.Decrypt(ciphertext, this.key, this.blocksize);
        }

        public override string Encrypt(string plaintext)
        {
            return this.solver.Encrypt(plaintext, this.key, this.blocksize);
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();

            this.key = Tools.ReadInputOrDefault("Key?", key);
            this.blocksize = Tools.ReadInputOrDefault("Block size?", blocksize);
        }

        protected override SolverResult _SolverRun(IECBSolver solver)
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
