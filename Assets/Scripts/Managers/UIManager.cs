using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("General")]

    [Tooltip("Link to the GameManager")]
    [SerializeField]
    private GameManager _gameManager;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField]
    private KnobsForKevin _knobs;


    [Header("Gameplay")]

    [Tooltip("Link to panel for score tracking during gameplay")]
    [SerializeField]
    private Transform _gameplayPanel;

    [Tooltip("Link to text label showing bonus points scored")]
    [SerializeField]
    private Text _bonusText;
    [Tooltip("Link to text label showing when goal points scored")]
    [SerializeField]
    private Text _goalAchievedText;
    [Tooltip("How many seconds until Bonus Text fades out")]
    [SerializeField]
    private float _bonusTextFadeInSec;

    [Tooltip("Link to text label showing next goal score")]
    [SerializeField]
    private Text _goalText;
    [Tooltip("Link to text label showing player's current score")]
    [SerializeField]
    private Text _playerScoreText;
    [Tooltip("Link to text label showing elapsed game time")]
    [SerializeField]
    private Text _timeText;


    [Header("Main Menu")]

    [Tooltip("Link to panel for Main Menu")]
    [SerializeField]
    private Transform _mainMenuPanel;
    [Tooltip("Link to panel for Main Menu")]
    [SerializeField]
    private Transform _gameplayTutorialPanel;


    [Header("Score Screen")]

    [Tooltip("Link to panel for Score Screen Menu")]
    [SerializeField]
    private Transform _gameOverPanel;
    [Tooltip("Link to text label showing the avalanche size")]
    [SerializeField]
    private Text _avalancheSizeText;
    [Tooltip("Link to text label showing player's ride time")]
    [SerializeField]
    private Text _rideTimeText;


    private int _goalScoreIndex = 0;

	private void Start()
	{
        InvokeRepeating("UpdateScore", 0f, 0.1f);
	}

    void UpdateScore () {

        // get current score
        var currentScore = _gameManager.CalculateScore();

        // gameplay screen
        switch(_gameManager.GetGameState()) {
            case GameState.MainMenu:
                // no score tracking in main menu
                return;
            case GameState.Playing:
                _playerScoreText.text = currentScore.ToString();
                _goalText.text = GetNextGoalScore(currentScore).ToString();
                _timeText.text = ((int)_gameManager.CalculateElapsedTimeSinceGameStart()).ToString() + "s";
                break;
            case GameState.GameOver:
                _avalancheSizeText.text = currentScore.ToString();
                _rideTimeText.text = ((int)_gameManager.CalculateElapsedTimeSinceGameStart()).ToString() + "s";
                break;
        }
	}

    private int GetNextGoalScore(int currentScore)
    {
        if (_knobs.goalScores[_goalScoreIndex] <= currentScore)
        {
            if (_goalScoreIndex < _knobs.goalScores.Length) {
                _goalScoreIndex++;
                ShowGoalAchieved();
            }
            else
                Debug.LogWarning("No more goal scores to achieve!");
        }

        return _knobs.goalScores[_goalScoreIndex];
    }

    public void ShowGoalAchieved() {
        // reset alpha to make it visible
        var c = _goalAchievedText.color;
        _goalAchievedText.color = new Color(c.r, c.g, c.b, 1.0f);
        // fade out
        StartCoroutine(FadeTextElement(_goalAchievedText));
    }

    public void ShowBonusScore(int bonusAmount) {
        // assign bonus text
        _bonusText.text = "BONUS!";
        // reset alpha to make it visible
        var c = _bonusText.color;
        _bonusText.color = new Color(c.r, c.g, c.b, 1.0f);
        // fade out
        StartCoroutine(FadeTextElement(_bonusText));
    }

    private IEnumerator FadeTextElement(Text uiText) {
        // fade out over a period of time by reducing the alpha
        const float fadeDelay = 0.25f;
        var fadeAmount = 1f / (_bonusTextFadeInSec/fadeDelay);
        while (uiText.color.a > 0) {
            var c = uiText.color;
            uiText.color = new Color(c.r, c.g, c.b, c.a - fadeAmount);
            yield return new WaitForSeconds(fadeDelay);
        }
    }

    public void ShowMainMenu() {
        _mainMenuPanel.gameObject.SetActive(true);
    }

    public void HideMainMenu() {
        _mainMenuPanel.gameObject.SetActive(false);
    }

    public void ShowGameOver() {
        _gameOverPanel.gameObject.SetActive(true);
    }

    public void HideGameOver() {
        _gameOverPanel.gameObject.SetActive(false);
    }

    public void ShowGameplayScreen()
    {
        _gameplayPanel.gameObject.SetActive(true);
    }

    public void HideGameplayScreen()
    {
        _gameplayPanel.gameObject.SetActive(false);
    }

    public void ShowGameplayTutorialScreen()
    {
        _gameplayTutorialPanel.gameObject.SetActive(true);
    }

    public void HideGameplayTutorialScreen()
    {
        _gameplayTutorialPanel.gameObject.SetActive(false);
    }
}
