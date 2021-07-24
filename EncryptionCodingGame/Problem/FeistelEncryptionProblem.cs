using System;
using System.Text;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class FeistelEncryptionProblem : BaseEncryptionProblem
    {
        private const int BLOCK_SIZE = 4;
        private static readonly IFeistelSolver DEFAULT_SOLVER = new CoreFeistelSolver();

        private IFeistelSolver solver = DEFAULT_SOLVER;
        private int blocksize = BLOCK_SIZE;

        //TODO: REPLACE THIS
        protected override string ExpectedCipherText => "dHJ+dmN/cnp9Z3ZrZw==";

        protected override string ExpectedPlainText => "PLAINTEXT";

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, blocksize);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, blocksize);
        }

        protected override void _ToolSetup()
        {
            Console.Write("What block size should I use? ");
            this.blocksize = Convert.ToInt32(Console.ReadLine());
            base._ToolSetup();
        }

        public bool RunSolver(IFeistelSolver solver)
        {
            if (solver == null)
            {
                return false;
            }

            var plaintext = "GAMEPLAINTEXT".ToUpper();

            var coreCipher = Encrypt(plaintext);
            var solverCipher = solver.Encrypt(plaintext, blocksize);

            var newplain = Decrypt(coreCipher);
            var solverPlain = solver.Decrypt(coreCipher, blocksize);

            var encryptResult = string.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = string.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");

            return encryptResult && decryptResult;
        }
    }
}
