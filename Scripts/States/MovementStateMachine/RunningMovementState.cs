using System;
using Godot;

public partial class RunningMovementState : State
{
    public PinkMan _player;

    public override void Ready()
    {
        _player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        _player.SetAnimation("Run");

        _player.SetDoubleJumpAvailable(true);
    }
    public override void UpdatePhysics(double delta)
    {
        Vector2 velocity = _player.Velocity;
       float direction = Input.GetAxis("move_left", "move_right") ;
		if (direction != 0f && !_player.IsDead)
		{
			velocity.X = direction * _player.GetSpeed();
		}
		else 
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, _player.GetSpeed());
		}

		_player.Velocity = velocity;
		_player.MoveAndSlide();
        if (!_player.IsOnFloor() && !_player.IsDead)
        {
            if (_player.Velocity.Y < 0)
                stateMachine.TransitionTo("JumpingMovementState");
            else
                stateMachine.TransitionTo("FallingMovementState");
        }

        if (_player.Velocity.X == 0)
        {
            stateMachine.TransitionTo("IdleMovementState");
        }
    }

    public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("jump") && _player.IsOnFloor() && !_player.IsDead)
            stateMachine.TransitionTo("JumpingMovementState");
        if (@event.IsActionPressed ("jump") && !_player.IsOnFloor() && !_player.IsDead)
        stateMachine.TransitionTo("DoubleJumpMovementState");

    }
}