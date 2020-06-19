using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Game._Scripts
{
    public class Counters : MonoBehaviour
    {
        #region InspectorVisible
        
        [field : SerializeField,
                 Tooltip("Input manager of the game")]
        private InputManager inputManager;
        
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
        private float fadeInDuration = 2f;
        
        #endregion

        private float _currentTime;

        private Coroutine _timeCounter;
        private bool _isEndScreenCorRunning;
        
        #region Enable/Disable

        private void OnEnable()
        {
            inputManager.DeathCounterEvent += DeathCountUpdate;
            inputManager.EndGameEvent += EndGameScore;
        }

        private void OnDisable()
        {
            inputManager.DeathCounterEvent -= DeathCountUpdate;
            inputManager.EndGameEvent -= EndGameScore;
        }

        #endregion

        private void Start()
        {
            _timeCounter = StartCoroutine(TimeCounter());
            fadeIn.canvasRenderer.SetAlpha(0.0f);
            
#if UNITY_EDITOR
            endGameButton.onClick.AddListener(EditorApplication.ExitPlaymode);
#endif
            endGameButton.onClick.AddListener(Application.Quit);
        }

        /// <summary>
        /// Stop timer and finish the game
        /// </summary>
        private void EndGameScore()
        {
            if (_isEndScreenCorRunning)
                return;
            
            StopCoroutine(_timeCounter);

            _isEndScreenCorRunning = true;
            StartCoroutine(EndScreenCoroutine());
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
        private IEnumerator EndScreenCoroutine()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(fadeInDuration);
            
            inputManager.ChangeCursorState(CursorLockMode.Confined);
            
            timerText.color = timerNameText.color = deathText.color = deathNameText.color 
                = Color.white;
            congratulationText.SetActive(true);
            endGameButton.gameObject.SetActive(true);
        }
    }
}
