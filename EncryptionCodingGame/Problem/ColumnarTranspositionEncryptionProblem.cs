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
        private int columnCount;

        public ColumnarTranspositionEncryptionProblem(string key = "4312", uint transpositionCount = 1)
        {
            this.key = key;
            this.transpositionCount = transpositionCount;
        }

        protected override string ExpectedCipherText => "AEXIXXLTXPNT";

        protected override string ExpectedPlainText => "PLAINTEXT";

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, this.key, this.transpositionCount);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, this.key, this.transpositionCount);
        }

        protected override void _ToolSetup()
        {
            Console.Write("Please enter your key as a string composed of only numbers: ");
            this.key = Console.ReadLine();
            this.columnCount = key.Max() - '0';

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
            this.key = "7462315";
            this.transpositionCount = 6;

            var solverCipher = solver.Encrypt(plaintext, this.key, this.transpositionCount);
            var coreCipher = Encrypt(plaintext);

            var solverPlain = solver.Decrypt(coreCipher, this.key, this.transpositionCount);
            var newplain = Decrypt(coreCipher);

            var encryptResult = String.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = String.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");

            return encryptResult && decryptResult;
        }
    }
}
