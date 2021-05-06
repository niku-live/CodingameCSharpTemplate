using System;
using System.Linq;

namespace Codingame.Contest
{
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
}
