using Godot;

public partial class JumpingMovementState : State
{
    public PinkMan player;

    public override void Ready()
    {
        player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        player.EmitSignal("Jumped");
        player.SetAnimation("Jump");

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
		if (direction != 0f)
		{
			velocity.X = direction * player.GetSpeed();
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, player.GetSpeed());
		}

        player.Velocity = velocity;
        player.MoveAndSlide();

        if (player.Velocity.Y > 0)
            stateMachine.TransitionTo("FallingMovementState");

        if (player.IsOnFloor())
        {
            if (player.Velocity.X == 0)
                stateMachine.TransitionTo("IdleMovementState");
            else
                stateMachine.TransitionTo("RunningMovementState");
        }
    }

    public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed ("jump") && player.GetDoubleJumpAvailable() && !player.IsDead)
        stateMachine.TransitionTo("DoubleJumpMovementState");
    }
}
