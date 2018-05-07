using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private float _speedModifyX = 0;
    private float _speedModifyY = 1;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField]
    private KnobsForKevin _knobs;

    // time game started so we can calculate elapased time
    private float _startGameTIme;

    public float BaseSpeed { get { return _knobs.baseSpeed; } }
    public float GameSpeedX { get { return _speedModifyX * _knobs.baseSpeed; } }
    public float GameSpeedY { get { return _speedModifyY * _knobs.baseSpeed; } }

    public float Score { get { return CalculateScore(); } }

    private void Start()
    {
        Intitialize();

        // HACK: Should be called by menu interactions
        StartNewGame();
    }

    public void Intitialize()
    {
        _speedModifyX = 0;
        _speedModifyY = 1;
    }

    public void SetSpeedModifiers(float x, float y)
    {
        _speedModifyX = x;
        _speedModifyY = y;
    }

    public void StartNewGame() {
        Intitialize();
        _startGameTIme = Time.time;
        // reset other things here
    }

    public int CalculateScore()
    {
        float currentSize = AvalancheManager.Instance.Size;
        // TODO: fancy calculations or something

        return (int)currentSize;
    }

    public float CalculateElapsedTimeSinceGameStart() {
        return Time.time - _startGameTIme;
    }
}
