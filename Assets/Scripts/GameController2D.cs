using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController2D : MonoBehaviour
{
    [SerializeField] private GameObject[] Hazards;
    [SerializeField] private Pickup2D[] PickUps;

    [SerializeField] private Transform HazardsParent;
    [SerializeField] private Transform PickUpsParent;

    [SerializeField, Range(1f, 10f)] private float SpawnTimerHazards;
    [SerializeField, Range(1f, 10f)] private float SpawnTimerPickups;

    [SerializeField, Range(1f, 5f)] private int numOfSpawnsHazards;
    [SerializeField, Range(1f, 5f)] private int numOfSpawnsPickups;

    [SerializeField] private Image Background;

    private PlayerController _playerControllerRef;

    private float spawnTimerH;
    private float spawnTimerP;

    [System.NonSerialized] public bool canSpawn;

    private int mapWidth;
    private int mapHeight;
    private int multiplier = 1;

    void Start()
    {
        spawnTimerH = SpawnTimerHazards;
        spawnTimerP = SpawnTimerPickups;

        mapWidth = (int)(Background.rectTransform.rect.width / 2) + 150;
        mapHeight = (int)(Background.rectTransform.rect.height / 2) + 125;

        if (_playerControllerRef == null) { _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            if (_playerControllerRef.numberOfCrabs_2D > 10) { multiplier += 3; }
            else { multiplier = 1; }

            spawnTimerH -= multiplier * Time.deltaTime;
            spawnTimerP -= Time.deltaTime;

            if (spawnTimerH < 0)
            {
                for (int i = 0; i < numOfSpawnsPickups; i++) { SpawnRandomHazard(); }
                spawnTimerH = SpawnTimerHazards;
            }

            if (spawnTimerP < 0)
            {
                for (int i = 0; i < numOfSpawnsPickups; i++) { SpawnPickup(); }
                spawnTimerP = SpawnTimerPickups;
            }
        }
    }

    private void SpawnRandomHazard()
    {
        GameObject _hazard = Instantiate(Hazards[Random.Range(0, Hazards.Length)], HazardsParent);
        Hazard newHazard = _hazard.GetComponent<Hazard>();

        int side = Random.Range(0, 2) * 2 - 1;

        Vector3 spawnPos = new Vector3(Random.Range((mapWidth * side), mapWidth * -side), Random.Range((mapHeight * side), mapHeight * -side), 0f);

        if (newHazard.ID == 0)
        {
            spawnPos.x = mapWidth * side;
            newHazard.SetTargetPosition(-spawnPos);
        }

        if (newHazard.ID == 2)
        {
            newHazard.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 90) * side);
        }

        newHazard._playerControllerRef = _playerControllerRef;
        newHazard.SetStartPosition(spawnPos);
    }

    private void SpawnPickup()
    {
        Pickup2D pickup = Instantiate(PickUps[0], PickUpsParent);
        int side = Random.Range(0, 2) * 2 - 1;
        Vector3 spawnPos = new Vector3(Random.Range(mapWidth * side, mapWidth * -side) , Random.Range((mapHeight * side), mapHeight * -side), 0f);

        pickup.SetStartPosition(spawnPos);
    }
}
