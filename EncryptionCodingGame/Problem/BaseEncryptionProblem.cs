using System;
namespace EncryptionCodingGame.Problem
{
    public abstract class BaseEncryptionProblem
    {
        public string PlainText { get; set; }

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

        public void Tool()
        {
            LogHeader();

            var choice = Tools.ReadInputOrDefault("(E)ncryption or(D)ecryption?", "E");
            var text = Tools.ReadInputOrDefault("Please input your text:", "DEFAULT INPUT TEXT FOR TESTING");

            _ToolSetup();

            switch (choice.ToUpper())
            {
                case "E": Console.WriteLine($"Ciphertext: {Encrypt(text)}"); break;
                case "D": Console.WriteLine($"Plaintext: {Decrypt(text)}"); break;
                default: break;
            }

            LogFooter();
        }
    }
}
