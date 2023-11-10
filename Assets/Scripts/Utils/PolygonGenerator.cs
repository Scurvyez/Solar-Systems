using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    #region setup
    //mesh properties
    Mesh mesh;
    public Vector3[] polygonPoints;
    public int[] polygonTriangles;
    public Vector2[] uvs;

    //polygon properties
    public bool isFilled;
    public int polygonSides;
    public float polygonRadius;
    public float centerRadius;

    void Start()
    {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        if (isFilled)
        {
            DrawFilled(polygonSides, polygonRadius);
        }
        else
        {
            DrawHollow(polygonSides, polygonRadius, centerRadius);
        }
    }
    #endregion

    public void DrawFilled(int sides, float radius)
    {
        polygonPoints = GetCircumferencePoints(sides, radius).ToArray();
        polygonTriangles = DrawFilledTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
    }

    public void DrawHollow(int sides, float outerRadius, float innerRadius)
    {
        List<Vector3> pointsList = new List<Vector3>();
        List<Vector3> outerPoints = GetCircumferencePoints(sides, outerRadius);
        pointsList.AddRange(outerPoints);
        List<Vector3> innerPoints = GetCircumferencePoints(sides, innerRadius);
        pointsList.AddRange(innerPoints);

        polygonPoints = pointsList.ToArray();

        polygonTriangles = DrawHollowTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;

        // okay, calculate the bounding box of our generated mesh (where outer most vertices of ring mesh reach)
        Vector3 min = Vector3.one * float.MaxValue;
        Vector3 max = Vector3.one * float.MinValue;
        foreach (Vector3 point in mesh.vertices)
        {
            min = Vector3.Min(min, point);
            max = Vector3.Max(max, point);
        }

        // now, set the uv array to the length of our vertices array (have to be the same length, this is important!)
        uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            // now...
            // for each axis, map the position of the vertex to some position along a straight line between the
            // min and max values for each axis within our bounding box
            // remember, use InverseLerp when we have a specific value and want to find its position within a range
            // and Lerp when we have normalized coordinates and want to find a specific value within a range
            Vector3 normalizedPoint = new Vector3(
                Mathf.InverseLerp(min.x, max.x, mesh.vertices[i].x),
                Mathf.InverseLerp(min.y, max.y, mesh.vertices[i].y),
                Mathf.InverseLerp(min.z, max.z, mesh.vertices[i].z)
            );
            // and then, set our uv array to the new, normalized coordinates
            // these represent the position of the vertex within our uv space
            uvs[i] = new Vector2(normalizedPoint.x, normalizedPoint.y);
        }
        // assign the new uvs to our mesh :)
        mesh.uv = uvs;
    }

    int[] DrawHollowTriangles(Vector3[] points)
    {
        int sides = points.Length / 2;
        int triangleAmount = sides * 2;

        List<int> newTriangles = new List<int>();
        for (int i = 0; i < sides; i++)
        {
            int outerIndex = i;
            int innerIndex = i + sides;

            //first triangle starting at outer edge i
            newTriangles.Add(outerIndex);
            newTriangles.Add(innerIndex);
            newTriangles.Add((i + 1) % sides);

            //second triangle starting at outer edge i
            newTriangles.Add(outerIndex);
            newTriangles.Add(sides + ((sides + i - 1) % sides));
            newTriangles.Add(outerIndex + sides);
        }
        return newTriangles.ToArray();
    }

    List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, 0));
        }
        return points;
    }

    int[] DrawFilledTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length - 2;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i++)
        {
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }
        return newTriangles.ToArray();
    }
}
