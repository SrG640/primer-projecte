using Godot;

public partial class IdleMovementState : State
{
    public PinkMan _player;

    public override void Ready()
    {
        _player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        
        _player.SetAnimation("Idle");

        _player.SetDoubleJumpAvailable(true);
    }


    public override void UpdatePhysics(double delta)
    {
        if (!_player.IsOnFloor())
        {
            if (_player.Velocity.Y < 0)
                stateMachine.TransitionTo("JumpingMovementState");
            else
                stateMachine.TransitionTo("FallingMovementState");
        }
    }
    public override void Update(double delta)
    {
        if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right"))
            stateMachine.TransitionTo("RunningMovementState");
    }

    public override void HandleInput(InputEvent @event)
    {
        
        if (@event.IsActionPressed("jump") && _player.IsOnFloor() && !_player.IsDead)
            stateMachine.TransitionTo("JumpingMovementState");
    }
}