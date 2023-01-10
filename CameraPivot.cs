using Godot;
using System;

public class CameraPivot : Spatial
{
    private const float ROTATION_SPEED = 0.02f; // radians per frame
    private const float TRANSLATION_SPEED = 0.07f; // meters per frame
    private const float ZOOM_FACTOR = 0.8f; // multiplied by current zoom

    public bool Disabled = false;

    public override void _PhysicsProcess(float delta)
    {
        if (!Disabled)
        {
            if (Input.IsActionPressed("shift"))
            {
                Pan();
            }
            else
            {
                Orbit();
            }
            if (Input.IsActionPressed("ctrl"))
            {
                Zoom();
            }
        }
    }

    private void Orbit()
    {
        if (Input.IsActionPressed("ui_left"))
        {
            RotateY(-ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            RotateY(ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_up") && Rotation.x > -Mathf.Pi/2 + 0.05f)
        {
            Rotate(Vector3.Left.Rotated(Vector3.Up, Rotation.y), 
                    ROTATION_SPEED);
        }
        if (Input.IsActionPressed("ui_down") && Rotation.x < Mathf.Pi/2 - 0.05f)
        {
            Rotate(Vector3.Left.Rotated(Vector3.Up, Rotation.y), 
                    -ROTATION_SPEED);
        }
    }

    private void Pan()
    {
        if (Input.IsActionPressed("ctrl"))
        {
            if (Input.IsActionPressed("ui_up"))
            {
                Transform = Transform.Translated(Vector3.Forward * 
                        TRANSLATION_SPEED);
            }
            if (Input.IsActionPressed("ui_down"))
            {
                Transform = Transform.Translated(Vector3.Back * 
                        TRANSLATION_SPEED);
            }
        }
        else
        {
            if (Input.IsActionPressed("ui_left"))
            {
                Transform = Transform.Translated(Vector3.Left * 
                        TRANSLATION_SPEED);
            }
            if (Input.IsActionPressed("ui_right"))
            {
                Transform = Transform.Translated(Vector3.Right * 
                        TRANSLATION_SPEED);
            }
            if (Input.IsActionPressed("ui_up"))
            {
                Transform = Transform.Translated(Vector3.Up * 
                        TRANSLATION_SPEED);
            }
            if (Input.IsActionPressed("ui_down"))
            {
                Transform = Transform.Translated(Vector3.Down * 
                        TRANSLATION_SPEED);
            }
        }
    }

    private void Zoom()
    {
        if (Input.IsActionJustPressed("equal"))
        {
            Camera cam = GetNode<Camera>("Camera");
            var transform = cam.Transform;
            transform.origin.z = transform.origin.z * ZOOM_FACTOR;
            cam.Transform = transform;
        }
        else if (Input.IsActionJustPressed("minus"))
        {
            Camera cam = GetNode<Camera>("Camera");
            var transform = cam.Transform;
            transform.origin.z = transform.origin.z * (1 / ZOOM_FACTOR);
            cam.Transform = transform;
        }
    }
}
