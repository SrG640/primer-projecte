using Godot;

public partial class FallingMovementState : State
{
    public PinkMan player;

    public override void Ready()
    {
        player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        
        player.SetAnimation("Fall");
    }

      public override void UpdatePhysics(double delta)
    {
       Vector2 velocity = player.Velocity;

		if (!player.IsOnFloor() && !player.IsDead)
		{
			velocity += player.GetGravity() * (float)delta;
		}
		player.Velocity = velocity;
		player.MoveAndSlide();

        if (player.IsOnFloor())
        {
            if (player.Velocity.Y == 0)
                stateMachine.TransitionTo("IdleMovementState");

            if (player.Velocity.X != 0)
            
                stateMachine.TransitionTo("IdleMovementState");
        }
    }
        public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("move_left") || @event.IsActionPressed("move_right") && !player.IsDead)
            stateMachine.TransitionTo("RunningMovementState");
        if (@event.IsActionPressed ("jump") && player.GetDoubleJumpAvailable() && !player.IsDead)
        stateMachine.TransitionTo("DoubleJumpMovementState");
    }
}
