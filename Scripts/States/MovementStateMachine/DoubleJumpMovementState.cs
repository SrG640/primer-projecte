using System.Reflection.Metadata;
using System.Runtime.Serialization.Formatters;
using Godot;

public partial class DoubleJumpMovementState : State
{
    public PinkMan player;

    public override void Ready()
    {
        player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        player.SetDoubleJumpAvailable(false);

        player.SetAnimation("DoubleJump");

        Vector2 velocity = player.Velocity;
        velocity.Y = player.GetJumpSpeed();
		player.Velocity = velocity;

    }

    public override void UpdatePhysics(double delta)
    {
        Vector2 velocity = player.Velocity;
        
        if (!player.IsOnFloor() && !player.IsDead)
		{
			velocity += player.GetGravity() * (float)delta;
		}

       float direction = Input.GetAxis("move_left", "move_right");
		if (direction != 0f && !player.IsDead)
		{
			velocity.X = direction * player.GetSpeed();
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, player.GetSpeed());
		}

        player.Velocity = velocity;
        player.MoveAndSlide();

        if (player.Velocity.Y > 0 && !player.IsDead)
            stateMachine.TransitionTo("FallingMovementState");
    }
}
