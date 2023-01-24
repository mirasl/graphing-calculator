//using Godot;
//using System;
//using Godot.Collections;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//
//public class Calculator2D : Node2D
//{
//	// The dimensions of the part of the screen currently visible to the camera:
//	public Rect2 Screen {get; private set;}
//
//	private PackedScene pLine;
//
//	public Element e; // KEEP this change (necessary for text input)
//
//
//	public override void _Ready()
//	{
//		pLine = GD.Load<PackedScene>("res://Line2D.tscn");
//
//		// e = interpret("x^(2-1)");
//
//		// Graph(e);
//	}
//
//	// Sets the line to reflect a graph of the function:
//	public void Graph(Element function)
//	{
//		bool inRange = true;
//
//		// The graph needs to be broken up into 100 seperate lines because one 
//		// line should not have over 1000 verticies.
//		for (int i = -200; i < 200; i++) 
//		{
//			Line2D line = pLine.Instance<Line2D>();
//			AddChild(line);
//
//			for (int xInt = i * 1000; xInt <= (i+1) * 1000; xInt++)
//			{
//				float x = xInt / 400f;//(Screen.Size.x / 50f);\
//				//GD.Print(x);
//				x += Screen.Position.x;
//
//				// if (inRange)
//				// {
//				//     if (f(x) > 150 + Screen.Position.y) // too high
//				//     {
//				//         inRange = false;
//				//         line.AddPoint(new Vector2(x - 0.005f, 100000000));
//				//     }
//				//     else if (f(x) < -150 + Screen.Position.y) // too low
//				//     {
//				//         inRange = false;
//				//         line.AddPoint(new Vector2(x - 0.005f, -100000000));
//				//     }
//				// }
//				if ((-function.run(x) > 150 + Screen.Position.y || -function.run(x) < -150 + Screen.Position.y) && inRange) // outside range
//				{
//					inRange = false;
//					if (-function.run(x) > 150 + Screen.Position.y) // too high
//					{
//						inRange = false;
//						line.AddPoint(new Vector2(x, 10000000));
//					}
//					else if (-function.run(x) < -150 + Screen.Position.y) // too low
//					{
//						inRange = false;
//						line.AddPoint(new Vector2(x, -10000000));
//					}
//				}
//
//				if ((-function.run(x) <= 150 + Screen.Position.y && -function.run(x) >= -150 + Screen.Position.y) && !inRange)
//				{
//					inRange = true;
//					line = pLine.Instance<Line2D>();
//					AddChild(line);
//
//					if (TakeDerivative(x, function) > 0) // slope is positive
//					{
//						line.AddPoint(new Vector2(x, 10000000));
//					}
//					else if (TakeDerivative(x, function) < 0) // slope is negative
//					{
//						line.AddPoint(new Vector2(x, -10000000));
//					}
//					//if (f(x - 0.005f))
//
//					//line.AddPoint(new Vector2(x, -10000000000));
//				}
//
//				if (inRange)
//				{
//					line.AddPoint(new Vector2(x, -function.run(x)));
//				}
//			}
//		}
//
//		RefreshScreen();
//	}
//
//	public float TakeDerivative(float x, Element function)
//	{
//		float rise = -function.run(x) + function.run(x + 0.00001f);
//		float run = 0.00001f;
//		return rise / run;
//	}
//
//	// Sets the Screen variable to reflect what the camera is currently viewing:
//	public void RefreshScreen()
//	{
//		Camera2D camera = GetNode<Camera2D>("Camera2D");
//		Screen = new Rect2(camera.Position, camera.Zoom * 
//				GetViewportRect().Size / Scale);
//		sig_ChangeZoom(camera.Zoom);
//	}
//
//	public void sig_ChangeZoom(Vector2 zoom)
//	{
//		foreach (Node child in GetChildren())
//		{
//			if (child is Line2D)
//			{
//				Line2D lineChild = (Line2D)child;
//				lineChild.Width = 0.2f * zoom.x;
//			}
//		}
//	}
//
//	// public override void _PhysicsProcess(float delta)
//	// {
//	// 	///
//	// 	if (Input.IsActionJustPressed("ui_accept"))
//	// 	{
//	// 		RefreshScreen();
//	// 		Graph(e);
//	// 		//GD.Print(Screen);
//	// 	}
//	// }
//
//	public Element interpret(String input) {
//		List<String> strList = seperate(input);
//		List<Element> flatElements = new List<Element>();
//		float floatS = 0;
//		for (int i = 0; i < strList.Count; i++) {
//			String s = strList[i];
//			if (float.TryParse(s, out floatS)) {
//				flatElements.Add(new Value("value", float.Parse(s)));
//			} else if (s.Equals("+")) {
//				flatElements.Add(new Add("add", false));
//			} else if (s.Equals("-")) {
//				flatElements.Add(new Add("subtract", true));
//			} else if (s.Equals("*")) {
//				flatElements.Add(new Multiply("multiply", false));
//			} else if (s.Equals("/")) {
//				flatElements.Add(new Multiply("divide", true));
//			} else if (s.Equals("^")) {
//				flatElements.Add(new Exponent("exponent", false));
//			} else if (s.Equals("x") || s.Equals("y")) {
//				flatElements.Add(new Variable("variable", s));
//			} else if (s.Equals("(")) {
//				flatElements.Add(new Paren("open"));
//			} else if (s.Equals(")")) {
//				flatElements.Add(new Paren("close"));
//			} else if (s.Equals("s")) {
//				i++;
//				if (strList[i].Equals("i")) {
//					i++;
//					if (strList[i].Equals("n")) {
//						flatElements.Add(new Sine("sine", false));
//					} else {
//						i -= 2;
//					}
//				} else {
//					i--;
//				}
//			} else if (s.Equals("c")) {
//				i++;
//				if (strList[i].Equals("o")) {
//					i++;
//					if (strList[i].Equals("s")) {
//						flatElements.Add(new Cosine("cosine", false));
//					} else {
//						i -= 2;
//					}
//				} else {
//					i--;
//				}
//			} else if (s.Equals("t")) {
//				i++;
//				if (strList[i].Equals("a")) {
//					i++;
//					if (strList[i].Equals("n")) {
//						flatElements.Add(new Tangent("tangent", false));
//					} else {
//						i -= 2;
//					}
//				} else {
//					i--;
//				}
//			}
//		}
//		Element tree = Pemdas(flatElements);
//		return tree;
//	}
//
//	public List<String> seperate(String input) {
//		//GD.Print(input);
//		input = Regex.Replace(input, @"\s+", "");
//		//GD.Print(input);
//		String type = "";
//		if (Char.IsDigit(input, 0)) {
//			type = "number";
//		} else if (input[0] == 'x') {
//			type = "variable";
//		} else {
//			type = "operator";
//		}
//		List<String> output = new List<String>();
//		String current = "";
//		for (int i = 0; i < input.Length; i++) {
//			if (Char.IsDigit(input, i)) {
//				if (type.Equals("number")) {
//					current += input[i];
//				} else {
//					output.Add(current);
//					current = input[i].ToString();
//					type = "number";
//				}
//			} else if(input[i] == 'x') {
//				output.Add(current);
//				current = input[i].ToString();
//				type = "variable";
//			} else {
//				output.Add(current);
//				current = input[i].ToString();
//				type = "operator";
//			}
//		}
//		output.Add(current);
//		//foreach (String s in output) {
//			//GD.Print(s);
//		//}
//		return output;
//	}
//
//	public Element Pemdas(List<Element> input) {
//		for (int i = 0; i < input.Count; i++) {
//			Element e = input[i];
//			if (e is Paren) {
//				Paren castE = (Paren) e;
//				if (e.type.Equals("open")) {
//					int parenCount = 1;
//					int j;
//					for (j = i+1; parenCount > 0 && j < input.Count; j++) {
//						Element f = input[j];
//						if (f is Paren) {
//							if (f.type.Equals("open")) {
//								parenCount++;
//							} else {
//								parenCount--;
//							}
//						}
//					}
//					Element g = Pemdas(input.GetRange(i+1, j-i-2));
//					castE.setX(g);
//					input.RemoveRange(i+1, j-i-1);
//				}
//			}
//		}
//		for (int i = 0; i < input.Count; i++) {
//			Element e = input[i];
//			if (e is Sine || e is Cosine || e is Tangent) {
//				Operator castE = (Operator) e;
//				castE.setX(input[i+1]);
//				input.RemoveAt(i+1);
//			}
//		}
//		for (int i = 0; i < input.Count; i++) {
//			Element e = input[i];
//			if (e is Exponent) {
//				Operator castE = (Operator) e;
//				castE.setY(input[i+1]);
//				input.RemoveAt(i+1);
//				castE.setX(input[i-1]);
//				input.RemoveAt(i-1);
//				i--;
//			}
//		}
//		for (int i = 0; i < input.Count; i++) {
//			Element e = input[i];
//			if (e is Multiply) {
//				Operator castE = (Operator) e;
//				castE.setY(input[i+1]);
//				input.RemoveAt(i+1);
//				castE.setX(input[i-1]);
//				input.RemoveAt(i-1);
//				i--;
//			}
//		}
//		for (int i = 0; i < input.Count; i++) {
//			Element e = input[i];
//			if (e is Add) {
//				Operator castE = (Operator) e;
//				castE.setY(input[i+1]);
//				input.RemoveAt(i+1);
//				castE.setX(input[i-1]);
//				input.RemoveAt(i-1);
//				i--;
//			}
//		}
//		if (input.Count > 1) {
//			throw new Exception("PEMDAS failed, check for extra operators.");
//		}
//		//GD.Print(input[0].type);
//		return input[0];
//	}
//}
//
//public abstract class Element {
//	public String type;
//
//	public Element(String typeIn) {
//		type = typeIn;
//	}
//
//	abstract public float run(float x);
//	abstract public float run(float x, float y);
//}
//
//public class Value : Element {
//	public float a;
//
//
//	public Value(String typeIn, float aIn) : base(typeIn) {
//		a = aIn;
//	}
//
//	override public float run(float x) {
//		return a;
//	}
//	override public float run(float x, float y) {
//		return a;
//	}
//}
//
//public class Variable : Element {
//	String name;
//	public Variable(String typeIn, String nameIn) : base(typeIn) {
//		name = nameIn;
//	}
//
//	override public float run(float x) {
//		return x;
//	}
//		override public float run(float x, float y) {
//		if (name == "x") {
//			return x;
//		} else if (name == "y") {
//			return y;
//		} else {
//			throw new Exception("bad variable name");
//		}
//	}
//}
//
//public class Paren : Operator {
//	public Paren(String typeIn) : base(typeIn, false) {
//	}
//
//	override public float run(float x) {
//		return a.run(x);
//	}
//	override public float run(float x, float y) {
//		return a.run(x, y);
//	}
//}
//
//abstract public class Operator : Element {
//	public Element a;
//	public Element b;
//	public bool inverse;
//
//	public Operator(String typeIn, bool inverseIn) : base(typeIn) {
//		inverse = inverseIn;
//	}
//
//	public void setX(Element aIn) {
//		a = aIn;
//	}	
//
//	public void setY(Element bIn) {
//		b = bIn;
//	}
//}
//
//public class Add : Operator {
//	public Add(String typeIn, bool inverseIn) : base(typeIn, inverseIn) {
//	}
//
//	override public float run(float x) {
//		if (!inverse) {
//			return a.run(x)+b.run(x);
//		} else {
//			return a.run(x)-b.run(x);
//		}
//
//	}
//	override public float run(float x, float y) {
//		if (!inverse) {
//			return a.run(x, y)+b.run(x, y);
//		} else {
//			return a.run(x, y)-b.run(x, y);
//		}
//
//	}
//}
//
//public class Multiply : Operator {
//	public Multiply(String typeIn, bool inverseIn) : base(typeIn, inverseIn) {
//	}
//
//	override public float run(float x) {
//		if (!inverse) {
//			return a.run(x)*b.run(x);
//		} else {
//			return a.run(x)/b.run(x);
//		}
//	}
//	override public float run(float x, float y) {
//		if (!inverse) {
//			return a.run(x, y)*b.run(x, y);
//		} else {
//			return a.run(x, y)/b.run(x, y);
//		}
//
//	}
//}
//
//public class Exponent : Operator {
//	public Exponent(String typeIn, bool inverseIn) : base(typeIn, inverseIn) {
//	}
//
//	override public float run(float x) {
//		if (!inverse) {
//			return Mathf.Pow(a.run(x), b.run(x));
//		} else {
//			return Mathf.Pow(a.run(x), 1/b.run(x));
//		}
//
//	}
//	override public float run(float x, float y) {
//		if (!inverse) {
//			return Mathf.Pow(a.run(x, y), b.run(x, y));
//		} else {
//			return Mathf.Pow(a.run(x, y), 1/b.run(x, y));
//		}
//
//	}
//}
//
//public class Sine : Operator {
//	public Sine(String typeIn, bool inverseIn) : base(typeIn, inverseIn) {
//	}
//
//	override public float run(float x) {
//		if (!inverse) {
//			return Mathf.Sin(a.run(x));
//		} else {
//			return Mathf.Sin(a.run(x));
//		}
//
//	}
//	override public float run(float x, float y) {
//		if (!inverse) {
//			return Mathf.Sin(a.run(x, y));
//		} else {
//			return Mathf.Sin(a.run(x, y));
//		}
//
//	}
//}
//
//public class Cosine : Operator {
//	public Cosine(String typeIn, bool inverseIn) : base(typeIn, inverseIn) {
//	}
//
//	override public float run(float x) {
//		if (!inverse) {
//			return Mathf.Cos(a.run(x));
//		} else {
//			return Mathf.Cos(a.run(x));
//		}
//
//	}
//	override public float run(float x, float y) {
//		if (!inverse) {
//			return Mathf.Cos(a.run(x, y));
//		} else {
//			return Mathf.Cos(a.run(x, y));
//		}
//
//	}
//}
//
//public class Tangent : Operator {
//	public Tangent(String typeIn, bool inverseIn) : base(typeIn, inverseIn) {
//	}
//
//	override public float run(float x) {
//		if (!inverse) {
//			return Mathf.Tan(a.run(x));
//		} else {
//			return Mathf.Tan(a.run(x));
//		}
//
//	}
//	override public float run(float x, float y) {
//		if (!inverse) {
//			return Mathf.Tan(a.run(x, y));
//		} else {
//			return Mathf.Tan(a.run(x, y));
//		}
//
//	}
//}
