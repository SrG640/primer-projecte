using Godot;
using System;

public partial class Finish : Area2D
{
	public void _on_body_entered (Node2D body)
	{
		if (body is PinkMan player && player.SceneFilePath == "res://Scenes/Level1.tscn")
        {
            GD.Print("Level Finished!");
			GetTree().ChangeSceneToFile("res://Scenes/Level2.tscn");

        }
		else
		{
			GD.Print("Game Finished!");
			GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
		}
	}
}
