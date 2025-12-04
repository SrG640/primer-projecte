using Godot;

public partial class IdleMovementState : State
{
    public PinkMan player;

    public override void Ready()
    {
        player = (PinkMan)GetTree().GetFirstNodeInGroup("PinkManGroup");
    }
    public override void Enter()
    {
        
        player.SetAnimation("Idle");

        player.SetDoubleJumpAvailable(true);
    }


    public override void UpdatePhysics(double delta)
    {
        if (!player.IsOnFloor()&& !player.IsDead)
        {
            if (player.Velocity.Y < 0)
                stateMachine.TransitionTo("JumpingMovementState");
            else
                stateMachine.TransitionTo("FallingMovementState");
        }
    }
    public override void Update(double delta)
    {
        if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right") && !player.IsDead)
            stateMachine.TransitionTo("RunningMovementState");
    }

    public override void HandleInput(InputEvent @event)
    {
        
        if (@event.IsActionPressed("jump") && player.IsOnFloor() && !player.IsDead)
            stateMachine.TransitionTo("JumpingMovementState");
    }
}