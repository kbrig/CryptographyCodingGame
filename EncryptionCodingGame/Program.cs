using System;
using System.Collections.Generic;
using EncryptionCodingGame.Problem;
using EncryptionCodingGame.Solver;
using EncryptionCodingGame.Solver.PlayerImplementation;

namespace EncryptionCodingGame
{
    class Program
    {
        static IEncryptionProblem[] problems = new []
        {
            new CaesarEncryptionProblem() as IEncryptionProblem,
            new PlayfairEncryptionProblem(),
            new VigenereEncryptionProblem(),
            new VernamEncryptionProblem(),
            new RailFenceEncryptionProblem(),
            new ColumnarTranspositionEncryptionProblem(),
            new FeistelEncryptionProblem(),
            new DESEncryptionProblem(),
            new ECBEncryptionProblem(),
            new CBCEncryptionProblem(),
            new CFBEncryptionProblem(),
            new OFBEncryptionProblem(),
            new CTREncryptionProblem()
        };

        static Dictionary<Type, Func<ISolver>> SolverFactory = new Dictionary<Type, Func<ISolver>>
        {
            { typeof(CaesarEncryptionProblem),      () => new PICaesarSolver() },
            { typeof(PlayfairEncryptionProblem),    () => new PIPlayfairSolver() },
            { typeof(VigenereEncryptionProblem),    () => new PIVigenereSolver() },
            { typeof(VernamEncryptionProblem),      () => new PIVernamSolver() },
            { typeof(RailFenceEncryptionProblem),   () => new PIRailFenceSolver() },
            { typeof(ColumnarTranspositionEncryptionProblem), () => new PIColumnarTranspositionSolver() },
            { typeof(FeistelEncryptionProblem),     () => new PIFeistelSolver() },
            { typeof(DESEncryptionProblem),         () => new PIDESSolver() },
            { typeof(ECBEncryptionProblem),         () => new PIECBSolver() },
            { typeof(CBCEncryptionProblem),         () => new PICBCSolver() },
            { typeof(CFBEncryptionProblem),         () => new PICFBSolver() },
            { typeof(OFBEncryptionProblem),         () => new PIOFBSolver() },
            { typeof(CTREncryptionProblem),         () => new PICTRSolver() }
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, let's start your code!");

            try
            {
                TestMode();
                //GameMode();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Not completed the game yet? Continue coding and good luck!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error of type {ex.GetType().Name}: {ex.Message}");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            ToolMode();
        }

        private static void TestMode()
        {
            foreach (var problem in problems)
            {
                problem.RunCoreSolver();
            }
        }

        private static void GameMode()
        {
            try
            {
                foreach (var problem in problems)
                {
                    var solverCreator = SolverFactory[problem.GetType()];
                    var solver = solverCreator();
                    problem.RunSolver(solver);
                }
                Console.WriteLine("Game won! Now go to bed!");
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
                    Console.WriteLine($"{i + 1}\t: {problems[i].GetType().Name.Split("EncryptionProblem")[0]}");
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
