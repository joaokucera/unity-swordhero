using System;
using UnityEngine;

namespace Widget
{
    public interface IInputService : IGameService
    {
        event Action OnHeldDownAnywhere;
        event Action OnWeaponKeysPressed;
        event Action OnReloadKeyPressed;
    }

    public class InputService : MonoBehaviour, IInputService
    {
        public event Action OnHeldDownAnywhere = delegate { };
        public event Action OnWeaponKeysPressed = delegate { };
        public event Action OnReloadKeyPressed = delegate { };

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                OnHeldDownAnywhere();
            }

            if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.W))
            {
                OnWeaponKeysPressed();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                OnReloadKeyPressed();
            }
        }
    }
}