using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController2D : MonoBehaviour
{
    [SerializeField] private Hazard[] Hazards;
    [SerializeField] private Transform HazardsParent;

    [SerializeField, Range(1f, 10f)] private float SpawnTimerHazards;
    [SerializeField, Range(1f, 10f)] private float SpawnTimerPickups;

    [SerializeField] private Image Background;

    private float spawnTimerH;
    private float spawnTimerP;

    public bool canSpawn;
    private int mapWidth;
    private int mapHeight;

    void Start()
    {
        spawnTimerH = SpawnTimerHazards;
        spawnTimerP = SpawnTimerPickups;

        mapWidth = (int)(Background.rectTransform.rect.width / 2);
        mapHeight = (int)(Background.rectTransform.rect.height / 2);
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
        Vector3 spawnPos = new Vector3(mapWidth * side, Random.Range((mapHeight * side), mapHeight * -side), 0f);

        newHazard.SetStartPosition(spawnPos);
        newHazard.SetTargetPosition(-spawnPos);
    }

    private void SpawnPickup()
    {

    }
}
