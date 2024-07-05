using SolarSystem;
using TMPro;
using UnityEngine;

namespace SolarSystemUI
{
    public class PlanetInfo_UI : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        
        [Header("Planet Info UI Elements")]
        public TextMeshProUGUI PlanetNameText;
        public TextMeshProUGUI PlanetMassText;
        public TextMeshProUGUI PlanetRadiusText;
        
        private TextMeshProUGUI[] _textElements;
        
        private void Start()
        {
            PopulateTextElementsArray();
        }
        
        private void Update()
        {
            GameObject selectedPlanet = GetSelectedPlanet();
            if (selectedPlanet?.GetComponent<PlanetInfo>() is not null)
            {
                ShowUIElements();
                SetSavedPlanetValues(selectedPlanet);
            }
            else
            {
                HideUIElements();
            }
        }
        
        private void SetSavedPlanetValues(GameObject selectedPlanet)
        {
            PlanetInfo planetInfo = selectedPlanet.GetComponent<PlanetInfo>();
            
            if (planetInfo is null) return;
            
            PlanetNameText.text = "<color=#ff8f8f>Name:</color> " + planetInfo.Name;
            PlanetMassText.text = "<color=#ff8f8f>Mass:</color> " + planetInfo.Mass;
            PlanetRadiusText.text = "<color=#ff8f8f>Radius:</color> " + planetInfo.Radius;
        }

        private GameObject GetSelectedPlanet()
        {
            return SystemCamera?.SelectedObject;
        }
        
        private void PopulateTextElementsArray()
        {
            _textElements = new [] 
            { 
                PlanetNameText, // 0
                PlanetMassText, // 1
                PlanetRadiusText, // 2
            };
        }
        
        private void ShowUIElements()
        {
            foreach (TextMeshProUGUI textElement in _textElements)
            {
                textElement.gameObject.SetActive(true);
            }
        }

        private void HideUIElements()
        {
            foreach (TextMeshProUGUI textElement in _textElements)
            {
                textElement.gameObject.SetActive(false);
            }
        }
    }
}
