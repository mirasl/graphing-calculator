using Godot;
using System;

public class UI : Control
{
    [Signal] delegate void Graph(string expression, float size, float p);
    [Signal] delegate void SetLineEditFocus(bool state);

    private bool lineEditFocused = false;

    string[] sampleEquations = new string[] {
        "sin(cos(x + t) + (y + t)/2)",
        "sin(x + t) - sin(y + t)",
        "x*x - y*y",
        "sin(cos(sin(cos(x+t) - y) - x) - t)",
        "1/(x*y)"
    };

    public override void _PhysicsProcess(float delta)
    {
        LineEdit le = GetNode<LineEdit>("Equation");
        if (le.HasFocus() != lineEditFocused)
        {
            lineEditFocused = le.HasFocus();
            EmitSignal("SetLineEditFocus", lineEditFocused);
        }
    }

    public void sig_GraphButtonPressed()
    {
        GetNode<Button>("Graph").ReleaseFocus();
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

    public void sig_RandomButtonPressed()
    {
        GetNode<Button>("Random").ReleaseFocus();
        GetNode<LineEdit>("Equation").Text = sampleEquations[(int)(GD.Randf() *
                sampleEquations.Length)];
        sig_GraphButtonPressed();
    }
}
