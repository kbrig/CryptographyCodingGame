namespace EncryptionCodingGame.Solver
{
    public interface IVernamSolver
    {
        string Encrypt(string plaintext, string key);
        string Decrypt(string ciphertext, string key);
    }
}
