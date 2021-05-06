
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

////////////////////////////////////////////////////////////////////////////////
//  Code from: Cell.cs                                                        //
////////////////////////////////////////////////////////////////////////////////

    public class Cell
    {
        public int Index { get; set; }
        public int Richess { get; set; }
        public int[] Neighbours { get; set; }        

        public Cell(int index, int richess, params int[] neighbours)
        {
            Index = index;
            Richess = richess;
            Neighbours = neighbours;
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: Game.cs                                                        //
////////////////////////////////////////////////////////////////////////////////

    public class Game
    {
        public int Day { get; set; }
        public int Nutrients { get; set; }
        public List<Cell> Board { get; set; }
        public List<GameAction> PossibleActions { get; set; }
        public List<Tree> Trees { get; set; }
        public int MySun { get; set; }
        public int OpponentSun { get; set; }
        public int MyScore { get; set; }
        public int OpponentScore { get; set; }
        public bool OpponentIsWaiting { get; set; }

        public Game()
        {
            Board = new List<Cell>();
            PossibleActions = new List<GameAction>();
            Trees = new List<Tree>();
        }

        public GameAction GetNextAction()
        {
            // TODO: write your algorithm here
            return PossibleActions.First();
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: GameAction.cs                                                  //
////////////////////////////////////////////////////////////////////////////////

    public class GameAction
    {
        public const string WAIT = "WAIT";
        public const string SEED = "SEED";
        public const string GROW = "GROW";
        public const string COMPLETE = "COMPLETE";

        public static GameAction Parse(string action)
        {
            string[] parts = action.Split(" ");
            switch (parts[0])
            {
                case WAIT:
                    return new GameAction(WAIT);
                case SEED:
                    return new GameAction(SEED, int.Parse(parts[1]), int.Parse(parts[2]));
                case GROW:
                case COMPLETE:
                default:
                    return new GameAction(parts[0], int.Parse(parts[1]));
            }
        }

        public string Type { get; set; }
        public int TargetCellIdx { get; set; }
        public int SourceCellIdx { get; set; }

        public GameAction(string type, int sourceCellIdx, int targetCellIdx)
        {
            Type = type;
            TargetCellIdx = targetCellIdx;
            SourceCellIdx = sourceCellIdx;
        }

        public GameAction(string type, int targetCellIdx): this(type, 0, targetCellIdx) { }

        public GameAction(string type): this(type, 0, 0) { }

        public override string ToString()
        {
            if (Type == WAIT)
            {
                return GameAction.WAIT;
            }
            if (Type == SEED)
            {
                return $"{SEED} {SourceCellIdx} {TargetCellIdx}";
            }
            return $"{Type} {TargetCellIdx}";
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: Program.cs                                                     //
////////////////////////////////////////////////////////////////////////////////

    public class Program
    {
        public static void Main()
        {
            Game game = new Game();
            GameDataReader.ReadGameBoard(game);
            // game loop
            while (true)
            {
                GameDataReader.ReadTurnInfo(game);
                var action = game.GetNextAction();
                GameConsole.WriteLine(action);
            }
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: Tree.cs                                                        //
////////////////////////////////////////////////////////////////////////////////

    public class Tree
    {
        public int CellIndex { get; set; }
        public int Size { get; set; }
        public bool IsMine { get; set; }
        public bool IsDormant { get; set; }

        public Tree(int cellIndex, int size, bool isMine, bool isDormant)
        {
            CellIndex = cellIndex;
            Size = size;
            IsMine = isMine;
            IsDormant = isDormant;
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: GameConsole.cs                                                 //
////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// System.Console wrapper with helper functions useful for Codingame
    /// </summary>
    public static class GameConsole
    {
        /// <summary>
        /// Debug level is used to select which debug messages should be printed
        /// Input - only input is printed (useful to get input from Codingame console)
        /// Simple - only SimpleDebugMessage output is printed
        /// Verbose - all output is printed
        /// </summary>
        public static GameConsoleDebugLevel DebugLevel = GameConsoleDebugLevel.None;

        public static string ReadLine()
        {
            var line = Console.ReadLine();
            if (DebugLevel == GameConsoleDebugLevel.Input)
            {
                DebugMessage(line);
            }
            return line;
        }

        public static int ReadInt() => int.Parse(ReadLine());
        public static string[] ReadStringArr() => ReadLine().Split(' ');
        public static int[] ReadIntArr() => ReadStringArr().Select(int.Parse).ToArray();
        public static void Write(string value) => Console.Write(value);
        public static void WriteLine(string value) => Console.WriteLine(value);
        public static void WriteLine(GameAction value, string debugMessage = null) => Console.WriteLine(debugMessage == null ? $"{value}": $"{value} {debugMessage}");

        /// <summary>
        /// Debug message could be implemented with compiler directives #if DEBUG, but Codingame does not support this
        /// </summary>
        /// <param name="message"></param>
        public static void SimpleDebugMessage(string message)
        {
            if (DebugLevel >= GameConsoleDebugLevel.Simple)
            {
                DebugMessage(message);
            }
        }

        public static void VerboseDebugMessage(string message)
        {
            if (DebugLevel >= GameConsoleDebugLevel.Verbose)
            {
                DebugMessage(message);
            }
        }

        private static void DebugMessage(string message)
        {
            Console.Error.WriteLine(message);
        }

        public enum GameConsoleDebugLevel
        {
            None,
            Input,
            Simple,
            Verbose
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: GameDataReader.cs                                              //
////////////////////////////////////////////////////////////////////////////////

    public static class GameDataReader
    {
        public static void ReadGameBoard(Game game)
        {
            game.Board.Clear();
            int numberOfCells = GameConsole.ReadInt(); // 37            
            for (int i = 0; i < numberOfCells; i++)
            {
                var inputs = GameConsole.ReadIntArr();
                game.Board.Add(new Cell(index: inputs[0], richess: inputs[1], inputs[2..]));
            }
        }

        public static void ReadTurnInfo(Game game)
        {
            ReadPlayersInfo(game);
            ReadTreesInfo(game);
            ReadActionsInfo(game);
        }

        private static void ReadPlayersInfo(Game game)
        {
            game.Day = GameConsole.ReadInt(); // the game lasts 24 days: 0-23
            game.Nutrients = GameConsole.ReadInt(); // the base score you gain from the next COMPLETE action
            var myInfo = GameConsole.ReadIntArr();
            game.MySun = myInfo[0]; // your sun points
            game.MyScore = myInfo[1]; // your current score
            var oppInfo = GameConsole.ReadIntArr();
            game.OpponentSun = oppInfo[0]; // opponent's sun points
            game.OpponentScore = oppInfo[1]; // opponent's score
            game.OpponentIsWaiting = oppInfo[2] != 0; // whether your opponent is asleep until the next day
        }

        private static void ReadActionsInfo(Game game)
        {
            game.PossibleActions.Clear();
            int numberOfPossibleMoves = GameConsole.ReadInt();
            for (int i = 0; i < numberOfPossibleMoves; i++)
            {
                var possibleMove = GameConsole.ReadLine();
                game.PossibleActions.Add(GameAction.Parse(possibleMove));
            }
        }

        private static void ReadTreesInfo(Game game)
        {
            game.Trees.Clear();
            int numberOfTrees = GameConsole.ReadInt(); // the current amount of trees
            for (int i = 0; i < numberOfTrees; i++)
            {
                var inputs = GameConsole.ReadStringArr();
                int cellIndex = int.Parse(inputs[0]); // location of this tree
                int size = int.Parse(inputs[1]); // size of this tree: 0-3
                bool isMine = inputs[2] != "0"; // 1 if this is your tree
                bool isDormant = inputs[3] != "0"; // 1 if this tree is dormant
                Tree tree = new Tree(cellIndex, size, isMine, isDormant);
                game.Trees.Add(tree);
            }
        }
    }

////////////////////////////////////////////////////////////////////////////////
//  Code from: .NETCoreApp,Version=v3.1.AssemblyAttributes.cs                 //
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
//  Code from: Codingame.Contest.DevApp.AssemblyInfo.cs                       //
////////////////////////////////////////////////////////////////////////////////
