using Godot;
using System;
using System.Threading.Tasks;

public partial class Saw : Area2D
{
    public float RotationSpeed = 360.0f;
    public override void _Process(double delta)

    {
        float radPerSec = RotationSpeed * (float)(Math.PI / 180.0);
        Rotation += radPerSec * (float)delta;
    }

    private async void _on_body_entered(Node2D body)
    {
        if (body is PinkMan player)
        {
            GD.Print("Player hit a Saw");
            await player.Dead();
        }
    }
}
