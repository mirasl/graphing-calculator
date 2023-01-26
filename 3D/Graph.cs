using Godot;
using System;
using Godot.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Transforms multiple points on a MeshInstance to represent the 3D graph of an
/// equation using x, y, and z variables. Accounts for both static graphs as
/// well as animated graphs (graphs which utilize the "t" variable).
/// </summary>
public class Graph : MeshInstance
{
	/// <summary>
	/// The current x value being iterated over.
	/// </summary>
	public static float x;

	/// <summary>
	/// The current y value being iterated over.
	/// </summary>
	public static float y;

	/// <summary>
	/// The current t value being iterated over.
	/// </summary>
	public static float t;

	/// <summary>
	/// The maximum allowed precision value of the graph (multiplied by user-
	/// inputted "precision percent" value to determine final precision).
	/// </summary>
	const int MAX_PRECISION = 500;

	/// <summary>
	/// The maximum allowed precision value of an animated graph (should be
	/// lower than MAX_PRECISION as the points must be redrawn each frame, thus
	/// much more processing power will be expended). Multiplied by user-
	/// inputted "precision percent" value to determine final precision.
	/// </summary>
	const int MAX_ANIMATION_PRECISION = 100;

	/// <summary>
	/// The element object which will interpret the equation over each xy point
	/// to form the shape of the graph.
	/// </summary>
	public Element e;

	/// <summary>
	/// Object which is manipulated to draw a static graph (does not need to
	/// be redrawn each frame)
	/// </summary>
	SurfaceTool st;

	/// <summary>
	/// Object which is manipulated to draw an animated graph (redrawn each
	/// frame)
	/// </summary>
	ImmediateGeometry ig;

	/// <summary>
	/// Represents time (in seconds) since this script was initiated
	/// </summary>
	float time = 0;

	/// <summary>
	/// Sets onready variables.
	/// </summary>
	public override void _Ready()
	{
		ig = GetNode<ImmediateGeometry>("ImmediateGeometry");
		st = new SurfaceTool();
	}

	/// <summary>
	/// Updates time value each frame.
	/// </summary>
	/// <param name="delta">Time (s) between frames.</param>
	public override void _PhysicsProcess(float delta)
	{
		Graph.t += delta;
	}

	/// <summary>
	/// Draws a static graph as a collection of points over the given plane.
	/// </summary>
	/// <param name="size">Size of the xy-plane which is graphed over</param>
	/// <param name="precisionPercent">Concentration of points within the space 
	/// 		on a scale from 0 to 1, 1 being more concentrated.</param>
	public void DrawGraph(Vector2 size, float precisionPercent)
	{
		precisionPercent = Mathf.Clamp(precisionPercent, 0, 1);
		int precision = (int)(precisionPercent * (float)MAX_PRECISION);

		st = new SurfaceTool();

		st.Begin(Mesh.PrimitiveType.Lines);

		for (int i = -precision; i < precision; i++)
		{
			float x = (float)i / (float)precision * size.x;
			for (int j = -precision; j < precision; j++)
			{
				float z = (float)j / (float)precision * size.y;

				Graph.x = x;
				Graph.y = z;
				st.AddVertex(new Vector3(x, e.run(), z));
			}
		}

		Godot.ArrayMesh m = st.Commit();

		Mesh = m;
	}
	
	/// <summary>
	/// Draws an animated 3D graph as a collection of points over the given 
	/// plane.
	/// </summary>
	/// <param name="size">Size of the xy-plane which is graphed over</param>
	/// <param name="precisionPercent">Concentration of points within the space 
	/// 		on a scale from 0 to 1, 1 being more concentrated.</param>
	public void DrawAnimatedGraph(Vector2 size, float precisionPercent)
	{
		precisionPercent = Mathf.Clamp(precisionPercent, 0, 1);
		int precision = (int)(precisionPercent * MAX_ANIMATION_PRECISION);
		//GD.Print(size);

		ig.Clear();

		ig.Begin(Mesh.PrimitiveType.Lines);

		for (int i = -precision; i < precision; i++)
		{
			float x = (float)i / (float)precision * size.x;
			for (int j = -precision; j < precision; j++)
			{
				float z = (float)j / (float)precision * size.y;

				Graph.x = x;
				Graph.y = z;
				ig.AddVertex(new Vector3(x, e.run(), z));
			}
		}

		ig.End();
	}

	/// <summary>
	/// Clears the current Mesh and SurfaceTool.
	/// </summary>
	public void ClearGraph()
	{
		st.Clear();
		Mesh = null;
	}

