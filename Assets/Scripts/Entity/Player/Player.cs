using UnityEngine;

public class Player : Entity
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _shootPoint;

    public int Money { get; private set; }

    protected override void Update()
    {
        base.Update();
        ProcessInput();
    }

    public void OnEnemyDied(Enemy enemy)
    {
        Money += enemy.Reward;
    }

    private void ProcessInput()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), 0);
        MovementPhysics.SetDirectionMove(move);

        if (move.x != 0)
        {
            AnimationController.SetFlipX(move.x < 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _weapon.Shoot(_shootPoint);
            AnimationController.PlayAttack();
        }
    }
}
