using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    private float _speedModifyX = 0;
    private float _speedModifyY = 1;
    private float _baseSpeed;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField]
    private KnobsForKevin _knobs;
    [Tooltip("Link to UIManager")]
    [SerializeField]
    private UIManager _uiManager;

    private float _startGameTime;
    private float _endGameTime;
    private GameState _currentGameState;

    private Coroutine _speedRoutine;
    private Coroutine _sizeRoutine;

    public float BaseSpeed { get { return _baseSpeed; } }
    public float GameSpeedX { get { return _speedModifyX * _baseSpeed; } }
    public float GameSpeedY { get { return _speedModifyY * _baseSpeed; } }

    public float Score { get { return CalculateScore(); } }

    public GameStateEvent OnGameStateChange = new GameStateEvent();

    private void Start()
    {
        //Intitialize();

        //// HACK: Should be called by menu interactions
        //StartNewGame();

        ChangeGameState(GameState.MainMenu);
        _baseSpeed = _knobs.baseSpeed;
    }

    private void IntitializeNewGame()
    {
        _speedModifyX = 0;
        _speedModifyY = 1;
        _baseSpeed = _knobs.baseSpeed;

        _startGameTime = Time.time;

        StopAdjustmentRoutines();

        _speedRoutine = StartCoroutine(SpeedUpdateRoutine());
        _sizeRoutine = StartCoroutine(SizeUpdateRoutine());
    }

    public void SetSpeedModifiers(float x, float y)
    {
        _speedModifyX = x;
        _speedModifyY = y;
    }

    public void StartNewGame() {
        Debug.Log("StartNewGame");
        ChangeGameState(GameState.Playing);
    }

    public void GameOver() {
        ChangeGameState(GameState.GameOver);
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

    public void AddBonusScore(int bonusAmount) {
        AvalancheManager.Instance.ModifySize(bonusAmount);
        _uiManager.ShowBonusScore(bonusAmount);
    }

    public float CalculateElapsedTimeSinceGameStart() {
        // if we're playing, keep updating end time
        if (_currentGameState == GameState.Playing)
        {
            _endGameTime = Time.time;
        }

        return _endGameTime - _startGameTime;
    }

    IEnumerator SpeedUpdateRoutine()
    {
        while (true) // yolo
        {
            _baseSpeed += _knobs.speedIncreaseAmount;
            yield return new WaitForSeconds(_knobs.speedIncreaseRate);
        }
    }

    IEnumerator SizeUpdateRoutine()
    {
        while (true) // yolo
        {
            AvalancheManager.Instance.ModifySize(_knobs.avalancheSizeIncreaseAmount);
            yield return new WaitForSeconds(_knobs.avalancheSizeIncreaseRate);
        }
    }

    public GameState GetGameState() {
        return _currentGameState;
    }

    private void StopAdjustmentRoutines()
    {
        if (_speedRoutine != null)
        {
            StopCoroutine(_speedRoutine);
            _speedRoutine = null;
        }
        if (_sizeRoutine != null)
        {
            StopCoroutine(_sizeRoutine);
            _sizeRoutine = null;
        }
    }

    private void ChangeGameState(GameState newState) {
        switch(newState) {
            case GameState.MainMenu:
                _uiManager.ShowMainMenu();
                _uiManager.HideGameOver();
                _uiManager.ShowGameplayScreen();
                _uiManager.ShowGameplayTutorialScreen();
                StopAdjustmentRoutines();
                break;
            case GameState.Playing:
                _uiManager.HideMainMenu();
                _uiManager.HideGameOver();
                _uiManager.ShowGameplayScreen();
                _uiManager.HideGameplayTutorialScreen();
                IntitializeNewGame();
                break;
            case GameState.GameOver:
                // main menu and game over should overlap :D
                _uiManager.ShowMainMenu();
                _uiManager.ShowGameOver();
                _uiManager.HideGameplayScreen();
                _uiManager.HideGameplayTutorialScreen();
                StopAdjustmentRoutines();
                break;
        }

        OnGameStateChange.Invoke(newState);

        _currentGameState = newState;
    }
}
