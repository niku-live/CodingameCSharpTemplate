namespace Codingame.Contest
{
    public class GameAction
    {
        public const string WAIT = "WAIT";
        public const string SEED = "SEED";
        public const string GROW = "GROW";
        public const string COMPLETE = "COMPLETE";

        public static GameAction Parse(string action)
        {
            string[] parts = action.Split(" ");
            return parts[0] switch
            {
                WAIT => new GameAction(WAIT),
                SEED => new GameAction(SEED, int.Parse(parts[1]), int.Parse(parts[2])),
                _ => new GameAction(parts[0], int.Parse(parts[1])),
            };
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
            return Type switch
            {
                WAIT => WAIT,
                SEED => $"{SEED} {SourceCellIdx} {TargetCellIdx}",
                _ => $"{Type} {TargetCellIdx}",
            };
        }
    }
}
