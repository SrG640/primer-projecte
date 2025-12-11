using Godot;
using System;
using System.Collections.Generic;

public partial class Finish : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // { changed code }
    public void _on_body_entered(Node2D body)
    {
        if (!(body is PinkMan))
            return;

        // Recolectar todos los Ducks en la escena
        var ducks = new List<Duck>();
        CollectDucks(GetTree().Root, ducks);

        // Si hay algún Duck vivo => no pasar de nivel
        foreach (var d in ducks)
        {
            if (d != null && !d.IsDead)
            {
                GD.Print("Duck not dead yet — cannot change level.");
                return;
            }
        }

        // Si no quedan ducks vivos -> avanzar
        GD.Print("Level Completed!");
        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }

    // Busca recursivamente el primer nodo Duck en la escena y lo devuelve (o null)
    private Duck FindAnyDuck()
    {
        return FindDuckInNode(GetTree().Root);
    }

    private Duck FindDuckInNode(Node node)
    {
        var children = node.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (!(children[i] is Node childNode)) 
                continue;

            if (childNode is Duck foundDuck)
                return foundDuck;

            var result = FindDuckInNode(childNode);
            if (result != null)
                return result;
        }
        return null;
    }

    private void CollectDucks(Node node, List<Duck> ducks)
    {
        var children = node.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (!(children[i] is Node childNode))
                continue;

            if (childNode is Duck foundDuck)
                ducks.Add(foundDuck);

            CollectDucks(childNode, ducks);
        }
    }
}

