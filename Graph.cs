using Godot;
using System;

public class Graph : MeshInstance
{
    const int PRECISION = 500; // concentration of points
    const int MOVING_PRECISION = 150; // concentration of points for moving graphs (must be redrawn each frame)

    float time = 0;
    ImmediateGeometry ig;

    public override void _Ready()
    {
        ig = GetNode<ImmediateGeometry>("ImmediateGeometry");

        //DrawGraph(new Rect2(new Vector2(0,0), new Vector2(15, 15)));
    }

    public override void _PhysicsProcess(float delta)
    {
        time += delta;
        DrawMovingGraph(new Rect2(new Vector2(0,0), new Vector2(15, 15)), time);
    }

    public void DrawGraph(Rect2 size)
    {
        SurfaceTool st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Lines);

        for (int i = -PRECISION; i < PRECISION; i++)
        {
            float x = (float)i / (float)PRECISION * size.Size.x;
            for (int j = -PRECISION; j < PRECISION; j++)
            {
                float z = (float)j / (float)PRECISION * size.Size.y;

                // FUNCTION HERE: //
                st.AddVertex(new Vector3(x, Mathf.Sin(x) - Mathf.Sin(z), z));
            }
        }


        Godot.ArrayMesh m = st.Commit();

        Mesh = m;
    }

    public void DrawMovingGraph(Rect2 size, float t)
    {
        ig.Clear();
        ig.Begin(Mesh.PrimitiveType.Lines);

        for (int i = -MOVING_PRECISION; i < MOVING_PRECISION; i++)
        {
            float x = (float)i / (float)MOVING_PRECISION * size.Size.x;

            for (int j = -MOVING_PRECISION; j < MOVING_PRECISION; j++)
            {
                float z = (float)j / (float)MOVING_PRECISION * size.Size.y;

                // FUNCTION HERE: //
                ig.AddVertex(new Vector3(x, Mathf.Sin(x + t) - Mathf.Sin(z + t), z));
            }
        }

        ig.End();
    }
}
