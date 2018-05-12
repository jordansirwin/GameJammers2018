﻿using System.Collections;
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


    private int _goalScoreIndex = 0;

	private void Start()
	{
        InvokeRepeating("UpdateScore", 0f, 0.1f);
	}

    void UpdateScore () {
        // if not playing the game, do nothing
        if (_gameManager.GetGameState() != GameManager.GameStates.Playing)
        {
            return;
        }

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