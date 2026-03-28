using UnityEngine;

public class IdleState : MonoBehaviour, IPlayerState
{
    [SerializeField] private float _wakeLimit = 10f;
    private float _wakeElapsed;
    private PlayerController _controller;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
        _wakeElapsed = 0f;

    }

    public void Enter()
    {
    }

    public void Tick()
    {
        if (_controller.IsWake)
        {
            _wakeElapsed += Time.deltaTime;

            if (_wakeElapsed >= _wakeLimit)
            {
                _controller.IsWake = false;
                _wakeElapsed = 0f;
            }

        }
    }

    public void FixedTick()
    {
    }

    public void Exit()
    {
    }
}