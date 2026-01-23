using Godot;
using System;
using System.Threading.Tasks;

public partial class PinkMan : CharacterBody2D
{
	private const float Speed = 250.0f;
	private const float JumpVelocity = -500.0f;
	bool canDoubleJump = true;
	public bool IsDead = false;

	private AudioStreamPlayer2D deathSound;
	private AnimatedSprite2D sprite;
	[Signal]public delegate void JumpedEventHandler();

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
	 public override void _PhysicsProcess(double delta)
    {
        if (sprite == null) return;

        if (Input.IsActionJustPressed("ui_left"))
        {
            sprite.FlipH = true;
        }
        else if (Input.IsActionJustPressed("ui_right"))
        {
            sprite.FlipH = false;
        }
    }

	public void SetAnimation(string animationName)
    { 
		if (sprite == null) return;
		if (IsDead && animationName != "Die") return;
		GD.Print($"Playing: {animationName}");
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

	public bool GetDoubleJumpAvailable()
	{
		return canDoubleJump;
	}
	public void SetDoubleJumpAvailable(bool b)
	{
		canDoubleJump = b;
	}

	public async Task Dead()
    {
		IsDead = true;
		deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
		deathSound.Play();
        SetAnimation("Die");
        Velocity = Vector2.Zero;

		SetPhysicsProcess(false);
		SetProcessInput(false);
		
		await ToSignal(sprite,"animation_finished");
		GetTree().ReloadCurrentScene();
    }
}
