using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheManager : MonoSingleton<AvalancheManager>
{
    [SerializeField] private GameObject _avalanchePrefab;
    [SerializeField] private GameObject _avalanceContainer;
    [SerializeField] private GameObject _topMostPosition;
    [SerializeField] private GameObject _startPosition;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField] private KnobsForKevin _knobs;

    [Tooltip("How far Avalanche can be from target position to be acceptably close.")]
    [SerializeField] private float _moveMarginOfError = 0.001f;

    private float _maxY;

    private GameObject _avalancheObject;
    private Coroutine _moveToEncroachmentRoutine;
    private Vector3 _targetEncroachPosition;

    [SerializeField] private float _size;
    public float Size { get { return _size; } }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _maxY = _topMostPosition.transform.position.y;

        var initialPosition = _startPosition.transform.position;

        _avalancheObject = GameObject.Instantiate<GameObject>(_avalanchePrefab, initialPosition, Quaternion.identity, _avalanceContainer.transform);
        _targetEncroachPosition = _avalancheObject.transform.position;
    }

    public void ModifySize(float amount)
    {
        _size += amount;
        // TODO: Visual fanciness adjustments
    }

    public void ModifyEncroachment(float amount)
    {
        // TODO: Scale this rather than directly modifying Y?
        float newY = _targetEncroachPosition.y + amount;

        _targetEncroachPosition.y = Mathf.Min(_maxY, newY);

        MoveToTarget();
    }

    private void MoveToTarget(bool force = false)
    {
        if(_moveToEncroachmentRoutine != null)
        {
            StopCoroutine(_moveToEncroachmentRoutine);
            _moveToEncroachmentRoutine = null;
        }

        if (force)
            _avalancheObject.transform.position = _targetEncroachPosition;
        else
            _moveToEncroachmentRoutine = StartCoroutine(MoveToTargetGradual());
    }

    IEnumerator MoveToTargetGradual()
    {
        while (Vector3.Distance(_avalancheObject.transform.position, _targetEncroachPosition) > _moveMarginOfError)
        {
            var moveRate = _knobs.encroachSpeedMultiplier * GameManager.Instance.BaseSpeed;
            var step = moveRate * Time.deltaTime;
            _avalancheObject.transform.position = Vector3.MoveTowards(_avalancheObject.transform.position, _targetEncroachPosition, step);

            yield return null;
        }
    }

    [SerializeField] float _debugEncroachModify = 0;
    [ContextMenu("ModifyEncroachmentByDebug")]
    public void DebugModify()
    {
        ModifyEncroachment(_debugEncroachModify);
    }
}
