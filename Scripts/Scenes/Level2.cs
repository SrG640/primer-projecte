using Godot;
using System;

public partial class Level2 : Node
{
	public void _on_finish_body_entered(Node2D body)
	{
		if (body is PinkMan player)
		{
			GD.Print("Game Finished!");
			GetTree().ChangeSceneToFile("res://Scenes/Levels/MainMenu.tscn");
		}
	}
}
