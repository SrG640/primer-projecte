using Godot;

public partial class FallingMovementState : State
{
    public PinkMan _player;

    public override void Ready()
    {
        _player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        
        _player.SetAnimation("Fall");
    }

      public override void UpdatePhysics(double delta)
    {
       Vector2 velocity = _player.Velocity;

		if (!_player.IsOnFloor() && !_player.IsDead)
		{
			velocity += _player.GetGravity() * (float)delta;
		}
		_player.Velocity = velocity;
		_player.MoveAndSlide();

        if (_player.IsOnFloor())
        {
            if (_player.Velocity.Y == 0)
                stateMachine.TransitionTo("IdleMovementState");

            if (_player.Velocity.X != 0)
            
                stateMachine.TransitionTo("IdleMovementState");
        }
    }
        public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("move_left") || @event.IsActionPressed("move_right") && !_player.IsDead)
            stateMachine.TransitionTo("RunningMovementState");
        if (@event.IsActionPressed ("jump") && _player.GetDoubleJumpAvailable() && !_player.IsDead)
        stateMachine.TransitionTo("DoubleJumpMovementState");
    }
}
