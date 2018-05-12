using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public enum GameStates {
        MainMenu,
        Playing,
        GameOver
    }

    private float _speedModifyX = 0;
    private float _speedModifyY = 1;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField]
    private KnobsForKevin _knobs;
    [Tooltip("Link to UIManager")]
    [SerializeField]
    private UIManager _uiManager;

    private float _startGameTime;
    private float _endGameTime;
    private GameStates _currentGameState;

    public float BaseSpeed { get { return _knobs.baseSpeed; } }
    public float GameSpeedX { get { return _speedModifyX * _knobs.baseSpeed; } }
    public float GameSpeedY { get { return _speedModifyY * _knobs.baseSpeed; } }

    public float Score { get { return CalculateScore(); } }

    private void Start()
    {
        //Intitialize();

        //// HACK: Should be called by menu interactions
        //StartNewGame();

        ChangeGameState(GameStates.MainMenu);
    }

    private void IntitializeNewGame()
    {
        _speedModifyX = 0;
        _speedModifyY = 1;
        _startGameTime = Time.time;
    }

    public void SetSpeedModifiers(float x, float y)
    {
        _speedModifyX = x;
        _speedModifyY = y;
    }

    public void StartNewGame() {
        Debug.Log("StartNewGame");
        ChangeGameState(GameStates.Playing);
    }

    public void GameOver() {
        ChangeGameState(GameStates.GameOver);
    }

    public void ExitGame() {
        Debug.Log("ExitGame");
        Application.Quit();
    }

    public int CalculateScore()
    {
        float currentSize = AvalancheManager.Instance.Size;
        // TODO: fancy calculations or something

        return (int)currentSize;
    }

    public float CalculateElapsedTimeSinceGameStart() {
        // if we're playing, keep updating end time
        if (_currentGameState == GameStates.Playing)
        {
            _endGameTime = Time.time;
        }

        return _endGameTime - _startGameTime;
    }

    public GameStates GetGameState() {
        return _currentGameState;
    }

    private void ChangeGameState(GameStates newState) {
        switch(newState) {
            case GameStates.MainMenu:
                _uiManager.ShowMainMenu();
                _uiManager.HideGameOver();
                _uiManager.ShowGameplayScreen();
                _uiManager.ShowGameplayTutorialScreen();
                break;
            case GameStates.Playing:
                _uiManager.HideMainMenu();
                _uiManager.HideGameOver();
                _uiManager.ShowGameplayScreen();
                _uiManager.HideGameplayTutorialScreen();
                IntitializeNewGame();
                break;
            case GameStates.GameOver:
                // main menu and game over should overlap :D
                _uiManager.ShowMainMenu();
                _uiManager.ShowGameOver();
                _uiManager.HideGameplayScreen();
                _uiManager.HideGameplayTutorialScreen();
                break;
        }

        _currentGameState = newState;
    }
}
