using UnityEngine;
using UnityEngine.UI;

public class PlayAnimationOnG : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "Play"; // اسم Trigger في Animator
    public Image targetImage; 
    public AudioSource firstSound; // الصوت الأول
    public AudioSource secondSound; // الصوت الثاني
    private bool isImageVisible = false; 

    void Start()
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(false); // إخفاء الصورة في البداية
        }

        if (firstSound == null || secondSound == null)
        {
            // Debug.LogWarning("Please assign both sounds in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            if (animator != null)
            {
                animator.SetTrigger(triggerName);
                // // Debug.Log("Trigger activated: Play");

         
                if (firstSound != null)
                {
                    firstSound.Play();
                    // // Debug.Log("First sound played.");
                }

            
                if (secondSound != null)
                {
                    StartCoroutine(PlaySecondSoundAfterDelay(3f));
                }

               
                if (targetImage != null && !isImageVisible)
                {
                    StartCoroutine(ShowImageWithDelay());
                }
            }
            else
            {
                // Debug.LogError("Animator is not assigned. Please assign it in the Inspector.");
            }
        }
    }

    private System.Collections.IEnumerator PlaySecondSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        secondSound.Play(); 
        // Debug.Log("Second sound played after delay.");
    }

    private System.Collections.IEnumerator ShowImageWithDelay()
    {
        yield return new WaitForSeconds(3); 
        targetImage.gameObject.SetActive(true); 
        isImageVisible = true;

        // Debug.Log("Image is now visible.");

        yield return new WaitForSeconds(80);
        targetImage.gameObject.SetActive(false); 
        isImageVisible = false;

        // Debug.Log("Image is now hidden.");
    }
}