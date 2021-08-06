using System;
namespace EncryptionCodingGame.Solver
{
    public interface ISolver { }

    public interface IKeySolver<TKey> : ISolver
    {
        string Encrypt(string plaintext, TKey key);
        string Decrypt(string ciphertext, TKey key);
    }
    public interface ICaesarSolver : IKeySolver<int> { }
    public interface IColumnarTranspositionSolver : IKeySolver<string> { }
    public interface IPlayfairSolver : IKeySolver<string> { }
    public interface IRailFenceSolver : IKeySolver<uint> { }
    public interface IVernamSolver : IKeySolver<string> { }
    public interface IVigenereSolver : IKeySolver<string> { }
    public interface IDESSolver : IKeySolver<string> { }

    public interface IBlockSolver<TKey> : ISolver
    {
        string Encrypt(string plaintext, TKey key, int blocksize);
        string Decrypt(string ciphertext, TKey key, int blocksize);
    }
    public interface IFeistelSolver : IBlockSolver<string> { }
    public interface IECBSolver : IBlockSolver<string> { }
    public interface ICBCSolver : IBlockSolver<string> { }
    public interface ICFBSolver : IBlockSolver<string> { }
    public interface IOFBSolver : IBlockSolver<string> { }
    public interface ICTRSolver : IBlockSolver<int> { }


}
