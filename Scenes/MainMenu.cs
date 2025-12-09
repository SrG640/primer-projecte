using Godot;
using System;

public partial class MainMenu : Node
{
		public void _on_button_pressed()
		{
			GD.Print("Start Button Pressed");
			GetTree().ChangeSceneToFile("res://Scenes/Level1.tscn");
		}

		public void _on_button_2_pressed()
		{
			GD.Print("Quit Button Pressed");
			GetTree().Quit();
		}
}
