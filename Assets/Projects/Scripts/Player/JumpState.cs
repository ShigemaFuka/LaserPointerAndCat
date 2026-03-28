using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpState : MonoBehaviour, IPlayerState
{
    [Header("最高到達高さ")]
    [SerializeField] private float _jumpHeight = 2f;
    [Header("ジャンプ全体時間")]
    [SerializeField] private float _jumpTime = 0.8f;
    [SerializeField] private float _length = 1f;
    [SerializeField] private Vector3 _offset = new(0, -1);
    [Header("最大移動範囲")]
    [SerializeField] private float _maxMoveRangeX = 2f;
    private PlayerController _controller;
    private Rigidbody2D _rb;
    private float _jumpStartTime;
    private Vector2 _pointerPos;
    private bool _jumpTriggered;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Enter()
    {
    }

    public void Tick()
    {
    }

    public void FixedTick()
    {
        // 三分の一を過ぎたところで戻す 物体の角だけスルー出来る程度を想定
        if (Time.time - _jumpStartTime >= _jumpTime / 3f) RaycastDown();

        TryJump();

        if (Time.time >= _jumpStartTime + _jumpTime)
        {
            _controller.ChangeState(PlayerState.Moving);
        }
    }

    public void Exit()
    {
        Init();
    }

    private void Init()
    {
        _jumpTriggered = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = 0;
    }

    private void TryJump()
    {
        if (_jumpTriggered) return;

        _pointerPos = _controller.PointerPos;
        Jump(_pointerPos);
    }

    private void Jump(Vector2 target)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerJump");

        Vector2 start = _rb.position;

        // X方向の移動距離を制限
        float distanceX = target.x - start.x;
        distanceX = Mathf.Clamp(distanceX, -_maxMoveRangeX, _maxMoveRangeX);

        // 速度計算
        float gravity = -(2 * _jumpHeight) / Mathf.Pow(_jumpTime / 2f, 2);
        float velocityY = (2 * _jumpHeight) / (_jumpTime / 2f);
        float velocityX = distanceX / _jumpTime;

        // 適用
        _rb.gravityScale = gravity / Physics2D.gravity.y;
        _rb.linearVelocity = new Vector2(velocityX, velocityY);

        _jumpStartTime = Time.time;

        _jumpTriggered = true;

        Debug.Log("Jump");
    }

    /// <summary>
    /// 真下に伸ばしたレイキャストで検知
    /// </summary>
    private void RaycastDown()
    {
        var pos = transform.position + _offset;

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, _length);

        if (hit.collider != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            Debug.DrawLine(pos, hit.point, Color.yellow);
            //Debug.Log("Hit: " + hit.collider.name);
        }
        else
        {
            Debug.DrawLine(pos, pos + Vector3.down * _length, Color.blue);
        }
    }
}