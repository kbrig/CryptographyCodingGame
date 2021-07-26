using System;
using EncryptionCodingGame.Solver;

namespace EncryptionCodingGame.Problem
{
    public interface IEncryptionProblem
    {
        public string Encrypt(string plaintext);
        public string Decrypt(string ciphertext);
        public void Tool();
        public void RunSolver(ISolver solver);
        public void RunGame(ISolver solver);
        public void RunCoreSolver();
    }

    public abstract class BaseEncryptionProblem<TSolver, TKey> : IEncryptionProblem
        where TSolver : class, ISolver
    {
        protected abstract TSolver DefaultSolver { get; }
        protected abstract TKey DefaultKey { get; }

        public abstract string Encrypt(string plaintext);
        public abstract string Decrypt(string ciphertext);

        protected virtual void LogHeader()
        {
            string line = Tools.MultiplyChar('=', 50);
            Console.WriteLine(line);
            Console.WriteLine($"= [ {this.GetType().Name,-42} ] =");
            Console.WriteLine(line);
        }

        protected virtual void LogFooter()
        {
            Console.WriteLine(Tools.MultiplyChar('=', 50));
            Console.WriteLine();
        }

        protected virtual void _ToolSetup() { }

        protected void RunLogged(Action codeBlock)
        {
            LogHeader();
            codeBlock();
            LogFooter();
        }

        protected void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        protected abstract SolverResult _SolverRun(TSolver solver);

        public void Tool()
        {
            RunLogged(() =>
            {
                var choice = Tools.ReadInputOrDefault("(E)ncryption or(D)ecryption?", "E");
                var text = Tools.ReadInputOrDefault("Please input your text:", "DEFAULT INPUT TEXT FOR TESTING");

                _ToolSetup();

                switch (choice.ToUpper())
                {
                    case "E": Log($"Ciphertext: {Encrypt(text)}"); break;
                    case "D": Log($"Plaintext: {Decrypt(text)}"); break;
                    default: break;
                }
            });
        }

        public virtual void RunSolver(ISolver solver)
        {
            var mySolver = solver as TSolver;
            if (mySolver == null)
            {
                throw new TypeInitializationException(typeof(TSolver).FullName, null);
            }

            var result = _SolverRun(mySolver);

            Console.WriteLine($"E({result.Original}) = {result.Cipher}");
            Console.WriteLine($"D({result.Cipher}) = {result.Plain}");
        }

        public virtual void RunGame(ISolver solver)
        {
            var mySolver = solver as TSolver;
            if (mySolver == null)
            {
                throw new TypeInitializationException(typeof(TSolver).FullName, null);
            }

            RunLogged(() =>
            {
                var solverResult = _SolverRun(mySolver);
                var coreResult = _SolverRun(DefaultSolver);

                var encryptResult = string.Compare(coreResult.Cipher, solverResult.Cipher) == 0;
                var decryptResult = string.Compare(solverResult.Original, solverResult.Plain) == 0;

                Console.WriteLine($"E ({(encryptResult ? "S" : "F")}): EXP: {coreResult.Cipher} ; RESULT: {solverResult.Cipher}");
                Console.WriteLine($"D ({(decryptResult ? "S" : "F")}): EXP: {solverResult.Original} ; RESULT: {solverResult.Plain}");
            });
        }

        public abstract void RunCoreSolver();
    }

    public abstract class BaseKeyEncryptionProblem<TSolver, TKey> : BaseEncryptionProblem<TSolver, TKey>
        where TSolver : class, IKeySolver<TKey>
    {
        public override void RunCoreSolver()
        {
            RunLogged(() =>
            {
                var plaintext = "TEST INPUT FOR TESTS IS GOOD";

                var ciphertext = this.DefaultSolver.Encrypt(plaintext, this.DefaultKey);
                Log($"E({plaintext}) = {ciphertext}");

                var newplain = this.DefaultSolver.Decrypt(ciphertext, this.DefaultKey);
                Log($"D({ciphertext}) = {newplain}");
            });
        }
    }

    public abstract class BaseBlockEncryptionProblem<TSolver, TKey> : BaseEncryptionProblem<TSolver, TKey>
        where TSolver : class, IBlockSolver<TKey>
    {
        public abstract int DefaultBlockSize { get; }

        public override void RunCoreSolver()
        {
            RunLogged(() =>
            {
                var plaintext = "NOW THIS IS SOME LONG-ASS TEXT TO MAKE SURE IT WORKS PERFECTLY FINE!";

                var ciphertext = this.DefaultSolver.Encrypt(plaintext, this.DefaultKey, this.DefaultBlockSize);
                Log($"E({plaintext}) = {ciphertext}");

                var newplain = this.DefaultSolver.Decrypt(ciphertext, this.DefaultKey, this.DefaultBlockSize);
                Log($"D({ciphertext}) = {newplain}");
            });
        }
    }
}
