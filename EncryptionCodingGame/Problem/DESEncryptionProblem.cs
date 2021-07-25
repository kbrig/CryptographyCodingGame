using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class DESEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IDESSolver DEFAULT_SOLVER = new CoreDESSolver();

        private IDESSolver solver = DEFAULT_SOLVER;
        private string key = "AABB09182736CCDD";
        private int blocksize = 64;

        public DESEncryptionProblem()
        {
        }

        protected override void _ToolSetup()
        {
            base._ToolSetup();

            this.key = Tools.ReadInputOrDefault("Key?", key);
            this.blocksize = Tools.ReadInputOrDefault("Block size?", blocksize);
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, key, blocksize);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, key, blocksize);
        }

        public bool RunSolver(IDESSolver solver)
        {
            if (solver == null)
            {
                return false;
            }
            LogHeader();
            var plaintext = "GAMEPLAINTEXT".ToUpper();

            var coreCipher = Encrypt(plaintext);
            var solverCipher = solver.Encrypt(plaintext, this.key, blocksize);

            var newplain = Decrypt(coreCipher);
            var solverPlain = solver.Decrypt(coreCipher, this.key, blocksize);

            var encryptResult = string.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = string.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");
            LogFooter();
            return encryptResult && decryptResult;
        }
    }
}
