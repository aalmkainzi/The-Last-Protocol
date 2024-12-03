using UnityEngine;

public class TowerTile : MonoBehaviour
{
    Tower placedTower;
    GameplayManager gameplayManager;
    GameObject cube;
    void Start()
    {
        cube = transform.GetChild(0).gameObject;
        gameplayManager = GameObject.FindWithTag("gameplayManager").GetComponent<GameplayManager>();
    }

    void Update()
    {
    }

    public GameObject PlaceTower(Tower prefab)
    {
        if(placedTower == null)
        {
            GameObject newTower = Instantiate(prefab.gameObject, transform);
            placedTower = newTower.GetComponent<Tower>();
            cube.SetActive(false);
            return newTower;
        }
        return null;
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
