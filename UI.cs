using Godot;
using System;

public class UI : Control
{
    [Signal] delegate void Graph(string expression, float size, float p);
    [Signal] delegate void SetLineEditFocus(bool state);

    private bool lineEditFocused = false;
    private int lastRandom = 0;

    // List of equations which are randomly selected when "Random" button is
    // clicked
    string[] sampleEquations = new string[] {
        "sin(cos(x + t) + (y + t)/2)",
        "sin(x + t) - sin(y + t)",
        "x*x - y*y",
        "sin(cos(sin(cos(x+t) - y) - x) - t)",
        "1/(x*y)",
        "sin(x*x - y*y)",
        "sin(x*x/10 + y*y/10 + t)",
        "sin(x*x + y*y)",
        "((4/10)^2-((6/10)-(x^2+y^2)^(1/2))^2)^(1/2)",
        "sin((x+15)^(6/10) + t) - cos((y+15)^(6/10) + t)",
        "1/((x^2+y^2))",
        "(x^2+y^2)^(1/2)",
        "(x^2+y^2)^(1/2) * sin(t)",
        "1/(100*sin(x) + 100*cos(y))",
        "x*x*y*y",
        "cos(t)*x",
        "((2)^2-(5-(x^2+y^2)^(1/2))^2)^(1/2) + x*sin(t) + y*cos(t)"
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

        // Guarentees next random graph is different than the last:
        int random = (int)(GD.Randf() * sampleEquations.Length);
        while (random == lastRandom) 
        {
            random = (int)(GD.Randf() * sampleEquations.Length);
        }
        lastRandom = random;

        GetNode<LineEdit>("Equation").Text = sampleEquations[(int)(GD.Randf() *
                sampleEquations.Length)];
        sig_GraphButtonPressed();
    }

    public void sig_GuideButtonPressed()
    {
        GetNode<WindowDialog>("WindowDialog").Popup_();
    }
}
