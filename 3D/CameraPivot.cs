using Godot;
using System;

/// <summary>
/// Handles input which allows the user to move the camera
/// </summary>
public class CameraPivot : Spatial
{
    /// <summary>
    /// Camera rotation speed in radians per frame.
    /// </summary>
    private const float ROTATION_SPEED = 0.02f;

    /// <summary>
    /// Camera translation speed in meters per frame.
    /// </summary>
    private const float TRANSLATION_SPEED = 0.07f;

    /// <summary>
    /// When zoom key pressed, this value (or its inverse) is multiplied by
    /// the current zoom value to zoom the camera in or out. Scale from 0 to
    /// 1; lower values result in larger zoom intervals.
    /// </summary>
    private const float ZOOM_FACTOR = 0.8f;

    /// <summary>
    /// If true, camera does not accept input and is "locked" (cannot move).
    /// </summary>
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

    /// <summary>
    /// Handles input which causes the camera to orbit around a point.
    /// </summary>
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

    /// <summary>
    /// Handles input which causes the camera to translate in all six 3D
    /// directions.
    /// </summary>
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

    /// <summary>
    /// Handles input which causes the camera to zoom in and out by moving the
    /// camera closer to or further from a point.
    /// </summary>
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
