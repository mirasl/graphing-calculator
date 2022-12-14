using Godot;
using System;

public class CameraControls : Camera2D
{
    private const int SPEED_MULTIPLIER = 1;

    [Signal] delegate void ChangeZoom(Vector2 newZoom);

    public override void _Process(float delta)
    {
        float currentSpeed = Zoom.x * SPEED_MULTIPLIER;
        if (Input.IsActionPressed("ui_right"))
        {
            Position = new Vector2(Position.x + currentSpeed, Position.y);
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            Position = new Vector2(Position.x - currentSpeed, Position.y);
        }

        if (Input.IsActionPressed("ui_up"))
        {
            Position = new Vector2(Position.x, Position.y - currentSpeed);
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            Position = new Vector2(Position.x, Position.y + currentSpeed);
        }

        if (Input.IsActionPressed("ctrl"))
        {
            if (Input.IsActionJustPressed("equal"))
            {
                Zoom = new Vector2(Zoom.x * 0.8f, Zoom.y * 0.8f);
                EmitSignal("ChangeZoom", Zoom);
            }

            if (Input.IsActionJustPressed("minus"))
            {
                Zoom = new Vector2(Zoom.x * 1.2f, Zoom.y * 1.2f);
                EmitSignal("ChangeZoom", Zoom);
            }
        }
    }
}
