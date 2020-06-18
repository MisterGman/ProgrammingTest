using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        [field : SerializeField,
                 Tooltip("Timer text")]
        private TextMeshProUGUI timerNameText;
        
        [field : SerializeField,
                 Tooltip("Death counter text")]
        private TextMeshProUGUI deathNameText;
        
        [field : SerializeField,
                 Tooltip("Death counter text")]
        private GameObject congratulationText;
        
        [field : SerializeField,
                 Tooltip("Death counter text")]
        private Button endGameButton;

        [field : SerializeField,
                 Tooltip("Fade in image")]
        private Image fadeIn;

        [field : SerializeField,
                 Tooltip("Duration of FadeIn")]
        private float fadeInDuration;
        
        #endregion

        private float _currentTime;

        private Coroutine _timeCounter;
        
        #region Enable/Disable

        private void OnEnable()
        {
            InputManager.DeathCounterEvent += DeathCountUpdate;
            InputManager.EndGameEvent += EndGameScore;
        }

        private void OnDisable()
        {
            InputManager.DeathCounterEvent -= DeathCountUpdate;
            InputManager.EndGameEvent -= EndGameScore;
        }

        #endregion


        private void Start()
        {
            _timeCounter = StartCoroutine(TimeCounter());
            fadeIn.canvasRenderer.SetAlpha(0.0f);
            
            endGameButton.onClick.AddListener(Application.Quit);
            
            congratulationText.SetActive(false);
            endGameButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Stop timer and finish the game
        /// </summary>
        private void EndGameScore()
        {
            StopCoroutine(_timeCounter);

            StartCoroutine(EndScreeCoroutine());
            fadeIn.CrossFadeAlpha(1, fadeInDuration, true);
        }

        /// <summary>
        /// Update death counter's text
        /// </summary>
        /// <param name="count"></param>
        private void DeathCountUpdate(int count) =>
            deathText.text = $"{count}";

        /// <summary>
        /// Time counter during the whole game until the player is finished
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimeCounter()
        {
            while (true)
            {
                _currentTime += Time.deltaTime;
                timerText.text = $"{_currentTime:F1}";

                yield return null;
            }
        }

        /// <summary>
        /// Stop time and show ui
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndScreeCoroutine()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(fadeInDuration);
            
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
            timerText.color = Color.white;
            timerNameText.color = Color.white;
            deathText.color = Color.white;
            deathNameText.color = Color.white;
            congratulationText.SetActive(true);
            endGameButton.gameObject.SetActive(true);
        }
        
        
    }
}
