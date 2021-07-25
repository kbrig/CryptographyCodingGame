using System;
namespace EncryptionCodingGame.Solver
{
    public interface IDESSolver
    {
        string Encrypt(string plaintext, string key, int blocksize);
        string Decrypt(string ciphertext, string key, int blocksize);
    }
}
