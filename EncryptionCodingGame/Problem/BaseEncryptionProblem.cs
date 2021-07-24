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
            Console.WriteLine($"=== Running {this.GetType().Name} ===");
        }

        protected virtual void LogFooter()
        {
            Console.WriteLine("".PadLeft(36, '='));
            Console.WriteLine();
        }

        protected virtual void _ToolSetup() { }

        public void Tool()
        {
            Console.Write("(E)ncryption or (D)ecryption? ");
            var choice = Console.ReadLine().ToUpper();

            Console.Write("Please input your text: ");
            var text = Console.ReadLine();

            _ToolSetup();

            switch (choice)
            {
                case "E": Console.WriteLine($"Ciphertext: {Encrypt(text)}"); break;
                case "D": Console.WriteLine($"Plaintext: {Decrypt(text)}"); break;
                default: break;
            }
        }
    }
}
