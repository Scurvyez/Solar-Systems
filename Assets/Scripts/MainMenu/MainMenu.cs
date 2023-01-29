using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;
    public float fadeDuration = 2f;

    public void NewSimulation()
    {
        StartCoroutine(FadeOutButtons());
    }

    public IEnumerator FadeOutButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            CanvasGroup canvasGroup = buttons[i].GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = buttons[i].gameObject.AddComponent<CanvasGroup>();

            float currentTime = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, 0f, currentTime / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }
        // Wait for a bit before loading the next scene
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitSimulation()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
