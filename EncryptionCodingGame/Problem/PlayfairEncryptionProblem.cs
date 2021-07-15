using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class PlayfairEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IPlayfairSolver DEFAULT_SOLVER = new CorePlayfairSolver();

        private string key;
        private IPlayfairSolver solver = DEFAULT_SOLVER;

        protected override string ExpectedCipherText => @"QPBSRQIUSZ";
        protected override string ExpectedPlainText => @"PLAINTEXT";

        public PlayfairEncryptionProblem(string key = "MONARCHY")
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

            Console.Write("What is your keyword (Remember: no dupplicate letters and no 'J's)? ");
            this.key = Console.ReadLine().ToUpper();
        }

        public bool RunSolver(IPlayfairSolver solver)
        {
            if (solver == null)
            {
                throw new ArgumentNullException(nameof(solver));
            }

            var plaintext = "GAMEPLAINTEXT".ToUpper();
            this.key = "BRIE";

            var solverCipher = solver.Encrypt(plaintext, key);
            var coreCipher = Encrypt(plaintext);

            var solverPlain = solver.Decrypt(coreCipher, key);
            var newplain = Decrypt(coreCipher);

            var encryptResult = String.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = String.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");

            return encryptResult && decryptResult;
        }
    }
}
