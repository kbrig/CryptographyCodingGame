﻿using System;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.Core;

namespace EncryptionCodingGame.Problem
{
    public class RailFenceEncryptionProblem : BaseEncryptionProblem
    {
        private static readonly IRailFenceSolver DEFAULT_SOLVER = new CoreRailFenceSolver();

        private uint depth;
        private IRailFenceSolver solver = DEFAULT_SOLVER;

        public RailFenceEncryptionProblem(uint depth = 3)
        {
            this.depth = depth;
        }

        public override string Decrypt(string ciphertext)
        {
            return solver.Decrypt(ciphertext, this.depth);
        }

        public override string Encrypt(string plaintext)
        {
            return solver.Encrypt(plaintext, this.depth);
        }

        protected override void _ToolSetup()
        {
            this.depth = Tools.ReadInputOrDefault("Rail depth?", depth);
        }

        public bool RunSolver(IRailFenceSolver solver)
        {
            if (solver == null)
            {
                throw new ArgumentNullException(nameof(solver));
            }
            LogHeader();
            var plaintext = "this long text is going to be a game change in the game".ToUpper();
            this.depth = 5;

            var solverCipher = solver.Encrypt(plaintext, this.depth);
            var coreCipher = Encrypt(plaintext);

            var solverPlain = solver.Decrypt(coreCipher, this.depth);
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
