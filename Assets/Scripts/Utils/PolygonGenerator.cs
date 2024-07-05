using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PolygonGenerator : MonoBehaviour
{
    #region setup
    public Vector3[] PolygonPoints;
    public int[] PolygonTriangles;
    public Vector2[] Uvs;
    public bool IsFilled;
    public int PolygonSides;
    public float PolygonRadius;
    public float CenterRadius;

    private Mesh _mesh;
    
    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void Update()
    {
        if (IsFilled)
        {
            DrawFilled(PolygonSides, PolygonRadius);
        }
        else
        {
            DrawHollow(PolygonSides, PolygonRadius, CenterRadius);
        }
    }
    #endregion

    private void DrawFilled(int sides, float radius)
    {
        PolygonPoints = GetCircumferencePoints(sides, radius).ToArray();
        PolygonTriangles = DrawFilledTriangles(PolygonPoints);
        _mesh.Clear();
        _mesh.vertices = PolygonPoints;
        _mesh.triangles = PolygonTriangles;
    }

    public void DrawHollow(int sides, float outerRadius, float innerRadius)
    {
        List<Vector3> pointsList = new List<Vector3>();
        List<Vector3> outerPoints = GetCircumferencePoints(sides, outerRadius);
        pointsList.AddRange(outerPoints);
        List<Vector3> innerPoints = GetCircumferencePoints(sides, innerRadius);
        pointsList.AddRange(innerPoints);

        PolygonPoints = pointsList.ToArray();

        PolygonTriangles = DrawHollowTriangles(PolygonPoints);
        _mesh.Clear();
        _mesh.vertices = PolygonPoints;
        _mesh.triangles = PolygonTriangles;

        // okay, calculate the bounding box of our generated mesh (where outer most vertices of ring mesh reach)
        Vector3 min = Vector3.one * float.MaxValue;
        Vector3 max = Vector3.one * float.MinValue;
        foreach (Vector3 point in _mesh.vertices)
        {
            min = Vector3.Min(min, point);
            max = Vector3.Max(max, point);
        }

        // now, set the uv array to the length of our vertices array (have to be the same length, this is important!)
        Uvs = new Vector2[_mesh.vertices.Length];
        for (int i = 0; i < Uvs.Length; i++)
        {
            // now...
            // for each axis, map the position of the vertex to some position along a straight line between the
            // min and max values for each axis within our bounding box
            // remember, use InverseLerp when we have a specific value and want to find its position within a range
            // and Lerp when we have normalized coordinates and want to find a specific value within a range
            Vector3 normalizedPoint = new Vector3(
                Mathf.InverseLerp(min.x, max.x, _mesh.vertices[i].x),
                Mathf.InverseLerp(min.y, max.y, _mesh.vertices[i].y),
                Mathf.InverseLerp(min.z, max.z, _mesh.vertices[i].z)
            );
            // and then, set our uv array to the new, normalized coordinates
            // these represent the position of the vertex within our uv space
            Uvs[i] = new Vector2(normalizedPoint.x, normalizedPoint.y);
        }
        // assign the new uvs to our mesh :)
        _mesh.uv = Uvs;
    }

    private static int[] DrawHollowTriangles(Vector3[] points)
    {
        int sides = points.Length / 2;
        int triangleAmount = sides * 2;

        List<int> newTriangles = new List<int>();
        for (int i = 0; i < sides; i++)
        {
            int innerIndex = i + sides;

            //first triangle starting at outer edge i
            newTriangles.Add(i);
            newTriangles.Add(innerIndex);
            newTriangles.Add((i + 1) % sides);

            //second triangle starting at outer edge i
            newTriangles.Add(i);
            newTriangles.Add(sides + ((sides + i - 1) % sides));
            newTriangles.Add(i + sides);
        }
        return newTriangles.ToArray();
    }

    private static List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        const float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, 0));
        }
        return points;
    }

    private static int[] DrawFilledTriangles(Vector3[] points)
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
