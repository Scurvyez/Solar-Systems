using UnityEngine;
using UnityEngine.UI;

namespace SolarSystemUI
{
    public class UI_DebugMarkers : MonoBehaviour
    {
        public bool ShowAxialTiltMarkers = true;
        public bool ShowSpinDirectionMarkers = true;
        
        public Button ToggleAxialTiltButton;
        public Button ToggleSpinDirectionButton;

        private void Start()
        {
            if (ToggleAxialTiltButton != null)
                ToggleAxialTiltButton.onClick.AddListener(ToggleAxialTiltMarkers);

            if (ToggleSpinDirectionButton != null)
                ToggleSpinDirectionButton.onClick.AddListener(ToggleSpinDirectionMarkers);
        }

        private void ToggleAxialTiltMarkers()
        {
            ShowAxialTiltMarkers = !ShowAxialTiltMarkers;
        }

        private void ToggleSpinDirectionMarkers()
        {
            ShowSpinDirectionMarkers = !ShowSpinDirectionMarkers;
        }
    }
}