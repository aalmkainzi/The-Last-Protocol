using UnityEngine;

public class PlaySingleAnimation : MonoBehaviour
{
    public Animation animationComponent; // مكون Animation
    public AnimationClip animationClip; // Animation Clip الذي تريد تشغيله

    void Start()
    {
        if (animationComponent == null)
        {
            Debug.LogError("Animation Component is not assigned. Please assign it in the Inspector.");
            return;
        }

        if (animationClip == null)
        {
            Debug.LogError("Animation Clip is not assigned. Please assign it in the Inspector.");
            return;
        }

        // التأكد من أن الكليب مضاف إلى Animation Component
        if (animationComponent.GetClip(animationClip.name) == null)
        {
            animationComponent.AddClip(animationClip, animationClip.name);
        }
    }

    void Update()
    {
        // عند الضغط على مفتاح معين (مثل G)
        if (Input.GetKeyDown(KeyCode.G))
        {
            animationComponent.Play(animationClip.name);
        }
    }

}