using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoSingleton<GameInputManager>
{
    [SerializeField] private PlayerCharacter _playerManager;
    [Tooltip("Enable use of Mouse to test touch controls")]
    [SerializeField] private bool _useMouseAsTouch = false;

    private bool _allowTouch = false;




    private Camera _mainCamera;

    public bool LeftInputActive { get; private set; }
    public bool RightInputActive { get; private set; }

    private void Awake()
    {
        LeftInputActive = false;
        RightInputActive = false;

#if UNITY_WEBGL
        _allowTouch = false;
#else
        _allowTouch = Input.touchSupported;
#endif
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // Testing hack
        if(Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.Instance.GameOver();
        }

        LeftInputActive = false;
        RightInputActive = false;

        if (_allowTouch)
        {
            GetInputsFromTouch();
        }
        else
        {
            if (_useMouseAsTouch)
            {
                GetInputFromMouse();
            }
            else
            {
                GetInputFromKeys();
            }
        }
    }

    private void GetInputsFromTouch()
    {
        if (Input.touchCount > 0)
        {
            var touchPosition = Input.GetTouch(0).position;

            // Use either player position or center of screen
            var focalPosition = _playerManager == null
                ? new Vector3(_mainCamera.pixelWidth / 2, 0)
                : _mainCamera.WorldToScreenPoint(_playerManager.CurrentPosition);

            if (touchPosition.x < focalPosition.x)
            {
                LeftInputActive = true;
                RightInputActive = false;
            }
            else if (touchPosition.x > focalPosition.x)
            {
                RightInputActive = true;
                LeftInputActive = false;
            }
        }
    }

    private void GetInputFromMouse()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;

            // Use either player position or center of screen
            var focalPosition = _playerManager == null
                ? new Vector3(_mainCamera.pixelWidth / 2, 0)
                : _mainCamera.WorldToScreenPoint(_playerManager.CurrentPosition);

            if (mousePosition.x < focalPosition.x)
            {
                LeftInputActive = true;
                RightInputActive = false;
            }
            else if (mousePosition.x > focalPosition.x)
            {
                RightInputActive = true;
                LeftInputActive = false;
            }
        }
    }

    private void GetInputFromKeys()
    {
        LeftInputActive = Input.GetAxis("Horizontal") < 0; //Input.GetKey(KeyCode.LeftArrow);
        RightInputActive = Input.GetAxis("Horizontal") > 0; //Input.GetKey(KeyCode.RightArrow);
    }
}
