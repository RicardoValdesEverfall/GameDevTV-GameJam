using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private int TimeToAction;
    [SerializeField, Range(0f,1f)] private float SuccessFactor;

    [SerializeField] private PlayerController _playerControllerRef;

    [SerializeField] private Transform[] Points;
    [SerializeField] private int index = 0;
    [SerializeField] private float attackTime;
    [SerializeField] public bool isAttacking;

    [SerializeField] private Transform Agent;
    [SerializeField] private Light DoorLight;

    private float timerToAction;
    private float timerToAttack;
    private int timeSinceLastAttack = 0;

    private void Start()
    {
        MoveAgent(index);
        DoorLight.gameObject.SetActive(false);
        if (_playerControllerRef == null) { _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); }
    }

    private void Update()
    {
        timerToAction += Time.deltaTime;
        if (timerToAction > TimeToAction)
        {
            UpdateAgents();
            timerToAction = 0;
        }
    }

    private void UpdateAgents()
    {
        if (!isAttacking)
        {
            float moveChance = Random.Range(0.0f, 1.0f);
            if (moveChance > SuccessFactor)
            {
                TakeAction();
            }
            else { Debug.Log("Failed Action"); }
        }
        else
        {
            timerToAttack -= Time.deltaTime;
            if (timerToAttack < 0)
            {
                DoorLight.gameObject.SetActive(false);
                timerToAttack = attackTime;
                isAttacking = false;
                index = 0;
                MoveAgent(index);
                Debug.Log("Failed Attack!");
            }

            if (_playerControllerRef.consoleStatus && timerToAttack > 0)
            {
                Attack();
            }
        }
    }

    private void TakeAction()
    {
        switch (index)
        {
            case 0:
                //Move agent to Kitchen2
                index = 1;
                break;
            case 1:
                //Move agent to hall
                index = 2;
                break;
            case 2:
                //Move agent to bathroom OR hall2 OR living room OR Kitchen2.
                index = Random.Range(1,6);
                if (index == 2) { index = 5; }
                break;
            case 3:
                //Check if agent can attack
                float attackChance = Random.Range(0.0f, 1.0f);
                if (attackChance > SuccessFactor) { isAttacking = true; DoorLight.gameObject.SetActive(true); }
                else { index = 2; MoveAgent(index); }
                return;
            case 4:
                //Move to hall
                index = 2;
                break;
            case 5:
                //Move to living room2 OR hall
                index = Random.Range(0, 1);
                if (index == 0) { index = 2; }
                else { index = 6; }
                break;
            case 6:
                //Move to bedroom OR living room
                index = Random.Range(0, 1);
                if (index == 0) { index = 5; }
                else { index = 7; }
                break;
            case 7:
                //Move to living room2
                index = 6;
                break;
        }

        if (index != 3 && isAttacking == false) { timeSinceLastAttack++; }
        if (timeSinceLastAttack > attackTime) { index = 3; timeSinceLastAttack = 0; }
        MoveAgent(index);
    }

    private void MoveAgent(int pointIndex)
    {
        Agent.position = Points[pointIndex].position;
        Debug.Log("Moved " + pointIndex.ToString());
    }

    private void Attack()
    {
       _playerControllerRef.PlayerCurrentState = PlayerController.PlayerState.isDead;
        Debug.Log("Succeeded Attack!");
    }
}
