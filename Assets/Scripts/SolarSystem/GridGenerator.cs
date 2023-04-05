using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int gridSize = 10;
    public float spacing = 10f;

    public LineRenderer lineRenderer;
    private Vector3[] linePositions;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GenerateGrid(spacing);
    }

    private void GenerateGrid(float spacing)
    {
        // Set the number of positions for the line renderer
        int numPositions = (gridSize + 1) * 4;

        // Initialize an array to hold the positions
        linePositions = new Vector3[numPositions];

        // Calculate the position of each horizontal line
        for (int i = 0; i <= gridSize; i++)
        {
            float x = (i - (gridSize / 2f)) * spacing;
            float z = gridSize / 2f * spacing;
            linePositions[i * 2] = new Vector3(x, 0, -z);
            linePositions[i * 2 + 1] = new Vector3(x, 0, z);

            // Calculate the position of each vertical line
            linePositions[(gridSize + 1) * 2 + i * 2] = new Vector3(-z, 0, x);
            linePositions[(gridSize + 1) * 2 + i * 2 + 1] = new Vector3(z, 0, x);
        }
        // Set the positions for the line renderer
        lineRenderer.positionCount = numPositions;
        lineRenderer.SetPositions(linePositions);
    }
}
