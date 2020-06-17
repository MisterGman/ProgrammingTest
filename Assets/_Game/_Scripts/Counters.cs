using System;
using TMPro;
using UnityEngine;

namespace _Game._Scripts
{
    public class Counters : MonoBehaviour
    {
        #region InspectorVisible

        [field : SerializeField,
                 Tooltip("Timer text")]
        private TextMeshProUGUI timerText;
        
        [field : SerializeField,
                 Tooltip("Death counter text")]
        private TextMeshProUGUI deathText;
        
        #endregion

        private float _currentTime;
        
        #region Enable/Disable

        private void OnEnable()
        {
            InputManager.DeathCounterEvent += DeathCountUpdate;
        }

        private void OnDisable()
        {
            InputManager.DeathCounterEvent -= DeathCountUpdate;
        }

        #endregion


        /// <summary>
        /// Update death counter's text
        /// </summary>
        /// <param name="count"></param>
        private void DeathCountUpdate(int count) =>
            deathText.text = $"{count}";

        private void FixedUpdate()
        {
            _currentTime += Time.deltaTime;
            timerText.text = $"{_currentTime:F1}";
        }
    }
}
