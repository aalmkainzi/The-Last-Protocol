using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Text progressText;   // Optional: UI text to show percentage

    private void Start()
    {
        // Start the loading process with a fixed duration of 3 seconds
        StartCoroutine(LoadSceneWithDelay("UI", 3f));  // Replace "GameScene" with your scene name
    }

    IEnumerator LoadSceneWithDelay(string sceneName, float delayTime)
    {
        // Begin loading the scene asynchronously, but prevent it from auto-activating
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        // While the scene is loading
        while (elapsedTime < delayTime)
        {
            elapsedTime += Time.deltaTime;

            // Get the loading progress (clamped between 0 and 1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the progress text (optional)
            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;  // Wait for the next frame
        }

        // After 3 seconds, allow the scene to activate
        operation.allowSceneActivation = true;
    }
}