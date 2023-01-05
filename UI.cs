using Godot;
using System;

public class UI : Control
{
    [Signal] delegate void Graph(string expression, float size, float p);

    public void sig_GraphButtonPressed()
    {
        if (GetNode<LineEdit>("Equation").Text == "")
        {
            return;
        }

        string size = GetNode<LineEdit>("Size").Text;
        if (size == "")
        {
            size = "15";
        }

        EmitSignal("Graph", 
                GetNode<LineEdit>("Equation").Text,
                float.Parse(size),
                (float)GetNode<HSlider>("HSlider").Value);
    }
}
