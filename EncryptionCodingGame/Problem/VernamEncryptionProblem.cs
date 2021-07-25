using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class VernamEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IVernamSolver DEFAULT_SOLVER = new CoreVernamSolver();

        private string key;
        private IVernamSolver solver = DEFAULT_SOLVER;

        public VernamEncryptionProblem(string key = "THISISKEY")
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

            this.key = Tools.ReadInputOrDefault("Key?", key);
        }

        public bool RunSolver(IVernamSolver solver)
        {
            if (solver == null)
            {
                throw new ArgumentNullException(nameof(solver));
            }
            LogHeader();
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
            LogFooter();
            return encryptResult && decryptResult;
        }
    }
}
