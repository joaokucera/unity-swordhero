using UnityEngine;

namespace Widget
{
    public class LootAtCameraComponent : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        private ICameraService _cameraService;
        
        private void Awake()
        {
            _cameraService = LocatorService.Instance.Get<ICameraService>();
            
            canvas.worldCamera = _cameraService.MainCamera;
        }

        private void Update()
        {
            if (_cameraService?.MainCamera != null)
            {
                transform.LookAt(_cameraService?.MainCamera.transform);
            }
        }
    }
}