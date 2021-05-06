namespace Codingame.Contest
{
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
}
