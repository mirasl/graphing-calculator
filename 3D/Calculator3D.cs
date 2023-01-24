using Godot;
using System;

public class Calculator3D : Spatial
{
	bool animated = false;
	Graph g;

	float precisionPercent;
	float size;

	public void sig_GetCameraPosition()
	{
		GetNode<Gridlines>("Gridlines").GlobalCameraOrigin = 
				GetNode<Camera>("CameraPivot/Camera").GlobalTransform.origin;
	}

	public override void _Ready()
	{
		g = GetNode<Graph>("Graph");
		//Graph("0", 10, 0.5f);
	}

	public void sig_Graph(string expression, float s = 15, float p = 0.66f) // sig_Graph() fix this in editor
	{
		g.ClearGraph();
		g.e = g.interpret(expression);
		if (expression.Find("t") != -1)
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

	public void sig_SetLineEditFocus(bool focused)
	{
		GetNode<CameraPivot>("CameraPivot").Disabled = focused;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (animated)
		{
			g.DrawAnimatedGraph(new Vector2(size, size), precisionPercent);
		}
	}
}
