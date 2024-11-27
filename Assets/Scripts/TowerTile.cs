using UnityEngine;

public class TowerTile : MonoBehaviour
{
    Tower placedTower;
    GameplayManager gameplayManager;
    void Start()
    {
        gameplayManager = GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>();
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

    private void OnTriggerStay(Collider other)
    {
        if(placedTower == null && gameplayManager.currentTowerTile == null)
        {
            gameplayManager.currentTowerTile = this;
        }
        // make layer only player
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameplayManager.currentTowerTile == this)
        {
            gameplayManager.currentTowerTile = null;
        }
    }
}
