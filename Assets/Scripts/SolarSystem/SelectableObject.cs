using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SelectionRing;
    private readonly float newX = 90.0f;
    private bool isSelected = false;
    
    private void Start()
    {
        SelectionRing.transform.localRotation = Quaternion.Euler(newX, SelectionRing.transform.localRotation.y, SelectionRing.transform.localRotation.z);
        var sP = transform.localScale;
        SelectionRing.transform.localScale = new Vector3(sP.x * 2.5f, sP.y * 2.5f, sP.z * 2.5f);
    }

    private void IsSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isSelected = true;
                }
                else
                {
                    isSelected = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            isSelected = false;
        }
    }

    private void Update()
    {
        IsSelected();
        SelectionRing.SetActive(isSelected);
    }
}
