public class DeathState : State
{
    public override void Enter()
    {
        base.Enter();
        CanExit = false;
    }
}