using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Debug.Log("MAIN MENU TIME");
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(int level)
    {
        Debug.Log("MAIN LEVEL" + level + " TIME");
        SceneManager.LoadScene("Level" + level);
    }
}
