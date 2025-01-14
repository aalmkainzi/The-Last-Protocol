using Unity.VisualScripting;
using UnityEngine;
using PrimeTween;
using UnityEngine.UI;
public class BallAttack : MonoBehaviour
{
    public float targetScale;
    public float animTime;
    AudioSource boomAudio;
    public RawImage white;
    private void Start()
    {
        Invoke(nameof(PlayAnim), animTime);
    }

    void PlayAnim()
    {
        boomAudio = GetComponent<AudioSource>();
        Tween.Scale(transform, targetScale, 3.5f).OnComplete(LaunchBall);
    }

    void LaunchBall()
    {
        Tween.LocalPositionX(transform, transform.localPosition.x + 4.0f, 0.2f).OnComplete(Boom);
    }

    void Boom()
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Play(true);
        boomAudio.Play();
        FadeToWhite();
    }

    void FadeToWhite()
    {
        Tween.Custom(white.color.a, 1.0f, 0.5f, (newVal) =>
        {
            Color c = white.color;
            c.a = newVal;
            white.color = c;
        }).OnComplete(()=>CutsceneManager.instance.GoToScene("Level1_Redo"));
    }
}
