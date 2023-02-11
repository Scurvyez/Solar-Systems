using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPlacementRings : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        GenerateSystemPlacementRings();
    }

    public void GenerateSystemPlacementRings()
    {
        int numRings = 1;
        float baseScale = target.transform.localScale.x * 0.785f;

        for (int i = 0; i < numRings; i++)
        {
            GameObject ring = new GameObject("Ring");
            ring.transform.position = target.transform.position;
            ring.transform.parent = target.transform;

            SpriteRenderer renderer = ring.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>("Sprites/SystemPlacementRingsTexture");

            ring.transform.localScale = new Vector3(baseScale + i, baseScale + i, 1);
        }
    }
}
