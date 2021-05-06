namespace Codingame.Contest
{
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
}
