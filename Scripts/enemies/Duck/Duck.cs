using Godot;
using System;
using System.Threading.Tasks;
using System.Reflection;

public partial class Duck : RigidBody2D
{
    public Duck duck;
    private const float Speed = 250.0f;
    private const float JumpVelocity = -500.0f;
    private const float BounceHorizontal = 200.0f;
    private const float BounceVertical = -350.0f;

    private AnimatedSprite2D sprite;
    public bool IsDead = false;
    
    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        SetAnimation("Idle");
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

            float dir = 0f;
            if (player is CharacterBody2D cb)
            {
                dir = Mathf.Sign(cb.Velocity.X);
            }
            else
            {
                try
                {
                    var prop = player.GetType().GetProperty("Velocity", BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null)
                    {
                        var vel = prop.GetValue(player);
                        if (vel is Vector2 v) dir = Mathf.Sign(v.X);
                    }
                    else
                    {
                        dir = Mathf.Sign(player.GlobalPosition.X - GlobalPosition.X);
                    }
                }
                catch
                {
                    dir = Mathf.Sign(player.GlobalPosition.X - GlobalPosition.X);
                }
            }

            float horizVel = (dir == 0f) ? 0f : dir * BounceHorizontal;
            var bounceVel = new Vector2(horizVel, BounceVertical);

            try
            {
                var method = player.GetType().GetMethod("Bounce", BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(player, new object[] { bounceVel });
                }
                else if (player is CharacterBody2D cb2)
                {
                    cb2.Velocity = bounceVel;
                }
                else if (player is PinkMan rb)
                {
                    rb.Velocity = bounceVel;
                }
                else
                {
                    var propVel = player.GetType().GetProperty("Velocity", BindingFlags.Public | BindingFlags.Instance);
                    if (propVel != null && propVel.PropertyType == typeof(Vector2))
                        propVel.SetValue(player, bounceVel);
                    else
                    {
                        var propLin = player.GetType().GetProperty("LinearVelocity", BindingFlags.Public | BindingFlags.Instance);
                        if (propLin != null && propLin.PropertyType == typeof(Vector2))
                            propLin.SetValue(player, bounceVel);
                    }
                }
            }
            catch
            {

            }

            SetPhysicsProcess(false);
            SetProcessInput(false);
        
            await ToSignal(sprite,"animation_finished");
            CallDeferred("queue_free");
        }
    }
}
