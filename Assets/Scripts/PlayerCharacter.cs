using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField] private KnobsForKevin _knobs;
    [SerializeField] private GameObject _bottomMostPosition;
    [Tooltip("How far Avalanche can be from target position to be acceptably close.")]
    [SerializeField] private float _moveMarginOfError = 0.001f;

    private float _minY;
    private Coroutine _moveToFallbackRoutine;
    private Vector3 _targetFallbackPosition;

    public PlayerDirection CurrentDirection { get; private set; }
    public Vector3 CurrentPosition
    {
        get { return gameObject.transform.position; }
    }

    private void Start()
    {
        CurrentDirection = PlayerDirection.Straight;
        _minY = _bottomMostPosition.transform.position.y;
    }

    private void Update()
    {
        var newDirection = GetDirection();
        if(newDirection != CurrentDirection)
        {
            CurrentDirection = newDirection;
            AdjustRotation();
            UpdateGameManagerSpeed();
        }
    }

    public void ModifyFallback(float amount)
    {
        float newY = _targetFallbackPosition.y + amount;
        _targetFallbackPosition.y = Mathf.Max(_minY, newY);
        MoveToTargetFallback();
    }

    private void MoveToTargetFallback(bool force = false)
    {
        if(_moveToFallbackRoutine != null)
        {
            StopCoroutine(_moveToFallbackRoutine);
            _moveToFallbackRoutine = null;
        }

        if (force)
            gameObject.transform.position = _targetFallbackPosition;
        else
            _moveToFallbackRoutine = StartCoroutine(MoveToTargetGradual());
    }

    IEnumerator MoveToTargetGradual()
    {
        while (Vector3.Distance(gameObject.transform.position, _targetFallbackPosition) > _moveMarginOfError)
        {
            var moveRate = _knobs.encroachSpeedMultiplier * GameManager.Instance.BaseSpeed;
            var step = moveRate * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _targetFallbackPosition, step);

            yield return null;
        }
    }

    private void UpdateGameManagerSpeed()
    {
        // get modifiers from knobs
        var mod = _knobs.playerTurnSpeedModifier;

        switch (CurrentDirection)
        {
            case PlayerDirection.Straight:
                GameManager.Instance.SetSpeedModifiers(0, 1);
                break;
            case PlayerDirection.Left:
                GameManager.Instance.SetSpeedModifiers(mod.x, mod.y);
                break;
            case PlayerDirection.Right:
                GameManager.Instance.SetSpeedModifiers(-mod.x, mod.y);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player OnTriggerEnter!");
        if (other.tag != "Avalanche")
            return;

        Debug.Log("Player hit avalanche!");
        GameManager.Instance.GameOver();
    }

    [SerializeField] float _debugEncroachModify = 0;
    [ContextMenu("ModifyEncroachmentByDebug")]
    public void DebugModify()
    {
        ModifyFallback(_debugEncroachModify);
    }
}