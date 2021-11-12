using System.Collections.Generic;
using UnityEngine;

namespace Widget
{
    public interface IPoolService : IGameService
    {
        GameObject GetPooledObject(GameObject prefab);
        void ReturnPooledObject(GameObject objectToReturn);
        void ReturnPooledObject(string assetName, GameObject prefab);
        void ReturnAllPooledChildren(GameObject container);
    }

    public class PoolService : MonoBehaviour, IPoolService
    {
        private readonly Dictionary<string, Stack<GameObject>> _poolsByAssetName =
            new Dictionary<string, Stack<GameObject>>();

        private Stack<GameObject> GetPool(string assetName)
        {
            _poolsByAssetName.TryGetValue(assetName, out Stack<GameObject> stack);

            if (stack == null)
            {
                stack = new Stack<GameObject>();
                _poolsByAssetName.Add(assetName, stack);
            }

            return stack;
        }

        private GameObject GetPooledObjectInternal(string assetName, GameObject prefab)
        {
            Stack<GameObject> stack = GetPool(assetName);
            GameObject pooledObject;

            if (stack.Count == 0)
            {
                pooledObject = Instantiate(prefab);
                PoolMember poolMember = pooledObject.AddComponent<PoolMember>();
                poolMember.Init(this, assetName);
            }
            else
            {
                pooledObject = stack.Pop();
            }

            pooledObject.SetActive(true);
            return pooledObject;
        }

        public GameObject GetPooledObject(GameObject prefab)
        {
            return GetPooledObjectInternal(prefab.name, prefab);
        }

        public void ReturnPooledObject(GameObject objectToReturn)
        {
            if (objectToReturn == null)
            {
                return;
            }

            PoolMember poolMember = objectToReturn.GetComponentInChildren<PoolMember>(true);
            if (poolMember != null)
            {
                poolMember.ReturnToPool();
            }
        }

        public void ReturnPooledObject(string assetName, GameObject objectToReturn)
        {
            objectToReturn.SetActive(false);
            
#if UNITY_EDITOR
            objectToReturn.transform.SetParent(transform, false);
#else
            objectToReturn.transform.SetParent(null, false);
#endif

            Stack<GameObject> stack = GetPool(assetName);
            stack.Push(objectToReturn);
        }

        public void ReturnAllPooledChildren(GameObject container)
        {
            if (container == null)
            {
                return;
            }

            PoolMember[] pooledObjects = container.GetComponentsInChildren<PoolMember>(true);
            foreach (PoolMember poolMember in pooledObjects)
            {
                poolMember.ReturnToPool();
            }
        }
    }
}