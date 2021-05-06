namespace Codingame.Contest
{
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
}
