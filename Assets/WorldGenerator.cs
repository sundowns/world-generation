using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class WorldGenerator : MonoBehaviour
{
    public int cols = 50;
    public int rows = 50;
    public float max_height = 100f;

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

        StartCoroutine(GetComponent<MeshGenerator>().Generate(noise_map, cols, rows, max_height));
    }
}