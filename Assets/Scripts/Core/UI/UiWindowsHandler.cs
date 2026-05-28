using System;
using System.Collections.Generic;
using Core.ResourceSystem;
using Core.UI.Abstract;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class UiWindowsHandler : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private readonly Dictionary<Type, IUiWindow> _windows = new();

        private IResourceProvider _resourceProvider;
        private DiContainer _container;

        [Inject]
        private void Construct(IResourceProvider resourceProvider, DiContainer container)
        {
            _resourceProvider = resourceProvider;
            _container = container;
        }

        public TW Show<TW>() where TW : class, IUiWindow<TW>
        {
            var window = GetOrCreate<TW>();
            window.transform.SetAsLastSibling();
            window.gameObject.SetActive(true);
            window.OnShow();
            return window;
        }

        public TW Show<TW, TP>(TP parameters)
            where TW : class, IUiWindow<TW, TP>
            where TP : IUiWindowParams<TW, TP>
        {
            var window = GetOrCreate<TW>();
            window.SetParameters(parameters);
            window.transform.SetAsLastSibling();
            window.gameObject.SetActive(true);
            window.OnShow();
            return window;
        }

        public void Hide<TW>() where TW : class, IUiWindow
        {
            if (_windows.TryGetValue(typeof(TW), out var window))
            {
                window.gameObject.SetActive(false);
            }
        }

        public void HideAll()
        {
            foreach (var window in _windows.Values)
            {
                window.gameObject.SetActive(false);
            }
        }

        private TW GetOrCreate<TW>() where TW : class, IUiWindow
        {
            var type = typeof(TW);

            if (_windows.TryGetValue(type, out var existing))
            {
                return existing as TW;
            }
            
            var prefab = _resourceProvider.Load<GameObject>(type.Name);
            var instance = _container.InstantiatePrefab(prefab, _canvas.transform);
            instance.SetActive(false);

            var component = instance.GetComponent<TW>();
            _windows.Add(type, component);

            return component;
        }
    }
}
