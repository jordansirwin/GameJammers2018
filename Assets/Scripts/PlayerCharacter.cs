using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerDirection CurrentDirection { get; private set; }
    public Vector3 CurrentPosition
    {
        get { return gameObject.transform.position; }
    }

    private void Start()
    {
        CurrentDirection = PlayerDirection.Straight;
    }

    private void Update()
    {
        var newDirection = GetDirection();
        if(newDirection != CurrentDirection)
        {
            CurrentDirection = newDirection;
            AdjustRotation();
            UpdateGameManager();
        }
    }

    private void UpdateGameManager()
    {
        switch (CurrentDirection)
        {
            case PlayerDirection.Straight:
                GameManager.Instance.SetSpeedModifiers(0, 1);
                break;
            case PlayerDirection.Left:
                GameManager.Instance.SetSpeedModifiers(0.5f, 0.5f);
                break;
            case PlayerDirection.Right:
                GameManager.Instance.SetSpeedModifiers(-0.5f, 0.5f);
                break;
        }
    }

    private PlayerDirection GetDirection()
    {
        var direction = PlayerDirection.Straight;
        if (GameInputManager.Instance.LeftInputActive)
            direction = PlayerDirection.Left;
        else if (GameInputManager.Instance.RightInputActive)
            direction = PlayerDirection.Right;

        return direction;
    }

    private void AdjustRotation()
    {
        float newAngle = 0;

        if (CurrentDirection == PlayerDirection.Left)
            newAngle = -45;
        else if (CurrentDirection == PlayerDirection.Right)
            newAngle = 45;

        var currentRotation = gameObject.transform.rotation.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newAngle);
    }
}