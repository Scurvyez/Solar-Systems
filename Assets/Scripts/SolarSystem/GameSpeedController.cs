using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class GameSpeedController : MonoBehaviour
{
    public Button[] Buttons;
    public float CurSpeed;

    public float Speed1;
    public float Speed2 = 0.01f;
    public float Speed3 = 0.25f;
    public float Speed4 = 0.5f;
    public float Speed5 = 1f;
    public float Speed6 = 50f;
    public float Speed7 = 100f;
    public float Speed8 = 200f;
    
    private static GameSpeedController _instance;
    private float _transitionDuration = 3.0f;
    
    public static GameSpeedController Instance
    {
        get
        {
            if (_instance is not null) return _instance;
            _instance = FindObjectOfType<GameSpeedController>();

            if (_instance is not null) return _instance;
            GameObject singletonObject = new ("GameSpeedController");
            _instance = singletonObject.AddComponent<GameSpeedController>();

            return _instance;
        }
    }

    private void Awake()
    {
        // Set curSpeed to 1 when the script instance is being loaded
        CurSpeed = Speed5;
    }

    public void SetGameSpeed(Button button)
    {
        float targetSpeed = CalculateGameSpeed(button);
        StopAllCoroutines(); // Stop any existing transition
        StartCoroutine(SmoothTransition(targetSpeed));
    }

    private float CalculateGameSpeed(Button button)
    {
        float speed = button.name switch
        {
            "Pause" => Speed1,
            "Slowww" => Speed2,
            "Quarter" => Speed3,
            "Half" => Speed4,
            "Normal" => Speed5,
            "Fast" => Speed6,
            "Double" => Speed7,
            "Warp Speed" => Speed8,
            _ => throw new System.ArgumentException($"Unknown speed name '{button.name}'")
        };
        return speed;
    }
    
    private IEnumerator SmoothTransition(float targetSpeed)
    {
        float startSpeed = CurSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _transitionDuration;
            CurSpeed = Mathf.Lerp(startSpeed, targetSpeed, EasingFunctionsUtil.EaseInOutCubic(t));
            yield return null;
        }
        CurSpeed = targetSpeed; // Ensure the exact target speed is set at the end
    }
}
