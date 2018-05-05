using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private float _baseSpeed = 5;

    private float _speedModifyX = 0;
    private float _speedModifyY = 1;

    public float BaseSpeed { get { return _baseSpeed; } }
    public float GameSpeedX { get { return _speedModifyX * _baseSpeed; } }
    public float GameSpeedY { get { return _speedModifyY * _baseSpeed; } }

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
