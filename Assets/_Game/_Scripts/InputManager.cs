using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class InputManager : MonoBehaviour
    {
        private KeyActions _keyActions;
        
        /// <summary>
        /// Input system
        /// </summary>
        public KeyActions KeyActions => _keyActions;
        
        #region Evemts

        /// <summary>
        /// Event that is called after death to update the counter
        /// </summary>
        public event DeathCounter DeathCounterEvent;
        public delegate void DeathCounter(int count);
        
        
        /// <summary>
        /// Event that is called after player has put item on the table
        /// </summary>
        public event EndGame EndGameEvent;
        public delegate void EndGame();

        #endregion
        
        private void Awake()
        {
            _keyActions = new KeyActions();

            ChangeCursorState(CursorLockMode.Locked);
            Cursor.visible = false;
        }


        #region Enable/Disable
        
        private void OnEnable() =>
            _keyActions.Enable();

        private void OnDisable() =>
            _keyActions.Disable();
        
        #endregion


        #region Invokers

        public void DeathCounterEventInvoker(int count) =>
            DeathCounterEvent?.Invoke(count);

        public void EndGameEventInvoker() =>
            EndGameEvent?.Invoke();
        
        #endregion
        
        public void ChangeCursorState(CursorLockMode state)
        {
            Cursor.lockState = state;

            switch (state)
            {
                case CursorLockMode.Locked: 
                    Cursor.visible = false;
                    break;
                case CursorLockMode.Confined:
                    Cursor.visible = true;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}