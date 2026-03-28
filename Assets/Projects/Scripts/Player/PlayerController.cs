using UnityEngine;
using System;
using System.Collections.Generic;

public enum PlayerState
{
    Idle, PrepareWake, Moving, PrepareJump, Jumping
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerState _debug;
    private Dictionary<PlayerState, IPlayerState> _states;
    private IPlayerState _currentState;
    public PlayerState State { get; private set; }
    public Vector2 PointerPos { get; private set; }
    public bool IsPressed { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsWake { get; set; } // 起こす準備をスキップするかどうか

    private InputAdapter _input;
    private Camera _camera;
    private Vector2 _prevPos;

    [Header("移動判定しきい値")]
    [SerializeField] private float _moveThreshold = 0.05f;

    //public event Action OnReleaseTrigger;


    private void Awake()
    {
        _camera = Camera.main;

        _states = new Dictionary<PlayerState, IPlayerState>()
        {
            { PlayerState.Idle, GetComponent<IdleState>() },
            { PlayerState.PrepareWake, GetComponent<PrepareWakeState>() },
            { PlayerState.Moving, GetComponent<MoveState>() },
            { PlayerState.PrepareJump, GetComponent<PrepareJumpState>() },
            { PlayerState.Jumping, GetComponent<JumpState>() }
        };
    }

    private void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    public void SetInput(InputAdapter input)
    {
        _input = input;

        _input.OnPointerPress += OnPointerPress;
        _input.OnPointerMove += GetLocationInfo;
        _input.OnPointerRelease += Release;

        IsWake = false;

        foreach (var state in _states.Values)
        {
            state.Initialize(this);
        }
    }

    private void Update()
    {
        _debug = State;
        UpdateMovementFlag();
        _currentState?.Tick();
        // Debug.Log("update");
    }

    private void FixedUpdate()
    {
        _currentState?.FixedTick();
        // Debug.Log("fixedUpdate");
    }

    private void GetLocationInfo(Vector2 pos)
    {
        PointerPos = _camera.ScreenToWorldPoint(
            new Vector3(
                pos.x,
                pos.y,
                Mathf.Abs(_camera.transform.position.z)
            )
        );
    }

    private void UpdateMovementFlag()
    {
        if (!IsPressed)
        {
            IsMoving = false;
            return;
        }

        float move = (PointerPos - _prevPos).sqrMagnitude;
        IsMoving = move > _moveThreshold * _moveThreshold;
        _prevPos = PointerPos;
    }

    public void ChangeState(PlayerState next)
    {
        if (_currentState != null)
            _currentState.Exit();

        State = next;
        _currentState = _states[next];

        _currentState.Enter();
        //Debug.Log($"change to: {_currentState}");

    }

    private void Release()
    {
        //OnReleaseTrigger?.Invoke();
        ChangeState(PlayerState.Idle);
    }

    private void OnPointerPress(bool pressed)
    {
        IsPressed = pressed;
        ChangeState(PlayerState.PrepareWake);

    }

    private void OnDisable()
    {
        _input.OnPointerMove -= GetLocationInfo;
        _input.OnPointerPress -= OnPointerPress;
        _input.OnPointerRelease -= Release;
    }
}