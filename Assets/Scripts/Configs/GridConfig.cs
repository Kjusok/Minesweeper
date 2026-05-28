using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Minesweeper/GridConfig", fileName = "GridConfig")]
    public class GridConfig : ScriptableObject
    {
        [field: Header("Grid Settings")]
        [field: SerializeField] public int Width { get; private set; } = 10;
        [field: SerializeField] public int Height { get; private set; } = 10;
        [field: SerializeField] public int MineCount { get; private set; } = 15;

        [field: Header("Visual")]
        [field: SerializeField] public float CellSize { get; private set; } = 50f;
        [field: SerializeField] public float CellSpacing { get; private set; } = 2f;

        [field: Header("Number Colors")]
        [field: SerializeField] public Color Color1 { get; private set; } = Color.blue;
        [field: SerializeField] public Color Color2 { get; private set; } = Color.green;
        [field: SerializeField] public Color Color3 { get; private set; } = Color.red;
        [field: SerializeField] public Color Color4 { get; private set; } = new Color(0f, 0f, 0.5f);
        [field: SerializeField] public Color Color5 { get; private set; } = new Color(0.5f, 0f, 0f);
        [field: SerializeField] public Color Color6 { get; private set; } = new Color(0f, 0.5f, 0.5f);
        [field: SerializeField] public Color Color7 { get; private set; } = Color.black;
        [field: SerializeField] public Color Color8 { get; private set; } = Color.gray;

        public Color GetNumberColor(int number)
        {
            return number switch
            {
                1 => Color1,
                2 => Color2,
                3 => Color3,
                4 => Color4,
                5 => Color5,
                6 => Color6,
                7 => Color7,
                8 => Color8,
                _ => Color.white
            };
        }
    }
}
