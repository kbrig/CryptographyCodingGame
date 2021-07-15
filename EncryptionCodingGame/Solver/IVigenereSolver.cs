namespace EncryptionCodingGame.Solver
{
    public interface IVigenereSolver
    {
        string Encrypt(string plaintext, string key);
        string Decrypt(string ciphertext, string key);
    }
}
