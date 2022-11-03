using Godot;
using System;
using Godot.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Calculator2D : Node2D
{
	// The dimensions of the part of the screen currently visible to the camera:
	public Rect2 Screen {get; private set;}

	private PackedScene pLine;
	
	Element e;


	public override void _Ready()
	{
		pLine = GD.Load<PackedScene>("res://Line2D.tscn");

		e = interpret("x^(2-1)+1*10/5");
		
		Graph();
		
	}

	// Sets the line to reflect a graph of the function:
	public void Graph()
	{
		bool inRange = true;

		// The graph needs to be broken up into 100 seperate lines because one 
		// line should not have over 1000 verticies.
		for (int i = -200; i < 200; i++) 
		{
			Line2D line = pLine.Instance<Line2D>();
			AddChild(line);

			for (int xInt = i * 1000; xInt <= (i+1) * 1000; xInt++)
			{
				float x = xInt / 400f;//(Screen.Size.x / 50f);\
				//GD.Print(x);
				x += Screen.Position.x;

				// if (inRange)
				// {
				//     if (f(x) > 150 + Screen.Position.y) // too high
				//     {
				//         inRange = false;
				//         line.AddPoint(new Vector2(x - 0.005f, 100000000));
				//     }
				//     else if (f(x) < -150 + Screen.Position.y) // too low
				//     {
				//         inRange = false;
				//         line.AddPoint(new Vector2(x - 0.005f, -100000000));
				//     }
				// }
				if ((f(x) > 150 + Screen.Position.y || f(x) < -150 + Screen.Position.y) && inRange) // outside range
				{
					inRange = false;
					if (f(x) > 150 + Screen.Position.y) // too high
					{
						inRange = false;
						line.AddPoint(new Vector2(x, 10000000));
					}
					else if (f(x) < -150 + Screen.Position.y) // too low
					{
						inRange = false;
						line.AddPoint(new Vector2(x, -10000000));
					}
				}

				if ((f(x) <= 150 + Screen.Position.y && f(x) >= -150 + Screen.Position.y) && !inRange)
				{
					inRange = true;
					line = pLine.Instance<Line2D>();
					AddChild(line);

					if (TakeDerivative(x) > 0) // slope is positive
					{
						line.AddPoint(new Vector2(x, 10000000));
					}
					else if (TakeDerivative(x) < 0) // slope is negative
					{
						line.AddPoint(new Vector2(x, -10000000));
					}
					//if (f(x - 0.005f))

					//line.AddPoint(new Vector2(x, -10000000000));
				}

				if (inRange)
				{
					line.AddPoint(new Vector2(x, f(x)));
				}
			}
		}
	}

	public float TakeDerivative(float x)
	{
		float rise = f(x) - f(x + 0.00001f);
		float run = 0.00001f;
		return rise / run;
	}

	// Sets the Screen variable to reflect what the camera is currently viewing:
	public void RefreshScreen()
	{
		Camera2D camera = GetNode<Camera2D>("Camera2D");
		Screen = new Rect2(camera.Position, camera.Zoom * 
				GetViewportRect().Size / Scale);
	}

	public override void _PhysicsProcess(float delta)
	{
		///
		if (Input.IsActionJustPressed("ui_accept"))
		{
			RefreshScreen();
			Graph();
			//GD.Print(Screen);
		}
	}
	
	public float f(float x) // x^2
	{
		return -e.run(x);
	}
	
	public Element interpret(String input) {
		List<String> strList = seperate(input);
		List<Element> flatElements = new List<Element>();
		float floatS = 0;
		foreach (String s in strList) {
			if (float.TryParse(s, out floatS)) {
				flatElements.Add(new Value("value", float.Parse(s)));
			} else if (s.Equals("+")) {
				flatElements.Add(new Add("add", false));
			} else if (s.Equals("-")) {
				flatElements.Add(new Add("subtract", true));
			} else if (s.Equals("*")) {
				flatElements.Add(new Multiply("multiply", false));
			} else if (s.Equals("/")) {
				flatElements.Add(new Multiply("divide", true));
			} else if (s.Equals("^")) {
				flatElements.Add(new Exponent("exponent", false));
			} else if (s.Equals("x")) {
				flatElements.Add(new Variable("variable"));
			} else if (s.Equals("(")) {
				flatElements.Add(new Paren("open"));
			} else if (s.Equals(")")) {
				flatElements.Add(new Paren("close"));
			}
		}
		Element tree = Pemdas(flatElements);
		return tree;

	}
	
	public List<String> seperate(String input) {
		//GD.Print(input);
		input = Regex.Replace(input, @"\s+", "");
		//GD.Print(input);
		String type = "";
		if (Char.IsDigit(input, 0)) {
			type = "number";
		} else if (input[0] == 'x') {
			type = "variable";
		} else {
			type = "operator";
		}
		List<String> output = new List<String>();
		String current = "";
		for (int i = 0; i < input.Length; i++) {
			if (Char.IsDigit(input, i)) {
				if (type.Equals("number")) {
					current += input[i];
				} else {
					output.Add(current);
					current = input[i].ToString();
					type = "number";
				}
			} else if(input[i] == 'x') {
				output.Add(current);
				current = input[i].ToString();
				type = "variable";
			} else {
				output.Add(current);
				current = input[i].ToString();
				type = "operator";
			}
		}
		output.Add(current);
		//foreach (String s in output) {
			//GD.Print(s);
		//}
		return output;
	}
	
	public Element Pemdas(List<Element> input) {
		for (int i = 0; i < input.Count; i++) {
			Element e = input[i];
			if (e is Paren) {
				Paren castE = (Paren) e;
				if (e.type.Equals("open")) {
					int parenCount = 1;
					int j;
					for (j = i+1; parenCount > 0 && j < input.Count; j++) {
						Element f = input[j];
						if (f is Paren) {
							if (f.type.Equals("open")) {
								parenCount++;
							} else {
								parenCount--;
							}
						}
					}
					Element g = Pemdas(input.GetRange(i+1, j-i-2));
					input.RemoveRange(i, j-i);
					input.Insert(i, g);
					i--;
				}
			}
		}
		for (int i = 0; i < input.Count; i++) {
			Element e = input[i];
			if (e is Exponent) {
				Operator castE = (Operator) e;
				castE.setY(input[i+1]);
				input.RemoveAt(i+1);
				castE.setX(input[i-1]);
				input.RemoveAt(i-1);
				i--;
			}
		}
		for (int i = 0; i < input.Count; i++) {
			Element e = input[i];
			if (e is Multiply) {
				Operator castE = (Operator) e;
				castE.setY(input[i+1]);
				input.RemoveAt(i+1);
				castE.setX(input[i-1]);
				input.RemoveAt(i-1);
				i--;
			}
		}
		for (int i = 0; i < input.Count; i++) {
			Element e = input[i];
			if (e is Add) {
				Operator castE = (Operator) e;
				castE.setY(input[i+1]);
				input.RemoveAt(i+1);
				castE.setX(input[i-1]);
				input.RemoveAt(i-1);
				i--;
			}
		}
		if (input.Count > 1) {
			throw new Exception("PEMDAS failed, check for extra operators.");
		}
		//GD.Print(input[0].type);
		return input[0];
	}
}

