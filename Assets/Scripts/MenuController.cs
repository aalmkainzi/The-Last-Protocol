using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public CanvasGroup mainMenuGroup;    // The main menu panel
    public GameObject levelSelectionPanel; // The level selection panel
    public float fadeDuration = 1f;     // Fade duration in seconds
    public CanvasGroup fadeCanvasGroup; // CanvasGroup of the fade panel
    public GameObject optionsPanel; 
    public Slider SoundSlider;
    // public AudioSource audioSource;
    private float currentSound;


    void Start()
    {
        currentSound = PlayerPrefs.GetFloat("GameVolume", 0.5f);
        // audioSource.volume = currentSound;
        SoundSlider.value = currentSound;

        // Listen to slider value changes
        SoundSlider.onValueChanged.AddListener(HandleVolumeChanged);
    }

    public void HandleVolumeChanged(float value)
    {
        currentSound = value;
        // audioSource.volume = currentSound;
        PlayerPrefs.SetFloat("GameVolume", currentSound);

    }
    public void PlayButtonClicked()
    {
        StartCoroutine(FadeOutMenu());
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
    private IEnumerator FadeOutMenu()
    {
        // float startAlpha = mainMenuGroup.alpha;
        // float time = 0;
        //
        // // Fading the main menu out
        // while (time < fadeDuration)
        // //{
        //     mainMenuGroup.alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
        //     time += Time.deltaTime;
        yield return null;
        // }
        mainMenuGroup.alpha = 0;
        mainMenuGroup.interactable = false;

        // Activate the level selection panel after the fade-out
        levelSelectionPanel.SetActive(true);
    }

    public void OptionsButtonClicked()
    {
        mainMenuGroup.alpha = 0;
        mainMenuGroup.interactable = false;
        optionsPanel.SetActive(true);
    }

    public void BackButtonClicked()
    {
        
        levelSelectionPanel.SetActive(false);
        float startAlpha = mainMenuGroup.alpha;
        float time = 0;
        
        // Fading the main menu out
        while (time < fadeDuration)
        {
            mainMenuGroup.alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            time += Time.deltaTime;
        }
        mainMenuGroup.alpha = 1;
        mainMenuGroup.interactable = true;

        // Activate the level selection panel after the fade-out
        // levelSelectionPanel.SetActive(true);
        // mainMenuGroup.interactable = true;
        // mainMenuGroup.alpha = 1;
    }
    
    public void BackButton2Clicked()
    {
        
        optionsPanel.SetActive(false);
        float startAlpha = mainMenuGroup.alpha;
        float time = 0;
        
        // Fading the main menu out
        while (time < fadeDuration)
        {
            mainMenuGroup.alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            time += Time.deltaTime;
        }
        mainMenuGroup.alpha = 1;
        mainMenuGroup.interactable = true;
    
        // Activate the level selection panel after the fade-out
        // levelSelectionPanel.SetActive(true);
        // mainMenuGroup.interactable = true;
        // mainMenuGroup.alpha = 1;
    
        
    
    }
    
    public void LoadLevel1()
    {
        SceneManager.LoadScene("FirstCutscene");

        FadeAndLoadScene("FirstCutscene");
        // GameObject.Find("VidPlay").GetComponent<PlayIntro>().PlayVideo();
        GetComponent<Canvas>().enabled = false;
    }

    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Start fading out
        yield return StartCoroutine(Fade(1f));

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
  

    // Coroutine to fade the CanvasGroup in and out
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
    
}