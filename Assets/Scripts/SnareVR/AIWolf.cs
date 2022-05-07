using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWolf : MonoBehaviour
{
    //Script Purpose: To apply AI to enemy zombies, so they can randomly attack player (increasing difficulty overtime)

    //collect the zombie and player details
    public NavMeshAgent thisZombieAgent = null;
    public Transform destination;
    public float speed = 1.0f;

    // Start is called before the first frame update
    private void OnEnable()
    {
        destination = ScriptsConnector.Instance.OnGetTrapDestination?.Invoke();

        thisZombieAgent.speed = speed;
        thisZombieAgent.SetDestination(destination.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Arrow(Clone)" || collision.gameObject.name == "DestinationAI")
        {
            bool isKilled = collision.gameObject.name == "Arrow(Clone)";

            ScriptsConnector.Instance.OnWolfDeath.Invoke(gameObject, isKilled);
            gameObject.SetActive(false);
        }
    }
}
