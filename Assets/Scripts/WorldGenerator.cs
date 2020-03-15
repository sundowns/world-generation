using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class WorldGenerator : MonoBehaviour
{
    public int cols = 50;
    public int rows = 50;
    public float world_height = 10f;
    [Range(0, 1)]
    public float water_level = 0.1f;
    [Range(0, 20)]
    public float terrace_height = 0.5f;

    // Noise Generation values
    public int octaves = 4;
    public float noise_scale;
    public Vector2 noise_offset = new Vector2(0, 0);
    [Range(0, 1)]
    public float persistence = 1f;
    public float lacunarity = 2f;
    public int seed;
    public bool use_random_seed = true;

    // Start is called before the first frame update
    void Start()
    {
        if (use_random_seed)
        {
            seed = System.DateTime.Now.GetHashCode();
        }

        var noise_map = NoiseGenerator.GenerateNoiseMap(cols, rows, seed, noise_scale, octaves, persistence, lacunarity, noise_offset);

        GetComponent<MeshGenerator>().Generate(noise_map, cols, rows, world_height, water_level, terrace_height);
    }
}