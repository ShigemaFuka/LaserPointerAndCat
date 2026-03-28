using UnityEngine;

public class PrepareJumpState : MonoBehaviour, IPlayerState
{
    [SerializeField] private float _stopTime = 2f;
    [Header("経過時間")]
    [SerializeField] private float _elapsed;
    private PlayerController _controller;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        _elapsed = 0f;
    }

    public void Tick()
    {
        if (!_controller.IsMoving)
        {
            _elapsed += Time.deltaTime;
        }
        else
        {
            _controller.ChangeState(PlayerState.Moving);
            _elapsed = 0;
        }

        if (_elapsed >= _stopTime)
        {
            _controller.ChangeState(PlayerState.Jumping);
            _elapsed = 0f;
        }
    }

    public void Exit()
    {
        _elapsed = 0f;
    }

    public void FixedTick() { }
}
