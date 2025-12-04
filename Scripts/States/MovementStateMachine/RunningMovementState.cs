using System;
using Godot;

public partial class RunningMovementState : State
{
    public PinkMan player;

    public override void Ready()
    {
        player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        player.SetAnimation("Run");

        player.SetDoubleJumpAvailable(true);
    }
    public override void UpdatePhysics(double delta)
    {
        Vector2 velocity = player.Velocity;
       float direction = Input.GetAxis("move_left", "move_right") ;
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
        if (!player.IsOnFloor() && !player.IsDead)
        {
            if (player.Velocity.Y < 0)
                stateMachine.TransitionTo("JumpingMovementState");
            else
                stateMachine.TransitionTo("FallingMovementState");
        }

        if (player.Velocity.X == 0)
        {
            stateMachine.TransitionTo("IdleMovementState");
        }
    }

    public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("jump") && player.IsOnFloor() && !player.IsDead)
            stateMachine.TransitionTo("JumpingMovementState");
        if (@event.IsActionPressed ("jump") && !player.IsOnFloor() && !player.IsDead)
        stateMachine.TransitionTo("DoubleJumpMovementState");

    }
}