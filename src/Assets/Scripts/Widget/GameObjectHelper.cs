using UnityEngine;

namespace Widget
{
    public static class GameObjectHelper
    {
        public static T CreateGameObject<T>(string name) where T : Component
        {
#if UNITY_EDITOR
            GameObject go = new GameObject(name);
#else
            GameObject go = new GameObject(name) {hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector};
#endif

            return go.AddComponent<T>();
        }
    }
}