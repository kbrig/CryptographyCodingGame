namespace EncryptionCodingGame.Solver
{
    public interface IRailFenceSolver
    {
        string Encrypt(string plaintext, uint depth);
        string Decrypt(string ciphertext, uint depth);
    }
}
