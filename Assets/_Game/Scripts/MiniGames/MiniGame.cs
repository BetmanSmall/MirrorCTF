using System.Collections;
using Thirdparty.MinMaxSlider.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
namespace _Game.Scripts.MiniGames {
    public class MiniGame : MonoBehaviour {
        [SerializeField] private MinMaxSlider minMaxSlider;
        [SerializeField] private Slider slider;
        [SerializeField] private float goodDistanceSize = 0.2f;
        [SerializeField] private float gameTime = 1.5f;
        [SerializeField] private float sliderMoveStep = 0.02f;
        [SerializeField] private float sliderMoveStepWait = 0.01f;
        [SerializeField] private Button userInteractButton;
        private float _gameTimeElapsed;
        private float _currentSliderMoveDirection;
        private Coroutine _currentCoroutine;

        public UnityEvent passed = new UnityEvent();
        public UnityEvent failed = new UnityEvent();

        public static MiniGame Instance { get; private set; }
        private void Awake() {
            // Debug.Log("MiniGame::Awake(); -- ");
            if (Instance != null && Instance != this) {
                Destroy(this);
            } else {
                Instance = this;
            }
            gameObject.SetActive(false);
        }

        private void Start() {
            // Debug.Log("MiniGame::Start(); -- ");
            userInteractButton?.onClick.AddListener(() => {
                if (slider.value >= minMaxSlider.Values.minValue && 
                    slider.value <= minMaxSlider.Values.maxValue) {
                    GamePassed();
                } else {
                    GameFailed();
                }
            });
            // NewGame();
        }

        public void NewGame() {
            // Debug.Log("MiniGame::NewGame(); -- ");
            _currentSliderMoveDirection = sliderMoveStep;
            float halfGoodDistanceSize = goodDistanceSize / 2f;
            float minGoodDistance = minMaxSlider.Values.minLimit + halfGoodDistanceSize;
            float maxGoodDistance = minMaxSlider.Values.maxLimit - halfGoodDistanceSize;
            float centerGoodDistance = Random.Range(minGoodDistance, maxGoodDistance);
            minMaxSlider.SetValues(centerGoodDistance - halfGoodDistanceSize, centerGoodDistance + halfGoodDistanceSize);
            _gameTimeElapsed = 0f;
        }

        public void NewGameStart() {
            // Debug.Log("MiniGame::NewGameStart(); -- ");
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(SliderMoveCoroutine());
        }

        private IEnumerator SliderMoveCoroutine() {
            // Debug.Log("MiniGame::SliderMoveCoroutine(); -- ");
            while (_gameTimeElapsed < gameTime) {
                slider.value += _currentSliderMoveDirection;
                if (_currentSliderMoveDirection > 0f && slider.value >= slider.maxValue ||
                    _currentSliderMoveDirection < 0f && slider.value <= slider.minValue) {
                    _currentSliderMoveDirection = -_currentSliderMoveDirection;
                }
                // _gameTimeElapsed += Time.deltaTime;
                // yield return new WaitForEndOfFrame();
                _gameTimeElapsed += sliderMoveStepWait;
                yield return new WaitForSeconds(sliderMoveStepWait);
            }
            GameFailed();
        }

        private void GamePassed() {
            // Debug.Log("MiniGame::GamePassed(); -- ");
            GameHide();
            passed?.Invoke();
        }

        private void GameFailed() {
            // Debug.Log("MiniGame::GameFailed(); -- ");
            GameHide();
            failed?.Invoke();
        }

        private void GameHide() {
            // Debug.Log("MiniGame::GameHide(); -- ");
            gameObject.SetActive(false);
        }

        private void OnEnable() {
            // Debug.Log("MiniGame::OnEnable(); -- ");
            gameObject.SetActive(true);
            enabled = true;
            NewGame();
            NewGameStart();
        }
    }
}