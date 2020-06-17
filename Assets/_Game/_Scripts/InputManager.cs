using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        
        private KeyActions _keyActions;
        public static KeyActions KeyActions => Instance._keyActions;

        /// <summary>
        /// Event that is called after death to update the counter
        /// </summary>
        public static event DeathCounter DeathCounterEvent;
        public delegate void DeathCounter(int count);

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
            
            _keyActions = new KeyActions();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        private void OnEnable() =>
            _keyActions.Enable();

        private void OnDisable() =>
            _keyActions.Disable();

        public static void DeathCounterEventInvoker(int count) =>
            DeathCounterEvent?.Invoke(count);
    }
}