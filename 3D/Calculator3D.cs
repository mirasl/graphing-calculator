using Godot;
using System;

/// <summary>
/// Controls top-level communication between UI and backend graph functionality.
/// </summary>
public class Calculator3D : Spatial
{
	/// <summary>
	/// Whether or not the current graph is time-dependent, i.e. if it contains
	/// the "t" variable 
	/// </summary>
	private bool animated = false;

	/// <summary>
	/// Graph node which displays the graphed equation
	/// </summary>
	private Graph g;

	/// <summary>
	/// Concentration of graphed points within the given size variable.
	/// Represented on a scale from 0 to 1.
	/// </summary>
	private float precisionPercent;

	/// <summary>
	/// Area of the xy-plane taken up by the graphed points.
	/// </summary>
	private float size;

	public override void _Ready()
	{
		g = GetNode<Graph>("Graph");
	}

	/// <summary>
	/// Passes variables into Graph.cs to graph the equation
	/// </summary>
	/// <param name="expression">The equation to be interpreted</param>
	/// <param name="s">The size of the area graphed</param>
	/// <param name="p">Precision (concentration of points graphed) on a scale 
	/// 		from 0 to 1</param>
	public void sig_Graph(string expression, float s = 15, float p = 0.66f)
	{
		g.ClearGraph();
		g.e = g.interpret(expression);
		string otherExpression = expression + "";
		while (otherExpression.Find("tan") != -1)
		{
			otherExpression = otherExpression.Remove(
					otherExpression.Find("tan"), 3);
		}
		if (otherExpression.Find("t") != -1)
		{
			animated = true;
			precisionPercent = p;
			size = s;
		}
		else
		{
			g.DrawGraph(new Vector2(s, s), p);
		}
	}

	/// <summary>
	/// Locks or unlocks camera movement based on line edit focus state
	/// </summary>
	/// <param name="focused">LineEdit focus state</param>
	public void sig_SetLineEditFocus(bool focused)
	{
		GetNode<CameraPivot>("CameraPivot").Disabled = focused;
	}

	/// <summary>
	/// If graph contains animated features, continuously updates the graph
	/// each frame to respond to the changing time value.
	/// </summary>
	/// <param name="delta">Time (s) between frames</param>
	public override void _Process(float delta)
	{
		if (animated)
		{
			g.DrawAnimatedGraph(new Vector2(size, size), precisionPercent);
		}
	}

	public void sig_CreateErrorMessage()
	{
        GetNode<WindowDialog>("ErrorWindow").Popup_();
	}
}
