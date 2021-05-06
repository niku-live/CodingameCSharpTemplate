using System.Collections.Generic;
using System.Linq;

namespace Codingame.Contest
{
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
}
