﻿using System;
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
            base._ToolSetup();
            this.blocksize = Tools.ReadInputOrDefault("Block size?", blocksize);
        }

        public bool RunSolver(IFeistelSolver solver)
        {
            if (solver == null)
            {
                return false;
            }
            LogHeader();
            var plaintext = "GAMEPLAINTEXT".ToUpper();

            var coreCipher = Encrypt(plaintext);
            var solverCipher = solver.Encrypt(plaintext, blocksize);

            var newplain = Decrypt(coreCipher);
            var solverPlain = solver.Decrypt(coreCipher, blocksize);

            var encryptResult = string.Compare(coreCipher, solverCipher) == 0;
            var decryptResult = string.Compare(newplain, solverPlain) == 0;

            Console.WriteLine($"ENC ({(encryptResult ? "S" : "F")}): EXP: {coreCipher} ; RESULT: {solverCipher}");
            Console.WriteLine($"DEC ({(decryptResult ? "S" : "F")}): EXP: {newplain} ; RESULT: {solverPlain}");
            LogFooter();
            return encryptResult && decryptResult;
        }
    }
}
