public abstract class State
{
    protected IMovable context;

    public State(IMovable context)
    {
        this.context = context;
    }

    public virtual bool CanTransitionToSelf()
    {
        return false;
    }

    public abstract void Tick();
    public abstract void OnEnter();
    public abstract void OnExit();
}