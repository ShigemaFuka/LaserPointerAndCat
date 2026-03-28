public interface IPlayerState
{
    void Initialize(PlayerController controller);
    void Enter();
    void Tick();
    void FixedTick();
    void Exit();
}