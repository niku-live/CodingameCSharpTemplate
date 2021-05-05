
using System;
using System.Linq;
using System.Reflection;

////////////////////////////////////////////////////////////////////////////////
//  Code from: Program.cs                                                     //
////////////////////////////////////////////////////////////////////////////////

    class Program
    {
        static void Main()
        {
            GameConsole.WriteLine("Hello World!");
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
        public static int[] ReadIntArr() => ReadLine().Split(' ').Select(int.Parse).ToArray();
        public static void Write(string value) => Console.Write(value);
        public static void WriteLine(string value) => Console.WriteLine(value);

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
//  Code from: .NETCoreApp,Version=v3.1.AssemblyAttributes.cs                 //
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
//  Code from: Codingame.Contest.DevApp.AssemblyInfo.cs                       //
////////////////////////////////////////////////////////////////////////////////
