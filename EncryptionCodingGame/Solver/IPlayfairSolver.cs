namespace EncryptionCodingGame.Solver
{
    public interface IPlayfairSolver
    {
        string Encrypt(string plaintext, string key);
        string Decrypt(string ciphertext, string key);
    }
}
