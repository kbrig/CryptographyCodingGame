namespace EncryptionCodingGame.Solver
{
    public interface IFeistelSolver
    {
        string Encrypt(string plaintext, int blocksize);
        string Decrypt(string ciphertext, int blocksize);
    }
}
