namespace Game.Grid
{
    public class CellData
    {
        public int X { get; }
        public int Y { get; }
        public bool HasMine { get; set; }
        public bool IsRevealed { get; private set; }
        public bool IsFlagged { get; private set; }
        public int AdjacentMines { get; set; }

        public CellData(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Reveal()
        {
            IsRevealed = true;
        }

        public void ToggleFlag()
        {
            if (!IsRevealed)
            {
                IsFlagged = !IsFlagged;
            }
        }
    }
}
