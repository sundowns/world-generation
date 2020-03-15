using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colours;

    public Gradient gradient;
    public MeshCollider mesh_collider = null;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        UpdateMesh();
    }

    float ToTerracedHeight(float height, float terrace_height)
    {
        return Mathf.Round(height / terrace_height) * terrace_height;
    }

    public void Generate(float[,] noise_map, int cols, int rows, float world_height, float water_level, float terrace_height)
    {
        vertices = new Vector3[(cols) * (rows)];

        float max_height = 0f;
        float min_height = world_height;
        float water_height = water_level * world_height;

        // Define the vertices
        for (int i = 0, z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++, i++)
            {
                var y = world_height * noise_map[x, z];

                vertices[i] = new Vector3(x, Mathf.Max(ToTerracedHeight(y, terrace_height), water_height), z);

                // Find the min/max height positions of the mesh (used to normalize colour picking later)
                if (y > max_height)
                    max_height = y;

                if (y < min_height)
                    min_height = y;
            }
        }

        // Create our triangles
        triangles = new int[6 * rows * cols];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < rows - 1; z++)
        {
            for (int x = 0; x < cols - 1; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + cols;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + cols;
                triangles[tris + 5] = vert + cols + 1;

                vert++;
                tris += 6;

                // yield return new WaitForSeconds(0.0000001f);
            }
            vert++;
        }

        // Create our colour points
        colours = new Color[vertices.Length];
        for (int i = 0, z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++, i++)
            {
                // Normalise our height based on min/max points
                float height = Mathf.InverseLerp(min_height, max_height, vertices[i].y);
                colours[i] = gradient.Evaluate(height);
            }
        }

    }

    void UpdateMesh()
    {
        if (mesh == null)
            return;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colours;
        mesh.RecalculateNormals();

        if (mesh_collider != null)
            mesh_collider.sharedMesh = mesh;
    }
}