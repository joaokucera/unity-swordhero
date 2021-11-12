using UnityEngine;

namespace Widget
{
    public class PoolMember : MonoBehaviour
    {
        private IPoolService _poolService;
        private string _assetName;

        public void Init(IPoolService poolService, string assetName)
        {
            _poolService = poolService;
            _assetName = assetName;
        }

        public void ReturnToPool()
        {
            _poolService.ReturnPooledObject(_assetName, gameObject);
        }
    }
}