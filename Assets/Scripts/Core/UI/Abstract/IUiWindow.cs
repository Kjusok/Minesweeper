using UnityEngine;

namespace Core.UI.Abstract
{
    public interface IUiWindow<TW> : IUiWindow
        where TW : IUiWindow<TW>
    {
    }

    public interface IUiWindow
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        void OnShow() { }
    }
}
