using UnityEngine;


public class PlanetMesh : MonoBehaviour
{
    [Range(2, 100)] public int resolution = 96;

    public MeshFilter meshFilter;
    private PlanetSphere[] planetSpheres;

    public void Awake()
    {
        Initialize();
        GenerateMesh();
    }

    private void Initialize()
    {
        if (meshFilter == null)
        {
            GameObject meshObject = new("CombinedMesh")
            {
                transform = { parent = transform }
            };
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();
        }
        
        planetSpheres = new PlanetSphere[6];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            Mesh tempMesh = new Mesh();
            planetSpheres[i] = new PlanetSphere(tempMesh, resolution, directions[i]);
        }
    }

    private void GenerateMesh()
    {
        // Combine all meshes into one mesh
        CombineMeshes();
    }

    private void CombineMeshes()
    {
        Mesh combinedMesh = new ();

        int verticesCount = resolution * resolution * 6;
        Vector3[] vertices = new Vector3[verticesCount];
        Vector2[] uvs = new Vector2[verticesCount];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6 * 6];
        int vertIndex = 0;
        int triIndex = 0;

        foreach (PlanetSphere planetSphere in planetSpheres)
        {
            planetSphere.ConstructMesh();

            Vector3[] sphereVertices = planetSphere.Mesh.vertices;
            Vector2[] sphereUVs = planetSphere.Mesh.uv;
            int[] sphereTriangles = planetSphere.Mesh.triangles;

            for (int i = 0; i < sphereVertices.Length; i++)
            {
                vertices[vertIndex] = sphereVertices[i];
                uvs[vertIndex] = sphereUVs[i];
                vertIndex++;
            }

            for (int i = 0; i < sphereTriangles.Length; i++)
            {
                triangles[triIndex] = sphereTriangles[i] + vertIndex - sphereVertices.Length;
                triIndex++;
            }
        }

        combinedMesh.vertices = vertices;
        combinedMesh.uv = uvs;
        combinedMesh.triangles = triangles;
        combinedMesh.RecalculateNormals();

        meshFilter.sharedMesh = combinedMesh;
    }
}