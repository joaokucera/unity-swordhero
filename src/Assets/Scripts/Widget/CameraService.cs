using UnityEngine;

namespace Widget
{
    public interface ICameraService : IGameService
    {
        Camera MainCamera { get; }
    }

    public class CameraService : ICameraService
    {
        private Camera _mainCamera;
        public Camera MainCamera => _mainCamera ? _mainCamera : _mainCamera = Camera.main;
    }
}