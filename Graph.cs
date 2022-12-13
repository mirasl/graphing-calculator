using Godot;
using System;

public class Graph : MeshInstance
{
    public override void _Ready()
    {
        DrawGraph();
    }

    public void DrawGraph()
    {
        SurfaceTool st = new SurfaceTool();

        st.Begin(Mesh.PrimitiveType.Lines);

        for (int i = -100; i < 100; i++)
        {
            float x = (float)i / 100.0f;
            for (int j = -100; j < 100; j++)
            {
                float z = (float)j / 100.0f;

                st.AddVertex(new Vector3(x, x*x - z*z, z));
            }
        }


        Godot.ArrayMesh m = st.Commit();

        Mesh = m;
    }

}
