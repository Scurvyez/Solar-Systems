using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public GameObject planetPrefab;
    public GameObject starPrefab;

    public void Start()
    {
        for (int i = 0; i < SaveManager.instance.activeSave.rockyPlanets.Count - 1; i++)
        {
            // create a new rocky planet object
            RockyPlanet p = new ();

            // instantiate the rocky planet prefab and set its properties
            GameObject planetObj = Instantiate(planetPrefab);
            planetObj.name = SaveManager.instance.activeSave.starSystemName + Random.Range(1, SaveManager.instance.activeSave.rockyPlanets.Count);
            planetObj.transform.localScale = Vector3.one * p.Radius * 2f;
            planetObj.transform.localPosition = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(0, 0, Random.Range(100f, 1000f));

            planetObj.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
