using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTexture : MonoBehaviour
{

    public int width = 2;
    public int height = 3;

    public int resolution = 256;

    public float scale = 10f;

    public float xOffset = 100f;
    public float yOffset = 100f;

    public Color colour;

    [Range(0, 1)]
    public float clampBottom = 0f;
    [Range(0, 1)]
    public float clampTop = 0.5f;

    void Start()
    {
        width *= resolution;
        height *= resolution;

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D tex = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Color c = CalculateColour(x, y);
                tex.SetPixel(x, y, c);
            }

        tex.Apply();
        return tex;
    }

    Color CalculateColour(int x, int y)
    {
        float xNorm = (float)x / width * scale + xOffset;
        float yNorm = (float)y / height * scale + yOffset;

        float sample = Mathf.PerlinNoise(xNorm, yNorm);

        sample = ClampSample(sample);

        Color c = new Color(sample, sample, sample) * colour;

        return c;
    }

    float ClampSample(float sample)
    {
        return (sample - 0) * (clampTop - clampBottom) / (1 - 0) + clampBottom;
    }
}
