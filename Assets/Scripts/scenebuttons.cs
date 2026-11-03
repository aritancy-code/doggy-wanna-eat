using UnityEngine;
using UnityEngine.SceneManagement; // this helps to load levels

// add this component to an object in scene (canvas)
// then wire buttons with On Click to the correspondign functions
public class scenebuttons : MonoBehaviour
{
    public string homeSceneName = "TUTORIAL"; // the scene the home button goes to (change to menu scene later)

    public void Replay() // restart this level from the beginning
    {
        Time.timeScale = 1f; // unpause, the ending screens pause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel() // go to the next level in the Scene List
    {
        Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex + 1;

        if (next < SceneManager.sceneCountInBuildSettings) // is there another level?
        {
            SceneManager.LoadScene(next);
        }
        else // that was the last level
        {
            SceneManager.LoadScene(homeSceneName);
        }
    }

    public void Home() // go back to the home scene
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(homeSceneName);
    }
}