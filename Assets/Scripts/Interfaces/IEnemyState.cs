public interface IEnemyState
{
    void EnterState(CommonEnemyBehaviour enemy);
    void UpdateState(CommonEnemyBehaviour enemy);
    void ExitState(CommonEnemyBehaviour enemy);
}
