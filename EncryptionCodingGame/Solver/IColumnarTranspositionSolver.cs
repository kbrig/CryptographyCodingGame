namespace EncryptionCodingGame.Solver
{
    public interface IColumnarTranspositionSolver
    {
        string Encrypt(string plaintext, string key, uint transpositionCount);
        string Decrypt(string ciphertext, string key, uint transpositionCount);
    }
}
