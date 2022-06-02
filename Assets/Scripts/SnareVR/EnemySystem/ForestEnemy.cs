using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//TODO - we are simply creating this class for forest enemy circumstances to implement generic inheritance technique
//will likely back up and create a Character class that both enemy and ally units can inherit from / using some composition or interfaces for greater complexity / decoupling
public class ForestEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //what will be our unique implementations OR why we need to have a more generic base class?     
    //what will be different in onSpawn or Death for these? Maybe VFXs, attacks - one shoots, one strikes, but this opens for options later when we think of them

    //VARIABLES / PROPERTIES
    //3d model
    public NavMeshAgent thisZombieAgent = null;
    public Transform destination;
    public float speed = 0.0f;
    public float health = 100;
    //animation

    //spawn - the stuff we want to uniquely happen for this enemy's setup (not actually being instantiated here)
    public virtual void Spawn(float updatedSpeed, Transform spawnPosition, Transform updatedDestination)
    {
        Reset();

        transform.position = spawnPosition.position;

        //can set to inspector value of greater than 0 to custom control the speed, otherwise will be set by the Controller
        if(speed == 0.0f)
        {
            thisZombieAgent.speed = updatedSpeed;
        }

        thisZombieAgent.SetDestination(updatedDestination.position);
    }

    //called every time Spawned so that returned to initial values (since will be pooled)
    //protected as it will only be called in this script and those that derive from it
    protected virtual void Reset()
    {
        health = 100;
    }

    //curently will be called when ontriggerenter occurs (here) - protected virtual
    //attack (for wolf it will be move to, for snake it wont be move, but will be spit)

    protected virtual void Attack(float damage)
    {
        //wolfAudio.PlayOneShot(wolfAttack);
    }

    //METHODS
    //play audio

    //take damange / death + repool (where is the pool stored? should be on the SpawnController & shouldnt care about type should only consider type Enemy...?)
    public virtual void TakeDamage(float damage)
    {
        health =- damage;

        if(health < 0)
        {
            //die
            //wolfAudio.PlayOneShot(wolfDeath);

            ScriptsConnector.Instance.OnWolfDeath.Invoke(gameObject, true);

            //repool
            gameObject.SetActive(false);
        }
    }

    public virtual void Repool()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Arrow(Clone)")
        {
            TakeDamage(100);
        }
        else if (collision.gameObject.name == "DestinationAI")
        {
            Attack(1);
        }
    }
}
