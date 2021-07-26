using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class CFBEncryptionProblem : BaseBlockEncryptionProblem<ICFBSolver, string>
    {
        private const int DEFAULT_BLOCK_SIZE = 3;
        private static readonly ICFBSolver DEFAULT_SOLVER = new CoreCFBSolver();
        private const string DEFAULT_KEY = "This key is cool and secure";

        private int blocksize = DEFAULT_BLOCK_SIZE;
        private ICFBSolver solver = DEFAULT_SOLVER;
        private string key = DEFAULT_KEY;

        protected override int DefaultBlockSize => DEFAULT_BLOCK_SIZE;
        protected override ICFBSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

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

        protected override SolverResult _SolverRun(ICFBSolver solver)
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
