using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class RailFenceEncryptionProblem : BaseKeyEncryptionProblem<IRailFenceSolver, uint>
    {
        private static readonly IRailFenceSolver DEFAULT_SOLVER = new CoreRailFenceSolver();
        private const uint DEFAULT_DEPTH = 3;

        private uint depth;
        private IRailFenceSolver solver = DEFAULT_SOLVER;

        protected override IRailFenceSolver DefaultSolver => DEFAULT_SOLVER;
        protected override uint DefaultKey => DEFAULT_DEPTH;

        public RailFenceEncryptionProblem(uint depth = DEFAULT_DEPTH)
        {
            this.depth = depth;
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, this.depth);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, this.depth);
        }

        protected override void _ToolSetup()
        {
            this.depth = Tools.ReadInputOrDefault("Rail depth?", depth);
        }

        protected override SolverResult _SolverRun(IRailFenceSolver solver)
        {
            var plaintext = "this long text is going to be a game change in the game".ToUpper();
            var depth = 5u;

            var cipher = solver.Encrypt(plaintext, depth);
            var plain = solver.Decrypt(cipher, depth);

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
