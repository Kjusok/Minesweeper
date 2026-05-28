using UnityEngine;

namespace Core.ResourceSystem
{
    public class ResourceProvider : IResourceProvider
    {
        private const string UiPath = "UI/";

        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(UiPath + path);
        }
    }
}
