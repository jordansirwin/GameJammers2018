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

    public float BaseSpeed { get { return _knobs.baseSpeed; } }
    public float GameSpeedX { get { return _speedModifyX * _knobs.baseSpeed; } }
    public float GameSpeedY { get { return _speedModifyY * _knobs.baseSpeed; } }

    public float Score { get { return CalculateScore(); } }

    private void Start()
    {
        Intitialize();
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

    private float CalculateScore()
    {
        float currentSize = AvalancheManager.Instance.Size;
        // TODO: fancy calculations or something

        return currentSize;
    }
}
