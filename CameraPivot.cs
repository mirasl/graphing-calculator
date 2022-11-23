using Godot;
using System;

public class CameraPivot : Spatial
{
    private const float ROTATION_SPEED = 0.04f; // radians per frame
    private const float TRANSLATION_SPEED = 0.06f; // meters per frame

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("shift"))
        {
            Pan();
        }
        else
        {
            Orbit();
        }
    }

    private void Orbit()
    {
        if (Input.IsActionPressed("ui_left"))
        {
            Transform = Transform.Rotated(Vector3.Up, -ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            Transform = Transform.Rotated(Vector3.Up, ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_up") && Rotation.x > -Mathf.Pi/2 + 0.05f)
        {
            Transform = Transform.Rotated(Vector3.Left.Rotated(Vector3.Up, Rotation.y), ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_down") && Rotation.x < Mathf.Pi/2 - 0.05f)
        {
            Transform = Transform.Rotated(Vector3.Left.Rotated(Vector3.Up, Rotation.y), -ROTATION_SPEED);
        }
    }

    private void Pan()
    {
        if (Input.IsActionPressed("ui_left"))
        {
            Transform = Transform.Translated(Vector3.Left * TRANSLATION_SPEED);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            Transform = Transform.Translated(Vector3.Right * TRANSLATION_SPEED);
        }
        if (Input.IsActionPressed("ui_up"))
        {
            Transform = Transform.Translated(Vector3.Up * TRANSLATION_SPEED);
        }
        if (Input.IsActionPressed("ui_down"))
        {
            Transform = Transform.Translated(Vector3.Down * TRANSLATION_SPEED);
        }
    }
}
