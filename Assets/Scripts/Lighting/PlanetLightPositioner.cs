using SolarSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Lighting
{
    public class PlanetLightPositioner : MonoBehaviour
    {
        public Transform Star;
    
        private float _distanceFromPlanet;
        private Light _lightSource;
        private PlanetInfo _planetInfo;
    
        private void Start()
        {
            _distanceFromPlanet = transform.parent.localScale.x + 2f;
            _lightSource = transform.AddComponent<Light>();
            _lightSource.type = LightType.Point;
            _lightSource.color = Color.white;
            _lightSource.intensity = 25f;
            _lightSource.range = 50f;
            _planetInfo = transform.parent.GetComponent<PlanetInfo>();

        
            // Log the _planetInfo and its LayerName property
            if (_planetInfo != null)
            {
                Debug.Log($"PlanetInfo: {_planetInfo}");
                Debug.Log($"PlanetInfo LayerName: {_planetInfo.LayerName}");
                _lightSource.cullingMask = LayerMask.NameToLayer(_planetInfo.LayerName);
            }
            else
            {
                Debug.LogWarning("PlanetInfo component not found on parent object.");
            }
        
            if (_lightSource == null)
            {
                Debug.LogWarning("LightSource is not assigned.");
            }
        }
    
        private void FixedUpdate()
        {
            Vector3 directionToStar = (Star.position - transform.position).normalized;
            _lightSource.transform.position = transform.position + directionToStar * _distanceFromPlanet;
            _lightSource.transform.LookAt(transform);
        }
    }   
}
