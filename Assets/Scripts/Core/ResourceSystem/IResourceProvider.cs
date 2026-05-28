using UnityEngine;

namespace Core.ResourceSystem
{
    public interface IResourceProvider
    {
        T Load<T>(string path) where T : Object;
    }
}
