public abstract class EnemyState    //imma rework this to be scriptable objects instead at some point.
{
    public abstract EnemyManager enemy { get; protected set; }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
