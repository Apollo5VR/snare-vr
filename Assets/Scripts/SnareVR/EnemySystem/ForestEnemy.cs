using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//TODO - we are simply creating this class for forest enemy circumstances to implement generic inheritance technique
//will likely back up and create a Character class that both enemy and ally units can inherit from / using some composition or interfaces for greater complexity / decoupling
//design notes: what will be our unique implementations OR why we need to have a more generic base class?     
//what will be different in onSpawn or Death for these? Maybe VFXs, attacks - one attacks ranged, one attacks close, but this opens for options later when we think of them
public abstract class ForestEnemy : MonoBehaviour, IDamageable
{
    //difficulty
    public bool useInspectorSpeed = false;
    public float speed = 0.0f;

    //audio
    public AudioClip spawnAudio;
    public AudioClip attackAudio;
    public AudioClip deathAudio;

    private AudioSource enemyAudioSource;
    private NavMeshAgent thisEnemyAgent;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeHealth(100);
        enemyAudioSource = GetComponent<AudioSource>();
        thisEnemyAgent = GetComponent<NavMeshAgent>();
    }

    //note - is the actions we want to uniquely happen for this enemy's setup (not actually being instantiated here)
    public virtual void Spawn(float updatedSpeed, Transform spawnPosition, Transform updatedDestination)
    {
        Reset();

        transform.position = spawnPosition.position;

        //can set to inspector value of greater than 0 to custom control the speed, otherwise will be set by the Controller
        thisEnemyAgent.speed = useInspectorSpeed ? speed : updatedSpeed;

        gameObject.SetActive(true);
        thisEnemyAgent.SetDestination(updatedDestination.position);

        enemyAudioSource.PlayOneShot(spawnAudio);
    }

    //called every time Spawned so that returned to initial values (since will be pooled)
    //protected as it will only be called in this script and those that derive from it
    protected virtual void Reset()
    {
        RestoreHealth();
    }

    protected virtual void Attack(float damage)
    {
        enemyAudioSource.PlayOneShot(attackAudio);

        //TODO - sendDamage

        //repool enemy 
        //TODO - will build out more extensive system for animal to continue attacking, but keeping simple now.
        StartCoroutine(WaitForAudioToRepool(attackAudio));
    }

    //TODO - consider implementing
    protected virtual void ApplyParticleFX(Vector3 position, Quaternion rotation)
    {

    }

    protected virtual void Repool()
    {
        gameObject.SetActive(false);
    }

    protected IEnumerator WaitForAudioToRepool(AudioClip clip)
    {
        while (enemyAudioSource.isPlaying)
        {
            yield return null;
        }

        // place your code that should execute when the audio source has finised here.
        Repool();
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Arrow(Clone)")
        {
            TakeDamage(100);
        }
        else if (collider.gameObject.name == "DestinationAI")
        {
            Attack(1);
        }
    }

    //note: these are implemented from IDamagable
    public float InitialHealth { get; set; }
    public float CurrentHealth { get; set; }

    public virtual void InitializeHealth(float initialHealth)
    {
        InitialHealth = initialHealth; //TODO - replace this hardcoded 100 with a dynamic value via param
    }

    //Question: is this going to lead to duplicative code when we use IDamagable otherwhere? is that a big problem?
    public virtual void RestoreHealth(float restoreAmount = 0.0f)
    {
        if(restoreAmount == 0.0f)
        {
            CurrentHealth = InitialHealth;
        }
        else
        {
            CurrentHealth += (float)restoreAmount;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHealth = -damage;

        //die
        if (CurrentHealth <= 0)
        {  
            enemyAudioSource.PlayOneShot(deathAudio);

            ScriptsConnector.Instance.OnDeath.Invoke(gameObject, true);

            //repool
            StartCoroutine(WaitForAudioToRepool(deathAudio));
        }
    }
}
