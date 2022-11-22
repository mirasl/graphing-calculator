using Godot;
using System;

public class TextInput : LineEdit
{
    [Signal] delegate void InputGraph(string input);

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("enter"))
        {
            string input = this.Text;
            EmitSignal("InputGraph", input);
        }
    }
}
