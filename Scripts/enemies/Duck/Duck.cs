using Godot;
using System;
using System.Threading.Tasks;

public partial class Duck : RigidBody2D
{
	public Duck duck;
	private const float Speed = 250.0f;
	private const float JumpVelocity = -500.0f;
	private AnimatedSprite2D sprite;
	private bool IsDead = false;
    
	public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        duck.SetAnimation("Idle");
    }
    public override void _PhysicsProcess(double delta)
    {
        if (LinearVelocity == Vector2.Zero)
        {
            SetAnimation("Idle");
        }
    }

	public void SetAnimation(string animationName)
    {
		sprite.Play(animationName);
    }

	public float GetSpeed()
    {
        return Speed;
    }

	public float GetJumpSpeed()
    {
        return JumpVelocity;
    }
	    private async void _on_damage_body_entered(Node2D body)
    {
        if (body is PinkMan player)
        {
            if (!player.IsDead)
			{
				GD.Print("Duck killed the Player");
            	await player.Dead();
			}
        }
    }
		private void _on_vision_body_entered(Node2D body)
    {
        if (body is PinkMan player)
        {
            GD.Print("Duck saw the Player");
            player.Jumped += _on_player_jumped;
        }
    }

    private void _on_player_jumped()
    {
        LinearVelocity = new Vector2(0, GetJumpSpeed());
        SetAnimation("Jump");
    }

    private void _on_vision_body_exited(Node2D body)
    {
        if (body is PinkMan player)
        {
            GD.Print("Duck lost sight of the Player");
            player.Jumped -= _on_player_jumped;
        }
    }

    	private async void _on_kill_body_entered(Node2D body)
    {
        if (body is PinkMan player)
        {
			IsDead = true;
            GD.Print("Duck was killed by Player");
			SetAnimation("Die");

		SetPhysicsProcess(false);
		SetProcessInput(false);
		
		await ToSignal(sprite,"animation_finished");
		QueueFree();
        }
    }
}
