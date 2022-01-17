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
        destination = TurretProgressionController.Instance.OnGetDinoNest();

        thisZombieAgent.speed = speed;
        thisZombieAgent.SetDestination(destination.position);
    }
}
