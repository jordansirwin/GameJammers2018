using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    [Tooltip("Link to text label showing next goal score")]
    [SerializeField]
    private Text _goalText;
    [Tooltip("Link to text label showing player's current score")]
    [SerializeField]
    private Text _playerScoreText;
    [Tooltip("Link to text label showing elapsed game time")]
    [SerializeField]
    private Text _timeText;

    [Tooltip("Link to the GameManager")]
    [SerializeField]
    private GameManager _gameManager;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField]
    private KnobsForKevin _knobs;

    private int _goalScoreIndex = 0;

	private void Start()
	{
        InvokeRepeating("UpdateScore", 1f, 0.25f);
	}

    void UpdateScore () {

        // get current score
        var currentScore = _gameManager.CalculateScore();
        _playerScoreText.text = currentScore.ToString();
        _goalText.text = GetNextGoalScore(currentScore).ToString();
        _timeText.text = ((int)_gameManager.CalculateElapsedTimeSinceGameStart()).ToString() + "s";
	}

    private int GetNextGoalScore(int currentScore)
    {
        if (_knobs.goalScores[_goalScoreIndex] <= currentScore)
        {
            if (_goalScoreIndex < _knobs.goalScores.Length)
                _goalScoreIndex++;
            else
                Debug.LogWarning("No more goal scores to achieve!");
        }

        return _knobs.goalScores[_goalScoreIndex];
    }
}
