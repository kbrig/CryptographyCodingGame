using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class OFBEncryptionProblem : BaseBlockEncryptionProblem<IOFBSolver, string>
    {
        private const int DEFAULT_BLOCK_SIZE = 8;
        private static readonly IOFBSolver DEFAULT_SOLVER = new CoreOFBSolver();
        private const string DEFAULT_KEY = "THIS KEY IS SECURE, TRUST ME";

        private int blocksize = DEFAULT_BLOCK_SIZE;
        private IOFBSolver solver = DEFAULT_SOLVER;
        private string key = DEFAULT_KEY;

        protected override int DefaultBlockSize => DEFAULT_BLOCK_SIZE;
        protected override IOFBSolver DefaultSolver => DEFAULT_SOLVER;
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

        protected override SolverResult _SolverRun(IOFBSolver solver)
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
