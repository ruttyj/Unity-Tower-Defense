using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise2D : MonoBehaviour
{
    int rows;
    int cols;
    float scale;
    float perlinOffsetRow;
    float perlinOffsetCol;

    public PerlinNoise2D(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        this.scale = Mathf.Max(rows, cols) * 30;
        this.perlinOffsetRow = Random.Range(0, 100);
        this.perlinOffsetCol = Random.Range(0, 100);
    }

    public float GetValue(float row, float col)
    {
        float pr = (float)row / (float)rows * scale + perlinOffsetRow;
        float pc = (float)col / (float)cols + scale + perlinOffsetCol;
        return Mathf.PerlinNoise(pr, pc);
    }
}
