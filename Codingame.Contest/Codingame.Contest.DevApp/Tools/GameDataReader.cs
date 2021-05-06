namespace Codingame.Contest
{
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
}
