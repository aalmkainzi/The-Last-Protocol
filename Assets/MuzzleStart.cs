using UnityEngine;

public class MuzzleStart : MonoBehaviour
{
    public float muzzleTime;

    void Start()
    {
        Invoke(nameof(PlayMuzzleFlash), muzzleTime + Random.Range(0f, 0.5f));
    }

    void PlayMuzzleFlash()
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Play(true);
    }
}
