using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class PlayfairEncryptionProblem : BaseKeyEncryptionProblem<IPlayfairSolver, string>
    {
        private static readonly IPlayfairSolver DEFAULT_SOLVER = new CorePlayfairSolver();
        private const string DEFAULT_KEY = "MONARCHY";

        private string key;
        private IPlayfairSolver solver = DEFAULT_SOLVER;

        protected override IPlayfairSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        public PlayfairEncryptionProblem(string key = DEFAULT_KEY)
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

            this.key = Tools.ReadInputOrDefault("What is your keyword (Remember: no dupplicate letters and no 'J's)? ", key);
            var filler = Tools.ReadInputOrDefault("Filler character?", 'X');

            this.solver = new CorePlayfairSolver(filler);
        }

        protected override SolverResult _SolverRun(IPlayfairSolver solver)
        {
            var plaintext = "GAMEPLAINTEXT".ToUpper();
            this.key = "BRIE";

            var cipher = solver.Encrypt(plaintext, key);
            var newplain = solver.Decrypt(cipher, key);

            var result = new SolverResult
            {
                Original = plaintext,
                Cipher = cipher,
                Plain = newplain
            };
            return result;
        }
    }
}
