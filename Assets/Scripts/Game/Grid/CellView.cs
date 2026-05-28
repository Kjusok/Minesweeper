using System;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Game.Grid
{
    public class CellView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _numberText;

        [Header("Sprites")]
        [SerializeField] private Sprite _hiddenSprite;
        [SerializeField] private Sprite _revealedSprite;
        [SerializeField] private Sprite _flagSprite;
        [SerializeField] private Sprite _mineSprite;

        private GridConfig _config;

        public CellData Data { get; private set; }

        public event Action<CellView, PointerEventData.InputButton> OnClicked;

        [Inject]
        private void Construct(GridConfig config)
        {
            _config = config;
        }

        public void Initialize(CellData data)
        {
            Data = data;
            UpdateView();
        }

        public void UpdateView()
        {
            _numberText.gameObject.SetActive(false);
            _icon.gameObject.SetActive(false);

            if (!Data.IsRevealed)
            {
                _background.sprite = _hiddenSprite;

                if (!Data.IsFlagged)
                {
                    return;
                }
                
                _icon.gameObject.SetActive(true);
                _icon.sprite = _flagSprite;

                return;
            }

            _background.sprite = _revealedSprite;

            if (Data.HasMine)
            {
                _icon.gameObject.SetActive(true);
                _icon.sprite = _mineSprite;

                return;
            }

            if (Data.AdjacentMines <= 0)
            {
                return;
            }
            _numberText.gameObject.SetActive(true);
            _numberText.text = Data.AdjacentMines.ToString();
            _numberText.color = _config.GetNumberColor(Data.AdjacentMines);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this, eventData.button);
        }

        public class Factory : PlaceholderFactory<CellView> { }
    }
}