	public Element interpret(String input) {
		List<String> strList = seperate(input);
		List<Element> flatElements = new List<Element>();
		float floatS = 0;
		for (int i = 0; i < strList.Count; i++) {
			String s = strList[i];
			if (float.TryParse(s, out floatS)) {
				flatElements.Add(new Value(float.Parse(s)));
			} else if (s.Equals(".")) {
				flatElements.Add(new Point(false));
			} else if (s.Equals("+")) {
				flatElements.Add(new Add(false));
			} else if (s.Equals("-")) {
				flatElements.Add(new Add(true));
			} else if (s.Equals("*")) {
				flatElements.Add(new Multiply(false));
			} else if (s.Equals("/")) {
				flatElements.Add(new Multiply(true));
			} else if (s.Equals("^")) {
				flatElements.Add(new Exponent(false));
			} else if (s.Equals("x")) {
				flatElements.Add(new X());
			} else if (s.Equals("y")) {
				flatElements.Add(new Y());
			} else if (s.Equals("t")) {
				flatElements.Add(new T());
			} else if (s.Equals("(")) {
				flatElements.Add(new Paren("open"));
			} else if (s.Equals(")")) {
				flatElements.Add(new Paren("close"));
			} else if (s.Equals("s")) {
				i++;
				if (strList[i].Equals("i")) {
					i++;
					if (strList[i].Equals("n")) {
						flatElements.Add(new Sine(false));
					} else {
						i -= 2;
					}
				} else {
					i--;
				}
			} else if (s.Equals("c")) {
				i++;
				if (strList[i].Equals("o")) {
					i++;
					if (strList[i].Equals("s")) {
						flatElements.Add(new Cosine(false));
					} else {
						i -= 2;
					}
				} else {
					i--;
				}
			} else if (s.Equals("t")) {
				i++;
				if (strList[i].Equals("a")) {
					i++;
					if (strList[i].Equals("n")) {
						flatElements.Add(new Tangent(false));
					} else {
						i -= 2;
					}
				} else {
					i--;
				}
			} else if (s.Equals("l")) {
				i++;
				if (strList[i].Equals("n")) {
					flatElements.Add(new Ln(false));
				} else {
					i--;
				}
			}
		}
		foreach (Element e in flatElements) {
			//GD.Print(e.type);
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
		} else {
			type = "operator or variable";
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
			} else {
				output.Add(current);
				current = input[i].ToString();
				type = "operator or variable";
			}
		}
		output.Add(current);
//		foreach (String s in output) {
//			GD.Print(s);
//		}
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
					castE.setX(g);
					input.RemoveRange(i+1, j-i-1);
				}
			}
		}
		for (int i = 0; i < input.Count; i++) {
			Element e = input[i];
			if (e is Point) {
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
			if (e is Sine || e is Cosine || e is Tangent || e is Ln) {
				Operator castE = (Operator) e;
				castE.setX(input[i+1]);
				input.RemoveAt(i+1);
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
				if (i-1>=0) {
					if (input[i-1].full()) {
						castE.setX(input[i-1]);
						input.RemoveAt(i-1);
						i--;
					}
				}
				if (!castE.full()) {
					castE.setX(new Value(0));
				}
			}
		}
		if (input.Count > 1) {
			//return new X();
			throw new Exception("PEMDAS failed, check for extra operators.");
		}
		return input[0];
	}
}

public abstract class Element {
	public String type;
	
	public Element(String typeIn) {
		type = typeIn;
	}
	
	abstract public bool full();
	abstract public float run();
}

public class Value : Element {
	public float a;
	
	
	public Value(float aIn) : base("value") {
		a = aIn;
	}
	
	override public float run() {
		return a;
	}
	
	override public bool full() {
		return true;
	}
	
	public float testMethod() {
		return Graph.t;
	}
}

public class X : Element {
	public X() : base("x") {
	}
	
	override public bool full() {
		return true;
	}
	
	override public float run() {
		return Graph.x;
	}
}

public class Y : Element {
	public Y() : base("y") {
	}
	
	override public bool full() {
		return true;
	}
	
	override public float run() {
		return Graph.y;
	}
}

public class T : Element {
	public T() : base("t") {
	}
	
	override public bool full() {
		return true;
	}
	
	override public float run() {
		return Graph.t;
	}
}

public class Paren : Operator {
	public Paren(String typeIn) : base(typeIn, false) {
		a = null;
	}
	
	override public bool full() {
		return !(a is null);
	}
	
	override public float run() {
		return a.run();
	}
}

abstract public class Operator : Element {
	public Element a;
	public Element b;
	public bool inverse;
	
	public Operator(String typeIn, bool inverseIn) : base(typeIn) {
		inverse = inverseIn;
		a = null;
	}
	
	override public bool full() {
		return !(a is null);
	}
	
	public void setX(Element aIn) {
		a = aIn;
	}	
	
	public void setY(Element bIn) {
		b = bIn;
	}
}

public class Point : Operator {
	public Point(bool inverseIn) : base("add", inverseIn) {
	}
	
	override public float run() {
		float bVal = b.run();
		float aVal = a.run();
		while (bVal > 1) {
			bVal = bVal/10;
		}
		if (aVal >= 0) {
			return aVal+bVal;
		} else {
			return aVal-bVal;
		}
		
	}
}

public class Add : Operator {
	public Add(bool inverseIn) : base("add", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return a.run()+b.run();
		} else {
			return a.run()-b.run();
		}
		
	}
}

public class Multiply : Operator {
	public Multiply(bool inverseIn) : base("multiply", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return a.run()*b.run();
		} else {
			return a.run()/b.run();
		}
	}
}

public class Exponent : Operator {
	public Exponent(bool inverseIn) : base("exponent", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return Mathf.Pow(a.run(), b.run());
		} else {
			return Mathf.Pow(a.run(), 1/b.run());
		}
		
	}
}

public class Sine : Operator {
	public Sine(bool inverseIn) : base("sine", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return Mathf.Sin(a.run());
		} else {
			return Mathf.Sin(a.run());
		}
		
	}
}

public class Cosine : Operator {
	public Cosine(bool inverseIn) : base("cosine", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return Mathf.Cos(a.run());
		} else {
			return Mathf.Cos(a.run());
		}
		
	}
}

public class Tangent : Operator {
	public Tangent(bool inverseIn) : base("tangent", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return Mathf.Tan(a.run());
		} else {
			return Mathf.Tan(a.run());
		}
		
	}
}

public class Ln : Operator {
	public Ln(bool inverseIn) : base("ln", inverseIn) {
	}
	
	override public float run() {
		if (!inverse) {
			return Mathf.Log(a.run());
		} else {
			return Mathf.Log(a.run());
		}
		
	}
}
