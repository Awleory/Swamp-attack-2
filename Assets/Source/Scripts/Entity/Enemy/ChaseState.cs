using UnityEngine;

public class ChaseState : State
{
    private Enemy _enemy;

    public ChaseState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void Exit()
    {
        base.Exit();

        _enemy.MovementPhysics.SetDirectionMove(Vector2.zero);
    }

    public override void LogicUpdate()
    {
        Transform targetTransform = _enemy.Target.transform;
        Vector2 move = new Vector2(Mathf.Sign(targetTransform.position.x - _enemy.transform.position.x), 0);
        _enemy.MovementPhysics.SetDirectionMove(move);

        if (move.x != 0)
        {
            _enemy.AnimationController.SetFlipX(move.x < 0);
        }
    }
}
