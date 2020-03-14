using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

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

    public IEnumerator Generate(float[,] noise_map, int cols, int rows, float max_height)
    {
        vertices = new Vector3[(cols) * (rows)];


        for (int i = 0, z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++, i++)
            {
                vertices[i] = new Vector3(x, max_height * noise_map[x, z], z);
            }
        }

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

                yield return new WaitForSeconds(0.0000001f);
            }
            vert++;
        }

    }

    void UpdateMesh()
    {
        if (mesh == null)
            return;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}