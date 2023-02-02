using Godot;
using System;

/// <summary>
/// Handles inputs given by the player, sending the user input data as signals
/// to then be interpreted by the calculator's mathematical backend interpreter.
/// </summary>
public class UI : Control
{
	[Signal] delegate void Graph(string expression, float size, float p);
	[Signal] delegate void SetLineEditFocus(bool state);

	/// <summary>
	/// The current focus state of the Equation lineEdit.
	/// </summary>
	private bool lineEditFocused = false;

	/// <summary>
	/// The last random sampleEquation index generated (used to ensure the
	/// random button always generates a new response).
	/// </summary>
	private int lastRandom = 0;

	/// <summary>
	/// List of sample equations, one of which is randomly selected when the 
	/// "random" button is pressed.
	/// </summary>
	/// <value>The list of the sample equations.</value>
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
		"((2)^2-(5-(x^2+y^2)^(1/2))^2)^(1/2) + x*sin(t) + y*cos(t)",
		"(x^2+y^2)^(-1/2) * sin(t)",
		"ln(x-sint)*ln(y+sint)",
		"tant+sinx+siny",
        "1/cos(x/3) - 1/cos(y/3)",
        "1/sin(x/3) + 1/sin(y/3)",
        "1/cos(x*sin(t/3)/3) + 1/cos(y*sin(t/3)/3)",
        "sin(cos(sin(cos(x+t) - y) - x) - t) *cos(t)",
        "(sin(x+t) - cos(y+t)) / (cos(t)/sin(t))",
        "sin(cos(x + t) + (y + t)/2) / (sin(cos(x + t) + (y + t)/2) - (sin(cos(x + t) + (y + t)/2)^2)^(1/2))",
        "(((0.5)^2-(5-(x^2+y^2)^(1/2))^2)^(1/2) + x + 1/((x+3.5)^2)) / ((((0.5)^2-(5-(x^2+y^2)^(1/2))^2)^(1/2) + x + 1/((x+3.5)^2)) + ((((0.5)^2-(5-(x^2+y^2)^(1/2))^2)^(1/2) + x + 1/((x+3.5)^2))^0.5)^2)", // ridiculously complicated smiley face :)
        "(1/sin(x) - 1/sin(y)) / ((1/sin(x) - 1/sin(y)) + ((1/sin(x) - 1/sin(y))^2)^0.5)"
	};

	/// <summary>
	/// Handles current focus state of the Equation lineEdit each frame.
	/// </summary>
	/// <param name="delta">Time (s) between frames.</param>
	public override void _Process(float delta)
	{
		LineEdit le = GetNode<LineEdit>("Equation");
		if (le.HasFocus() != lineEditFocused)
		{
			lineEditFocused = le.HasFocus();
			EmitSignal("SetLineEditFocus", lineEditFocused);
		}
	}

	/// <summary>
	/// Called when the "Graph" button is pressed; interprets current user-
	/// inputted data and sends the "Graph" signal to be mathematically
	/// interpreted by the "backend."
	/// </summary>
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

	/// <summary>
	/// Passes a random equation from the sampleEquations list into the 
	/// equation lineEdit, then simulates a "Graph" button press to immediately
	/// graph the randomly generated equation.
	/// </summary>
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
		
		GD.Randomize();
		GetNode<LineEdit>("Equation").Text = sampleEquations[(int)(GD.Randf() *
				sampleEquations.Length)];
		sig_GraphButtonPressed();
	}

	/// <summary>
	/// Shows the Guide popup.
	/// </summary>
	public void sig_GuideButtonPressed()
	{
		GetNode<WindowDialog>("WindowDialog").Popup_();
	}
}
