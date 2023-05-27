using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController2D : MonoBehaviour
{
    [SerializeField] private Hazard[] Hazards;
    [SerializeField] private Pickup2D[] PickUps;

    [SerializeField] private Transform HazardsParent;
    [SerializeField] private Transform PickUpsParent;

    [SerializeField, Range(1f, 10f)] private float SpawnTimerHazards;
    [SerializeField, Range(1f, 10f)] private float SpawnTimerPickups;

    [SerializeField] private Image Background;

    private PlayerController _playerControllerRef;

    private float spawnTimerH;
    private float spawnTimerP;

    public bool canSpawn;
    private int mapWidth;
    private int mapHeight;

    void Start()
    {
        spawnTimerH = SpawnTimerHazards;
        spawnTimerP = SpawnTimerPickups;

        mapWidth = (int)(Background.rectTransform.rect.width / 2) - 150;
        mapHeight = (int)(Background.rectTransform.rect.height / 2) - 150;

        if (_playerControllerRef == null) { _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            spawnTimerH -= Time.deltaTime;
            spawnTimerP -= Time.deltaTime;

            if (spawnTimerH < 0)
            {
                SpawnRandomHazard();
                spawnTimerH = SpawnTimerHazards;
            }

            if (spawnTimerP < 0)
            {
                SpawnPickup();
                spawnTimerP = SpawnTimerPickups;
            }
        }
    }

    private void SpawnRandomHazard()
    {
        Hazard newHazard = Instantiate(Hazards[Random.Range(0, Hazards.Length)], HazardsParent);
        int side = Random.Range(0, 2) * 2 - 1;
        Vector3 spawnPos = new Vector3(Random.Range((mapWidth * side), mapWidth * -side), Random.Range((mapHeight * side), mapHeight * -side), 0f);

        if (newHazard.ID == 0)
        {
            spawnPos.x = mapWidth * side;
            newHazard.SetTargetPosition(-spawnPos);
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