public abstract class Element {
	public String type;
	
	public Element(String typeIn) {
		type = typeIn;
	}
	
	abstract public float run(float x);
}

public class Value : Element {
	public float a;
	
	
	public Value(String typeIn, float aIn) : base(typeIn) {
		a = aIn;
	}
	
	override public float run(float x) {
		return a;
	}
}

public class Variable : Element {
	public Variable(String typeIn) : base(typeIn) {
	}
	
	override public float run(float x) {
		return x;
	}
}

public class Paren : Element {
	public Element a;
	
	public Paren(String typeIn) : base(typeIn) {
	}
	
	override public float run(float x) {
		return a.run(x);
	}
}

abstract public class Operator : Element {
	public Element a;
	public Element b;
	
	public Operator(String typeIn) : base(typeIn) {
	}
	
	public void setX(Element aIn) {
		a = aIn;
	}	
	
	public void setY(Element bIn) {
		b = bIn;
	}
}

public class Add : Operator {
	bool inverse;
	
	public Add(String typeIn, bool inverseIn) : base(typeIn) {
		inverse = inverseIn;
	}
	
	override public float run(float x) {
		if (!inverse) {
			return a.run(x)+b.run(x);
		} else {
			return a.run(x)-b.run(x);
		}
		
	}
}

public class Multiply : Operator {
	bool inverse;
	
	public Multiply(String typeIn, bool inverseIn) : base(typeIn){
		inverse = inverseIn;
	}
	
	override public float run(float x) {
		if (!inverse) {
			return a.run(x)*b.run(x);
		} else {
			return a.run(x)/b.run(x);
		}
	}
}

public class Exponent : Operator {
	bool inverse;
	
	public Exponent(String typeIn, bool inverseIn) : base(typeIn) {
		inverse = inverseIn;
	}
	
	override public float run(float x) {
		if (!inverse) {
			return Mathf.Pow(a.run(x), b.run(x));
		} else {
			return Mathf.Pow(a.run(x), 1/b.run(x));
		}
		
	}
}
