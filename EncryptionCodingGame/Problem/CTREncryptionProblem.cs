using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class CTREncryptionProblem : BaseBlockEncryptionProblem<ICTRSolver, int>
    {
        private static readonly ICTRSolver DEFAULT_SOLVER = new CoreCTRSolver();
        private const int DEFAULT_BLOCK_SIZE = 64;
        private const int DEFAULT_KEY = 0xa1b2c3d;

        private ICTRSolver solver = DEFAULT_SOLVER;
        private int blocksize = DEFAULT_BLOCK_SIZE;
        private int key = DEFAULT_KEY;

        protected override int DefaultBlockSize => DEFAULT_BLOCK_SIZE;
        protected override ICTRSolver DefaultSolver => DEFAULT_SOLVER;
        protected override int DefaultKey => DEFAULT_KEY;

        public override string Decrypt(string ciphertext)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plaintext)
        {
            throw new NotImplementedException();
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();
            this.key = Tools.ReadInputOrDefault("Key?", key);
            this.blocksize = Tools.ReadInputOrDefault("Block size?", blocksize);
        }

        protected override SolverResult _SolverRun(ICTRSolver solver)
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
