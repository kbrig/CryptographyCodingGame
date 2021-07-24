using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{

    public class VigenereEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IVigenereSolver DEFAULT_SOLVER = new CoreVigenereSolver();

        private string key;
        private IVigenereSolver solver = DEFAULT_SOLVER;

        public VigenereEncryptionProblem(string key = "DECEPTIVE")
        {
            this.key = key;
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, this.key);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, this.key);
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();

            Console.Write("Keyword? ");
            this.key = Console.ReadLine().Trim().ToUpper();
        }


        public bool RunSolver(IVigenereSolver solver)
        {
            if (solver == null)
            {
                throw new ArgumentNullException(nameof(solver));
            }

            var plaintext = "THISTEXTISNOTCHEESYENOUGH".ToUpper();
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
