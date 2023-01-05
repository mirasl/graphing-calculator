using Godot;
using System;

public class UI : Control
{
    [Signal] delegate void Graph(string expression, float size, float pPercent);

    public void sig_GraphButtonPressed()
    {
        EmitSignal("Graph", 
                GetNode<LineEdit>("Equation").Text,
                float.Parse(GetNode<LineEdit>("Size").Text),
                (float)GetNode<HSlider>("HSlider").Value);
    }
}
