using Godot;
using System;

public class CameraPivot : Spatial
{
    private const float ROTATION_SPEED = 0.04f; // radians per frame

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("shift"))
        {
            // Pan();
        }
        else
        {
            Orbit();
        }
    }

    private void Orbit()
    {
        GD.Print(Rotation.x);
        RayCast rc = GetNode<RayCast>("RayCast");
        rc.CastTo = Vector3.Left;
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
    // private void Pan()
    // {
    //     if (Input.IsActionJustPressed("ui_left"))
    //     {
    //         Transform.origin.
    //     }
    // }
}
