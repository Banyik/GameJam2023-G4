using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    int xMax = 10;
    int yMax = 10;
    float scale = 15;
    public float xOffset = 10;
    public float yOffset = 10;
    float maxOffset = 100000f;
    float mapType = 0;
    private void Start()
    {
        CalculateOffsets();
        for (int i = 1; i <= xMax; i++)
        {
            for (int j = 1; j <= yMax; j++)
            {
                mapType = (Mathf.Round((CalculateMapPerlinNoise(i, j) + 1) * 2) / 2 * 2) - 2;
                Debug.Log(mapType);
            }
        }
    }
    void CalculateOffsets()
    {
        xOffset = Random.Range(0.0f, maxOffset);
        yOffset = Random.Range(0.0f, maxOffset);
    }
    float CalculateMapPerlinNoise(float x, float y)
    {
        float xNoise = x / (float)xMax * scale+ xOffset;
        float yNoise = y / (float)yMax * scale+ yOffset;
        return Mathf.PerlinNoise(xNoise, yNoise);
    }
}
