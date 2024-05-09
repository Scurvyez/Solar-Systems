using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    private static GameSpeedController _instance;

    public Button[] buttons;
    public float curSpeed = 1f; // Initialize curSpeed to 1 by default

    public float speed1 = 0f;
    public float speed2 = 0.005f;
    public float speed3 = 0.25f;
    public float speed4 = 1f;
    public float speed5 = 15f;
    public float speed6 = 40f;
    public float speed7 = 75f;
    public float speed8 = 75f;

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
            "Pause" => speed1,
            "Slowww" => speed2,
            "Quarter" => speed3,
            "Half" => speed4,
            "Normal" => speed5,
            "Fast" => speed6,
            "Double" => speed7,
            "Warp Speed" => speed8,
            _ => throw new System.ArgumentException($"Unknown speed name '{button.name}'")
        };
        return speed;
    }
}
