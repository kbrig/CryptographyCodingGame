using System;
using System.Linq;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class ColumnarTranspositionEncryptionProblem : BaseKeyEncryptionProblem<IColumnarTranspositionSolver, string>
    {
        private static readonly IColumnarTranspositionSolver DEFAULT_SOLVER = new CoreColumnarTranspositionSolver();
        private const string DEFAULT_KEY = "3142";

        private string key;
        private IColumnarTranspositionSolver solver = DEFAULT_SOLVER;
        private uint transpositionCount;

        protected override IColumnarTranspositionSolver DefaultSolver => DEFAULT_SOLVER;
        protected override string DefaultKey => DEFAULT_KEY;

        public ColumnarTranspositionEncryptionProblem(string key = DEFAULT_KEY, uint transpositionCount = 1)
        {
            this.key = key;
            this.transpositionCount = transpositionCount;
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
            this.key = Tools.ReadInputOrDefault("Please enter your key as a string composed of only numbers: ", key);
            this.transpositionCount = Tools.ReadInputOrDefault("How many consecutive transpositions with this key? ", transpositionCount);
        }

        protected override SolverResult _SolverRun(IColumnarTranspositionSolver solver)
        {
            var plaintext = "this long text is going to be a game change in the game".ToUpper();
            key = "7462315";
            transpositionCount = 6;

            var solverCipher = plaintext;
            var coreCipher = plaintext;

            for (int i = 0; i < transpositionCount; i++)
            {
                solverCipher = solver.Encrypt(solverCipher, key);
                coreCipher = Encrypt(coreCipher);
            }

            var solverPlain = coreCipher;
            var newplain = coreCipher;
            for (int i = 0; i < transpositionCount; i++)
            {
                solverPlain = solver.Decrypt(solverPlain, key);
                newplain = Decrypt(newplain);
            }

            var result = new SolverResult
            {
                Original = plaintext,
                Cipher = coreCipher,
                Plain = newplain
            };
            return result;
        }
    }
}
