using UnityEngine;
using UnityEngine.SceneManagement;

public class loadsc2 : MonoBehaviour
{
    public string Level1_Redo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    public void loadScene()
    {
        SceneManager.LoadScene(Level1_Redo);
    }
    void Start()
    {
        SceneManager.LoadScene(Level1_Redo);
    }
    // Update is called once per frame
   
}
