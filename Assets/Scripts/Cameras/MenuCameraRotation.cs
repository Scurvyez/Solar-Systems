using UnityEngine;

namespace Cameras
{
    public class MenuCameraRotation : MonoBehaviour
    {
        public Camera Camera;
        public float RotationSpeed = 10f;

        private void Update()
        {
            Camera.transform.Rotate(Vector3.forward, -RotationSpeed * Time.deltaTime);
        }
    }
}