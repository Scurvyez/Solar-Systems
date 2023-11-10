using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    private static GameSpeedController _instance;

    public Button[] buttons;
    public float curSpeed = 1f; // Initialize curSpeed to 1 by default

    public static GameSpeedController Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance is null, try to find it in the scene
                _instance = FindObjectOfType<GameSpeedController>();

                // If it's still null, create a new instance
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameSpeedController");
                    _instance = singletonObject.AddComponent<GameSpeedController>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // Set curSpeed to 1 when the script instance is being loaded
        curSpeed = 1f;
    }

    public void SetGameSpeed(Button button)
    {
        curSpeed = CalculateGameSpeed(button);
    }

    public float CalculateGameSpeed(Button button)
    {
        float speed = button.name switch
        {
            "Pause" => 0.0f,
            "Slowww" => 0.005f,
            "Quarter" => 0.25f,
            "Half" => 0.5f,
            "Normal" => 1f,
            "Fast" => 1.5f,
            "Double" => 2f,
            "Warp Speed" => 50.0f,
            _ => throw new System.ArgumentException($"Unknown speed name '{button.name}'")
        };
        return speed;
    }
}
