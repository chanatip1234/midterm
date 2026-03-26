using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource clickSource;
    public void NextStage()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevelRoutine("midterm 2"));
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator LoadLevelRoutine(string sceneName)
    {
        if (clickSource != null)
        {
            clickSource.Play();
            
            yield return new WaitForSecondsRealtime(0.15f);
        }

        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
