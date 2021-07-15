using System;
namespace EncryptionCodingGame.Problem
{
    public abstract class BaseEncryptionProblem
    {
        public string PlainText { get; set; }
        protected abstract string ExpectedCipherText { get; }
        protected abstract string ExpectedPlainText { get; }

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

        public bool RunTest(string plaintext = "PLAINTEXT")
        {
            LogHeader();

            var ciphertext = Encrypt(plaintext);
            var newplain = Decrypt(ciphertext);
            var encryptResult = String.Compare(ExpectedCipherText, ciphertext) == 0;
            var decryptResult = String.Compare(newplain, plaintext) == 0;

            Console.WriteLine($"Cipher ({(encryptResult ? "S" : "F")}): {ciphertext}");
            Console.WriteLine($"Plain ({(decryptResult ? "S" : "F")}): {newplain}");

            LogFooter();
            return encryptResult && decryptResult;
        }

        protected virtual void _ToolSetup() { }


        public void Tool()
        {
            Console.Write("(E)ncryption or (D)ecryption? ");
            var choice = Console.ReadLine().ToUpper();

            Console.Write("Please input your text: ");
            var text = Console.ReadLine().Replace(" ", "").ToUpper();

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
