using System;
using System.Collections.Generic;

namespace Game.Grid
{
    public class MineGenerator
    {
        public void GenerateMines(CellData[,] cells, int safeX, int safeY, int mineCount)
        {
            var width = cells.GetLength(0);
            var height = cells.GetLength(1);
            var available = new List<(int x, int y)>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (Math.Abs(x - safeX) <= 1 && Math.Abs(y - safeY) <= 1)
                    {
                        continue;
                    }
                    available.Add((x, y));
                }
            }

            var random = new Random();
            for (var i = available.Count - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);
                (available[i], available[j]) = (available[j], available[i]);
            }

            var count = Math.Min(mineCount, available.Count);
            for (var i = 0; i < count; i++)
            {
                var (mx, my) = available[i];
                cells[mx, my].HasMine = true;
            }
        }
    }
}
