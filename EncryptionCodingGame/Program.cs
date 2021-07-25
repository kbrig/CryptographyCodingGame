using System;
using EncryptionCodingGame.Problem;
using EncryptionCodingGame.Solver.Core;
using EncryptionCodingGame.Solver.PlayerImplementation;

namespace EncryptionCodingGame
{
    class Program
    {
        static BaseEncryptionProblem[] problems = new []
        {
            new CaesarEncryptionProblem() as BaseEncryptionProblem,
            new PlayfairEncryptionProblem(),
            new VigenereEncryptionProblem(),
            new VernamEncryptionProblem(),
            new RailFenceEncryptionProblem(),
            new ColumnarTranspositionEncryptionProblem(),
            new FeistelEncryptionProblem(),
            new DESEncryptionProblem()
        };

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, let's start your code!");

            try
            {
                TestMode();
                GameMode();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not completed the game yet? Continue coding and good luck!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error of type {ex.GetType().Name}: {ex.Message}");
            }
            ToolMode();
        }

        private static void TestMode()
        {
            (problems[0] as CaesarEncryptionProblem).RunSolver(new CoreCaesarSolver());
            (problems[1] as PlayfairEncryptionProblem).RunSolver(new CorePlayfairSolver());
            (problems[2] as VigenereEncryptionProblem).RunSolver(new CoreVigenereSolver());
            (problems[3] as VernamEncryptionProblem).RunSolver(new CoreVernamSolver());
            (problems[4] as RailFenceEncryptionProblem).RunSolver(new CoreRailFenceSolver());
            (problems[5] as ColumnarTranspositionEncryptionProblem).RunSolver(new CoreColumnarTranspositionSolver());
            (problems[6] as FeistelEncryptionProblem).RunSolver(new CoreFeistelSolver());
            (problems[7] as DESEncryptionProblem).RunSolver(new CoreDESSolver());
        }

        private static void GameMode()
        {
            try
            {
                if ((problems[0] as CaesarEncryptionProblem).RunSolver(new PICaesarSolver()) &&
                    (problems[1] as PlayfairEncryptionProblem).RunSolver(new PIPlayfairSolver()) &&
                    (problems[2] as VigenereEncryptionProblem).RunSolver(new PIVigenereSolver()) &&
                    (problems[3] as VernamEncryptionProblem).RunSolver(new PIVernamSolver()) &&
                    (problems[4] as RailFenceEncryptionProblem).RunSolver(new PIRailFenceSolver()) &&
                    (problems[5] as ColumnarTranspositionEncryptionProblem).RunSolver(new PIColumnarTranspositionSolver()) &&
                    (problems[6] as FeistelEncryptionProblem).RunSolver(new PIFeistelSolver()))
                {
                    Console.WriteLine("Game won! Now go to bed!");
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not done yet? Implement all methods!");
            }
        }

        private static void ToolMode()
        {
            do
            {
                Console.WriteLine("=== TOOL MODE ===");
                Console.WriteLine("0\t: Exit Tool Mode");
                for (int i = 0; i < problems.Length; i++)
                {
                    Console.WriteLine($"{i + 1}\t: {problems[i].GetType().Name}");
                }
                Console.WriteLine();

                Console.Write("Please select what cipher you would like to use as a tool: ");
                var userInput = Convert.ToUInt32(Console.ReadLine().Trim());
                if (userInput == 0)
                {
                    break;
                }

                var solver = problems[userInput - 1];
                solver.Tool();
            } while (true);
        }
    }
}
