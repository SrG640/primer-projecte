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
    }
	public void Enter()
    {
        duck.SetAnimation("Idle");
    }

	public void SetAnimation(string animationName)
    { 
		GD.Print($"Duck Playing: {animationName}");
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
		private async void _on_vision_body_entered(Node2D body)
    {
        if (body is PinkMan player)
        {
            GD.Print("Duck saw the Player");
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
