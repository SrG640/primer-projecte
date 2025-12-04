using Godot;

public partial class JumpingMovementState : State
{
    public PinkMan _player;

    public override void Ready()
    {
        _player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        
        _player.SetAnimation("Jump");

        Vector2 velocity = _player.Velocity;
        velocity.Y = _player.GetJumpSpeed();
		_player.Velocity = velocity;

    }

    public override void UpdatePhysics(double delta)
    {
        Vector2 velocity = _player.Velocity;
        
        if (!_player.IsOnFloor() && !_player.IsDead)
		{
			velocity += _player.GetGravity() * (float)delta;
		}

       float direction = Input.GetAxis("move_left", "move_right");
		if (direction != 0f)
		{
			velocity.X = direction * _player.GetSpeed();
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, _player.GetSpeed());
		}

        _player.Velocity = velocity;
        _player.MoveAndSlide();

        if (_player.Velocity.Y > 0)
            stateMachine.TransitionTo("FallingMovementState");

        if (_player.IsOnFloor())
        {
            if (_player.Velocity.X == 0)
                stateMachine.TransitionTo("IdleMovementState");
            else
                stateMachine.TransitionTo("RunningMovementState");
        }
    }

    public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed ("jump") && _player.GetDoubleJumpAvailable() && !_player.IsDead)
        stateMachine.TransitionTo("DoubleJumpMovementState");
    }
}
