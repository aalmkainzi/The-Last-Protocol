using UnityEngine;

public class Rycast_game : MonoBehaviour
{

    public Transform firePoint;
    public float range = 90000f;
    // public LineRenderer laserLine;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public Player player;
    int layerMask;

    public LineRenderer myLine;
    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("EnemyBot");
        Debug.Log("LAYER MASK " + layerMask);
        player = transform.parent.gameObject.GetComponent<Player>();
    }
    void Update()
    {
        // Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red);

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        RaycastHit hit;
        //Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red);
        // myLine.SetPositions(new Vector3[] { firePoint.position, firePoint.position + firePoint.forward * 10 });
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range,  layerMask))
        {
            /*            laserLine.enabled = true;
                        laserLine.SetPosition(0, firePoint.position);
                        laserLine.SetPosition(1, hit.point);
            */
            Debug.Log("FIRED AT ENEMY");
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.TakeDamage(player.power);
        }
        else
        {
/*            laserLine.enabled = true;
            laserLine.SetPosition(0, firePoint.position);
            laserLine.SetPosition(1, firePoint.position + firePoint.forward * range);
*/        }

        Invoke("DisableLaser", 0.1f);
    }

    void DisableLaser()
    {
        // laserLine.enabled = false;
    }
}
