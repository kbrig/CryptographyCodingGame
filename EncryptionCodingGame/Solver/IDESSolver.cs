using System;
namespace EncryptionCodingGame.Solver
{
    public interface IDESSolver
    {
        string Encrypt(string plaintext, int blocksize);
        string Decrypt(string ciphertext, int blocksize);
    }
}
