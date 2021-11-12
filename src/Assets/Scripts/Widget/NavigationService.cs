using System;
using UnityEngine;

namespace Widget
{
    public interface INavigationService : IGameService
    {
        public event Action<RaycastHit> OnRaycastHit;
    }

    public class NavigationService : INavigationService, IDisposable
    {
        public event Action<RaycastHit> OnRaycastHit = delegate { };
    
        private readonly ICameraService _cameraService;
        private readonly IInputService _inputService;
        private readonly RaycastHit[] _raycastHits = new RaycastHit[1];
    
        public NavigationService(ICameraService cameraService, IInputService inputService)
        {
            _cameraService = cameraService;
            _inputService = inputService;
            
            _inputService.OnHeldDownAnywhere += HeldDownAnywhere;
        }

        public void Dispose()
        {
            _inputService.OnHeldDownAnywhere -= HeldDownAnywhere;
        }
    
        private void HeldDownAnywhere()
        {
            Ray ray = _cameraService.MainCamera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.RaycastNonAlloc(ray, _raycastHits) > 0)
            {
                OnRaycastHit(_raycastHits[0]);
            }
        }
    }
}