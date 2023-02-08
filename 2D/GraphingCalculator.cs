using Godot;
using System;

public class GraphingCalculator : Node2D
{
	public void sig_InputGraph(string input)
	{
		GD.Print(input);
		//Calculator2D calc = GetNode<Calculator2D>("Lines");
		//Element function = calc.interpret(input);
		//calc.Graph(function);
	}
}
