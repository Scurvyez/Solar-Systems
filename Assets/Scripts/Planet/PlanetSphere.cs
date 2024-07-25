using UnityEngine;

public class PlanetSphere
{
    public Mesh Mesh => mesh;
    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;

    public PlanetSphere(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                // Calculate UV coordinates
                float u = Mathf.Atan2(pointOnUnitSphere.z, pointOnUnitSphere.x) / (2 * Mathf.PI) + 0.5f;
                float v = Mathf.Asin(pointOnUnitSphere.y) / Mathf.PI + 0.5f;
                uvs[i] = new Vector2(u, v);

                if (x == resolution - 1 || y == resolution - 1) continue;
                triangles[triIndex] = i;
                triangles[triIndex + 1] = i + resolution + 1;
                triangles[triIndex + 2] = i + resolution;

                triangles[triIndex + 3] = i;
                triangles[triIndex + 4] = i + 1;
                triangles[triIndex + 5] = i + resolution + 1;
                triIndex += 6;
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs; // Set UVs
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}