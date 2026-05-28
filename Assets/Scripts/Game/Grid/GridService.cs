using System;
using System.Collections.Generic;
using Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Game.Grid
{
    public class GridService
    {
        private const float HalfDivider = 2f;

        private readonly GridConfig _config;
        private readonly MineGenerator _mineGenerator;
        private readonly CellView.Factory _cellViewFactory;

        private CellData[,] _cells;
        private CellView[,] _cellViews;
        private NeighborHelper _neighborHelper;
        private int _revealedCount;
        private bool _minesGenerated;
        private bool _isInputEnabled;

        public event Action<CellData> OnCellRevealed;
        public event Action OnMineHit;
        public event Action OnWin;

        public GridService(GridConfig config, MineGenerator mineGenerator, CellView.Factory cellViewFactory)
        {
            _config = config;
            _mineGenerator = mineGenerator;
            _cellViewFactory = cellViewFactory;
        }

        public void SetInputEnabled(bool enabled)
        {
            _isInputEnabled = enabled;
        }

        public void CreateGrid(Transform parent)
        {
            _cells = new CellData[_config.Width, _config.Height];
            _cellViews = new CellView[_config.Width, _config.Height];
            _neighborHelper = new NeighborHelper(_config.Width, _config.Height);
            _revealedCount = 0;
            _minesGenerated = false;

            var offset = new Vector2(
                -(_config.Width - 1) * (_config.CellSize + _config.CellSpacing) / HalfDivider,
                -(_config.Height - 1) * (_config.CellSize + _config.CellSpacing) / HalfDivider
            );

            var cellStep = _config.CellSize + _config.CellSpacing;

            for (var x = 0; x < _config.Width; x++)
            {
                for (var y = 0; y < _config.Height; y++)
                {
                    _cells[x, y] = new CellData(x, y);

                    var view = _cellViewFactory.Create();
                    view.transform.SetParent(parent, false);

                    var rectTransform = view.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(_config.CellSize, _config.CellSize);
                    rectTransform.anchoredPosition = new Vector2(
                        offset.x + x * cellStep,
                        offset.y + y * cellStep
                    );

                    view.Initialize(_cells[x, y]);
                    view.OnClicked += HandleCellClicked;
                    _cellViews[x, y] = view;
                }
            }
        }

        public void Cleanup()
        {
            if (_cellViews == null)
            {
                return;
            }

            foreach (var view in _cellViews)
            {
                if (view)
                {
                    Object.Destroy(view.gameObject);
                }
            }

            _cellViews = null;
            _cells = null;
            _neighborHelper = null;
        }

        private void HandleCellClicked(CellView cellView, PointerEventData.InputButton button)
        {
            if (!_isInputEnabled)
            {
                return;
            }

            var data = cellView.Data;
            switch (button)
            {
                case PointerEventData.InputButton.Right:
                    ToggleFlag(data.X, data.Y);
                    break;
                case PointerEventData.InputButton.Left:
                    RevealCell(data.X, data.Y);
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        private void RevealCell(int x, int y)
        {
            var cell = _cells[x, y];

            if (cell.IsRevealed || cell.IsFlagged)
            {
                return;
            }

            if (!_minesGenerated)
            {
                _mineGenerator.GenerateMines(_cells, x, y, _config.MineCount);
                _neighborHelper.CalculateAdjacentMines(_cells);
                _minesGenerated = true;
            }

            cell.Reveal();
            _revealedCount++;
            _cellViews[x, y].UpdateView();
            OnCellRevealed?.Invoke(cell);

            if (cell.HasMine)
            {
                RevealAllMines();
                OnMineHit?.Invoke();
                return;
            }

            if (cell.AdjacentMines == 0)
            {
                FloodReveal(x, y);
            }

            CheckWinCondition();
        }

        private void ToggleFlag(int x, int y)
        {
            var cell = _cells[x, y];
            cell.ToggleFlag();
            _cellViews[x, y].UpdateView();
        }

        private void FloodReveal(int startX, int startY)
        {
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue((startX, startY));

            while (queue.Count > 0)
            {
                var (cx, cy) = queue.Dequeue();

                _neighborHelper.ForEachNeighbor(cx, cy, (nx, ny) =>
                {
                    var neighbor = _cells[nx, ny];

                    if (neighbor.IsRevealed || neighbor.IsFlagged || neighbor.HasMine)
                    {
                        return;
                    }

                    neighbor.Reveal();
                    _revealedCount++;
                    _cellViews[nx, ny].UpdateView();
                    OnCellRevealed?.Invoke(neighbor);

                    if (neighbor.AdjacentMines == 0)
                    {
                        queue.Enqueue((nx, ny));
                    }
                });
            }
        }

        private void RevealAllMines()
        {
            foreach (var cell in _cells)
            {
                if (cell.HasMine)
                {
                    cell.Reveal();
                    _cellViews[cell.X, cell.Y].UpdateView();
                }
            }
        }

        private void CheckWinCondition()
        {
            var targetCount = _config.Width * _config.Height - _config.MineCount;

            if (_revealedCount == targetCount)
            {
                OnWin?.Invoke();
            }
        }
    }
}
