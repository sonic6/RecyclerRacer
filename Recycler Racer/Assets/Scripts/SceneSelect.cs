using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Load scene")
            StartCoroutine(WaitBecauseTheLoadingSceneIsCuteSoYouHaveToWatchItEvenIfTheComputerIsVeryFast());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    IEnumerator WaitBecauseTheLoadingSceneIsCuteSoYouHaveToWatchItEvenIfTheComputerIsVeryFast()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Kartscene 02");
    }
    
}
