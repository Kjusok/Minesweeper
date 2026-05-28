using System;

namespace Game.Grid
{
    public class NeighborHelper
    {
        private const int NeighborMin = -1;
        private const int NeighborMax = 1;

        private readonly int _width;
        private readonly int _height;

        public NeighborHelper(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void ForEachNeighbor(int x, int y, Action<int, int> action)
        {
            for (var dx = NeighborMin; dx <= NeighborMax; dx++)
            {
                for (var dy = NeighborMin; dy <= NeighborMax; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    var nx = x + dx;
                    var ny = y + dy;

                    if (IsInBounds(nx, ny))
                    {
                        action(nx, ny);
                    }
                }
            }
        }

        private int CountNeighbors(int x, int y, Func<int, int, bool> predicate)
        {
            var count = 0;
            ForEachNeighbor(x, y, (nx, ny) =>
            {
                if (predicate(nx, ny))
                {
                    count++;
                }
            });
            return count;
        }

        public void CalculateAdjacentMines(CellData[,] cells)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    if (cells[x, y].HasMine)
                    {
                        continue;
                    }

                    cells[x, y].AdjacentMines = CountNeighbors(x, y, (nx, ny) => cells[nx, ny].HasMine);
                }
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }
    }
}