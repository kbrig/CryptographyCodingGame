﻿using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class CaesarEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly ICaesarSolver DEFAULT_SOLVER = new CoreCaesarSolver();

        private int shift;
        private ICaesarSolver solver = DEFAULT_SOLVER;

        protected override string ExpectedCipherText => @"SODLQWHAW";
        protected override string ExpectedPlainText => @"PLAINTEXT";

        public CaesarEncryptionProblem(int shift = 3)
        {
            this.shift = shift;
        }

        public override string Encrypt(string plainText)
        {
            return solver.Encrypt(plainText, shift);
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, shift);
        }

        protected override void _ToolSetup()
        {
            Console.Write("Shift? ");
            this.shift = Convert.ToInt32(Console.ReadLine().Trim());
        }

        public bool RunSolver(ICaesarSolver solver)
        {
            if (solver == null)
            {
                return false;
            }

            var plaintext = "GAMEPLAINTEXT".ToUpper();
            this.shift = 5;

            var solverCipher = solver.Encrypt(plaintext, 5);
            var coreCipher = Encrypt(plaintext);

            var solverPlain = solver.Decrypt(coreCipher, 5);
            var newplain = Decrypt(coreCipher);

            var encryptResult = String.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = String.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");

            return encryptResult && decryptResult;
        }
    }
}
