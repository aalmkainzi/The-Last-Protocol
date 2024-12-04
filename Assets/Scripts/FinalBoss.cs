using UnityEngine;

public class FinalBoss : Enemy
{
    GameObject particle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        particle = transform.Find("Particle").gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        GetComponent<AudioSource>().clip = gameplayManager.booms[Random.Range(0, gameplayManager.booms.Length)];
        GetComponent<AudioSource>().Play();
        animator.StopPlayback();
        particle.SetActive(true);
        base.Die();
        //DestroyBoss();
        Invoke(nameof(DestroyBoss), 1.0f);
    }

    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}
