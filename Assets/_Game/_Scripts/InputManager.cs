using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        
        private KeyActions _keyActions;
        
        /// <summary>
        /// Input system
        /// </summary>
        public static KeyActions KeyActions => Instance._keyActions;

        /// <summary>
        /// Event that is called after death to update the counter
        /// </summary>
        public static event DeathCounter DeathCounterEvent;
        public delegate void DeathCounter(int count);
        
        
        /// <summary>
        /// Event that is called after player has put item on the table
        /// </summary>
        public static event EndGame EndGameEvent;
        public delegate void EndGame();

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

        public static void EndGameEventInvoker() =>
            EndGameEvent?.Invoke();
    }
}