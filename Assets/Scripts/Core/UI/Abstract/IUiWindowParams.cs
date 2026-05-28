namespace Core.UI.Abstract
{
    public interface IUiWindow<TW, in TP> : IUiWindow
        where TW : IUiWindow<TW, TP>
        where TP : IUiWindowParams<TW, TP>
    {
        void SetParameters(TP parameters);
    }

    public interface IUiWindowParams<TW, TP>
        where TW : IUiWindow<TW, TP>
        where TP : IUiWindowParams<TW, TP>
    {
    }
}
