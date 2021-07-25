using System;
using System.Linq;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class ColumnarTranspositionEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IColumnarTranspositionSolver DEFAULT_SOLVER = new CoreColumnarTranspositionSolver();

        private string key;
        private IColumnarTranspositionSolver solver = DEFAULT_SOLVER;
        private uint transpositionCount;

        public ColumnarTranspositionEncryptionProblem(string key = "4312", uint transpositionCount = 1)
        {
            this.key = key;
            this.transpositionCount = transpositionCount;
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, key, transpositionCount);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, key, transpositionCount);
        }

        protected override void _ToolSetup()
        {
            Console.Write("Please enter your key as a string composed of only numbers: ");
            this.key = Console.ReadLine();

            Console.Write("How many consecutive transpositions with this key? ");
            this.transpositionCount = Convert.ToUInt32(Console.ReadLine());
        }

        public bool RunSolver(IColumnarTranspositionSolver solver)
        {
            if (solver == null)
            {
                throw new ArgumentNullException(nameof(solver));
            }

            var plaintext = "this long text is going to be a game change in the game".ToUpper();
            key = "7462315";
            transpositionCount = 6;

            var solverCipher = solver.Encrypt(plaintext, key, transpositionCount);
            var coreCipher = Encrypt(plaintext);

            var solverPlain = solver.Decrypt(coreCipher, key, transpositionCount);
            var newplain = Decrypt(coreCipher);

            var encryptResult = string.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = string.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");

            return encryptResult && decryptResult;
        }
    }
}
