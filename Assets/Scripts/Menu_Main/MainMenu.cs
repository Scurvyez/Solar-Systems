using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class MainMenu : MonoBehaviour
{
    public void NewSimulation()
    {
        StartCoroutine(DelayedAction(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void QuitSimulation()
    {
        Application.Quit();
    }
    
    private static IEnumerator DelayedAction(int sceneIndex)
    {
        yield return new WaitForSeconds(ConstantsUtil.BUTTON_ACTION_DELAY_SOUND);
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(ConstantsUtil.BUTTON_ACTION_DELAY_SOUND);
        Application.Quit();
    }
}
