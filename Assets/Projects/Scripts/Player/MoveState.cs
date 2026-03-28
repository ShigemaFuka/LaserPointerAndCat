using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveState : MonoBehaviour, IPlayerState
{
    [Header("移動")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _offset = 5f;
    [Header("停止判定")]
    [SerializeField] private float _stopTime = 0.7f;
    private PlayerController _controller;
    private Rigidbody2D _rb;
    private float _elapsed;
    private Vector3 _targetPosition;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
        _rb = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Enter()
    {
        Init();
    }

    public void Tick()
    {
        if (!_controller.IsMoving)
        {
            _elapsed += Time.deltaTime;

            if (_elapsed >= _stopTime)
            {
                _controller.ChangeState(PlayerState.PrepareJump);
            }
        }
        else
        {
            _elapsed = 0f;
        }
    }

    public void FixedTick()
    {
        Move();
    }

    public void Exit()
    {
        Init();
    }

    private void Init()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = 0;
        _elapsed = 0f;
    }

    private void Move()
    {
        _targetPosition = _controller.PointerPos;

        Vector2 target = _targetPosition;
        var diff = _rb.position - target;

        if (diff.sqrMagnitude > _offset * _offset)
        {
            Vector3 stopPos = target + (_rb.position - target).normalized;
            Vector3 dir = ((Vector2)stopPos - _rb.position).normalized;
            _rb.linearVelocity = new Vector3(dir.x * _speed, _rb.linearVelocity.y, dir.z * _speed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _offset);
    }
}
