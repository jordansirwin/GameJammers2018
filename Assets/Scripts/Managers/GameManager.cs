using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // Config values
    [SerializeField] private float _baseSpeedStart;

    [SerializeField] private float _baseSpeed;

    public float BaseSpeed { get { return _baseSpeed; } }
    public float Score { get { return CalculateScore(); } }

    private void Start()
    {
        Intitialize();
    }

    public void Intitialize()
    {
        _baseSpeed = _baseSpeedStart;
    }

    public void ModifyBaseSpeed(float amount)
    {
        _baseSpeed += amount;
        // TODO: Affect other things (avalanche)
    }

    private float CalculateScore()
    {
        float currentSize = AvalancheManager.Instance.Size;
        // TODO: fancy calculations or something

        return currentSize;
    }
}
