using UnityEngine;

namespace SolarSystemUI
{
    public class UI_BillboardEffect : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        public Vector3 PositionalOffset = new (2.0f, 0.0f, 0.0f);
        
        private GameObject _previousSelectedObject;
        private Vector3 _anchoredPosition;

        private void Update()
        {
            if (SystemCamera is null) return;
            GameObject selectedObject = GetSelectedGameObject();

            if (selectedObject is not null)
            {
                if (_previousSelectedObject is not null && selectedObject != _previousSelectedObject)
                {
                    _previousSelectedObject = selectedObject;
                }
                UpdateCanvasPosition(selectedObject);
            }

            // Always update the rotation to face the camera
            UpdateCanvasRotation();
        }

        private void UpdateCanvasPosition(GameObject selectedObject)
        {
            Vector3 worldOffset = SystemCamera.transform.TransformVector(PositionalOffset);
            _anchoredPosition = selectedObject.transform.position + worldOffset;
            transform.position = _anchoredPosition;
        }

        private void UpdateCanvasRotation()
        {
            if (SystemCamera is null) return;
            transform.LookAt(transform.position + SystemCamera.transform.rotation * Vector3.forward, SystemCamera.transform.rotation * Vector3.up);
        }

        private GameObject GetSelectedGameObject()
        {
            return SystemCamera?.SelectedObject;
        }
    }
}