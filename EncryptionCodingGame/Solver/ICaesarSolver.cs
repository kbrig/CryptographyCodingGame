namespace EncryptionCodingGame.Solver
{
    public interface ICaesarSolver
    {
        string Encrypt(string plaintext, int shift);
        string Decrypt(string ciphertext, int shift);
    }
}
