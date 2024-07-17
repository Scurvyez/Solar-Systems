using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SolarSystemUI
{
    public class UI_SystemGrid : MonoBehaviour
    {
        public bool GridVisible = false; // Set to false by default
        public Button ShowGridButton;
        public MeshFilter MeshFilter;
        public Vector2Int GridSize;
        public float GridSpacing;
        public Color LineColor;
        public bool DEBUG_UpdateGrid;
        
        [Range(0, 1)]
        public float LineFadeStart = 0.45f;
        [Range(0, 1)]
        public float LineFadeEnd = 0.55f;

        public float FadeRadius = 5.0f; // The radius at which the fade effect starts
        public float FadeStrength = 0.5f; // The amount by which the alpha value decreases

        private Mesh _mesh;
        private List<Vector3> _vertices;
        private List<int> _indices;
        private List<Color> _colors;

        private void Start() 
        {
            _mesh = new Mesh();
            MeshRenderer meshRenderer = MeshFilter.GetComponent<MeshRenderer>();
            meshRenderer.material.color = LineColor;
            ShowGridButton.onClick.AddListener(ToggleGridVisibility);
            
            BuildGrid();

            // Set grid visibility based on the GridVisible flag
            meshRenderer.enabled = GridVisible;
            
            if (!DEBUG_UpdateGrid) return;
            Debug.LogWarning($"System Grid Updating Every Frame...");
        }

        private void Update()
        {
            // rebuilding the grid is expensive, so uh... yeah...
            // throw a log message as a reminder, so we don't spend hours tracking down why we lost ~300 fps again xD
            if (!DEBUG_UpdateGrid) return;
            BuildGrid();
        }

        private void BuildGrid() 
        {
            _vertices = new List<Vector3>();
            _indices = new List<int>();
            _colors = new List<Color>();

            float xMin = GridSpacing * GridSize.x / 2f;
            float zMin = GridSpacing * GridSize.y / 2f;
            Vector3 gridCenter = new Vector3(0, 0, 0); // Assuming the grid is centered at (0,0,0)

            for (int i = 0; i <= GridSize.x; i++)
            {
                for (int j = 0; j <= GridSize.y; j++)
                {
                    float x1 = i * GridSpacing - xMin;
                    float x2 = (i + 1) * GridSpacing - xMin;
                    float z1 = j * GridSpacing - zMin;
                    float z2 = (j + 1) * GridSpacing - zMin;

                    if (i != GridSize.x)
                    {
                        AddLineSegment(new Vector3(x1, 0, z1), new Vector3(x2, 0, z1), gridCenter);
                    }

                    if (j != GridSize.y)
                    {
                        AddLineSegment(new Vector3(x1, 0, z1), new Vector3(x1, 0, z2), gridCenter);
                    }
                }
            }

            _mesh.vertices = _vertices.ToArray();
            _mesh.SetIndices(_indices.ToArray(), MeshTopology.Lines, 0);
            _mesh.colors = _colors.ToArray();
            MeshFilter.mesh = _mesh;
        }

        private void AddLineSegment(Vector3 start, Vector3 end, Vector3 center)
        {
            int startIndex = _vertices.Count;

            Vector3 fadeStartPoint = Vector3.Lerp(start, end, LineFadeStart);
            Vector3 fadeEndPoint = Vector3.Lerp(start, end, LineFadeEnd);

            AddVertexWithFading(start, center, LineColor.a);
            AddVertexWithFading(fadeStartPoint, center, 0);
            AddVertexWithFading(fadeStartPoint, center, 0);
            AddVertexWithFading(fadeEndPoint, center, 0);
            AddVertexWithFading(fadeEndPoint, center, 0);
            AddVertexWithFading(end, center, LineColor.a);

            _indices.Add(startIndex);
            _indices.Add(startIndex + 1);
            _indices.Add(startIndex + 2);
            _indices.Add(startIndex + 3);
            _indices.Add(startIndex + 4);
            _indices.Add(startIndex + 5);
        }

        private void AddVertexWithFading(Vector3 vertex, Vector3 center, float segmentAlpha)
        {
            float distanceFromCenter = Vector3.Distance(vertex, center);
            float alpha = segmentAlpha;

            if (distanceFromCenter > FadeRadius)
            {
                alpha = Mathf.Max(0, alpha - FadeStrength * (distanceFromCenter - FadeRadius));
            }

            _vertices.Add(vertex);
            _colors.Add(new Color(LineColor.r, LineColor.g, LineColor.b, alpha));
        }

        private void ToggleGridVisibility()
        {
            GridVisible = !GridVisible;
            MeshFilter.GetComponent<MeshRenderer>().enabled = GridVisible;
        }
    }
}
