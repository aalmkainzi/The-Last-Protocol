using UnityEngine;
using UnityEngine.Video;

public class PlayIntro : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        // Get the VideoPlayer component attached to the GameObject
        videoPlayer = GetComponent<VideoPlayer>();

        // Make sure the video is not playing at start
        videoPlayer.Stop();

        // Subscribe to the event that is called when the video finishes
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        if(videoPlayer.isPlaying && Input.anyKeyDown)
        {
            Debug.Log("SKIPEPED? ? ? ? ?");
            OnVideoComplete();
        }
    }

    // Method to start playing the video
    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    // This method will be called when the video finishes playing
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Call your custom method here (e.g., trigger the next scene, show a UI, etc.)
        OnVideoComplete();
    }

    // Example of the custom method to be called when the video finishes
    private void OnVideoComplete()
    {
        GameObject.Find("CC").GetComponent<MenuController>().StartCoroutine(
            GameObject.Find("CC").GetComponent < MenuController > ().FadeAndLoadScene("Level1"));
    }
}
