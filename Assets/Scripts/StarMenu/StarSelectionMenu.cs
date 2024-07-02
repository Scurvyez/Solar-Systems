using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarSelectionMenu : MonoBehaviour
{
    public Button targetButton;
    public float pulseSpeed = 1.0f;
    public float pulseAmount = 0.1f;
    public float pulseDuration = 2.0f;
    private Vector3 originalScale;
    private bool pulsing = false;

    private void Start()
    {
        originalScale = targetButton.transform.localScale;
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Forward()
    {
        string dataPath = Application.persistentDataPath;
        string saveFilePath = dataPath + "/" + SaveManager.instance.activeSave.saveName + ".xml";

        if (System.IO.File.Exists(saveFilePath))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            StartPulsing();
        }
    }

    private void StartPulsing()
    {
        pulsing = true;
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        float t = 0;
        float pulseStartTime = Time.time;

        while (pulsing)
        {
            if (Time.time - pulseStartTime >= pulseDuration)
            {
                pulsing = false;
                targetButton.transform.localScale = originalScale;
                break;
            }

            t += Time.deltaTime * pulseSpeed;
            float scale = Mathf.Sin(t) * pulseAmount + 1;
            targetButton.transform.localScale = originalScale * scale;

            yield return null;
        }
    }
}
