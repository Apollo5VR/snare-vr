using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWolf : MonoBehaviour
{
    //Script Purpose: To apply AI to enemy zombies, so they can randomly attack player (increasing difficulty overtime)

    //collect the zombie and player details
    public NavMeshAgent thisZombieAgent = null;
    public Transform player;
    public float speed = 1.0f;

    //increase difficulty as time progresses
    private float difficultyTimeCounter;
    //public float easy;
    //public float medium;
    //public float hard;

    // Start is called before the first frame update
    void Start()
    {
        //thisZombieAgent = GetComponent<NavMeshAgent>();
        difficultyTimeCounter = Time.time;
        thisZombieAgent.speed = speed;
        thisZombieAgent.SetDestination(player.position);
    }
}
