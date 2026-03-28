using UnityEngine;

/// <summary>
/// 起きた状態ならすぐMoveに遷移する
/// </summary>
public class PrepareWakeState : MonoBehaviour, IPlayerState
{
    [Header("1往復なら4カウント")]
    [SerializeField] private int _requiredPasses = 4;
    private PlayerController _controller;
    private Collider2D _col;
    private bool _wasInside = false;
    private int _passCount = 0;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
        _col = GetComponent<Collider2D>();
    }

    public void Enter()
    {
        ResetCounter();

        if (_controller.IsWake)
        {
            _controller.ChangeState(PlayerState.Moving);
            Exit();
        }

        //Debug.Log("prepareWake Enter");
    }

    public void Tick()
    {
        //Debug.Log("prepareWake Tick");

        Vector2 pointerPos = _controller.PointerPos;
        bool isInside = _col.OverlapPoint(pointerPos);

        if (isInside != _wasInside)
        {
            _passCount++;
            _wasInside = isInside;
            Debug.Log("Pass Count: " + _passCount);

            if (_passCount >= _requiredPasses)
            {
                _controller.ChangeState(PlayerState.Moving);
                _controller.IsWake = true;
                Debug.Log("撫で成功！");
                ResetCounter();
            }
        }
    }

    private void ResetCounter()
    {
        _passCount = 0;
        _wasInside = false;
    }

    public void FixedTick()
    {
    }

    public void Exit()
    {
        ResetCounter();
        //Debug.Log("prepare wake Exit");
    }

    private void Release()
    {
        ResetCounter();
    }

    private void OnDisable()
    {
        //_controller.OnReleaseTrigger -= Release;
    }
}
