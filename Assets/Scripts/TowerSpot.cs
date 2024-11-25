using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    Tower placedTower;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void PlaceTower(Tower prefab)
    {
        if(placedTower == null)
        {
            GameObject newTower = Instantiate(prefab.gameObject, transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // make layer only player
        // display a transparent area showing the TowerSpot
    }
}
