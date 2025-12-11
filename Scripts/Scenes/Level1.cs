using Godot;
using System;

public partial class Level1 : Node
{
	public void _on_finish_body_entered(Node2D body)
	{
		if (body is PinkMan player)
		{
			GD.Print("Level Finished!");
			GetTree().ChangeSceneToFile("res://Scenes/Levels/Level2.tscn");
		}
	}
}
