using Godot;
using System;

public class CameraPivot : Spatial
{
    private const float ROTATION_SPEED = 0.04f; // radians per frame

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("shift")) // orbit
        {
            //Pan();
        }
        else // pan
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
            // use Rotation.x to figure it out (the problem is that Vector3.Left changes when you pass +-90 degrees Rotation.x)
            Transform = Transform.Rotated(Vector3.Left.Rotated(Vector3.Up, Rotation.y), ROTATION_SPEED);//(Mathf.Abs(Rotation.x) < Mathf.Pi/2 - 0.1f && Mathf.Abs(Rotation.z) < Mathf.Pi/2 - 0.1f) ? Rotation.y : 0), ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_down") && Rotation.x < Mathf.Pi/2 - 0.05f)
        {
            Transform = Transform.Rotated(Vector3.Left.Rotated(Vector3.Up, Rotation.y), -ROTATION_SPEED);//(Mathf.Abs(Rotation.x) < Mathf.Pi/2 - 0.1f && Mathf.Abs(Rotation.z) < Mathf.Pi/2 - 0.1f) ? Rotation.y : 0), -ROTATION_SPEED);
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
