
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyProjectile : MonoBehaviour {
    public GameObject liveTroll;
    public GameObject deadTroll;
    GameObject trollClub;
    Vector3 liveTrollPosition;
    public ResponseCollector responseCollector;
    int nextLevel;
    float delayTime;
    bool startDelayTime;

    //animations;
    public string trollFallAnimation;
    public string trollLegsAnimation;
    public Animator trollFallStateMachine;
    public Animator trollLegsStateMachine;

    // Use this for initialization
    void Start () {
        deadTroll.SetActive(false);
        trollClub = GameObject.Find("TrollClub");
        responseCollector = GameObject.Find("ResponseCollector").GetComponent<ResponseCollector>();
        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (startDelayTime == true)
        {
            delayTime = delayTime + Time.deltaTime;
        }
        if (delayTime > 5)
        {
                SceneManager.LoadScene(nextLevel);
        }
        //for TESTING
        //if (Time.time > 5 && Time.time < 6)
        //{
        //    liveTrollPosition = liveTroll.transform.position;
        //    //Instantiate Dead Troll
        //    deadTroll.transform.position = liveTrollPosition;
        //    deadTroll.SetActive(true);

        //    //unparent club & make use gravity

        //    //delete Live Troll
        //    Destroy(liveTroll);

        //    //unparent club & make use gravity




        //    //liveTrollPosition = liveTroll.transform.position;
        //    //trollLegsStateMachine.SetTrigger(trollLegsAnimation);
        //    //trollFallStateMachine.SetTrigger(trollFallAnimation);
        //}
    }

    void OnTriggerEnter(Collider other)
    {

        trollDeath();

    }
    public void trollDeath()
    {
        responseCollector.objectUsed = "Troll";
        startDelayTime = true;
        liveTrollPosition = liveTroll.transform.position;
        //Instantiate Dead Troll
        deadTroll.transform.position = liveTrollPosition;
        deadTroll.SetActive(true);

        //unparent club & make use gravity
        trollClub.transform.SetParent(null);
        trollClub.GetComponent<Rigidbody>().useGravity = true;

        //record response
        


        //delete Live Troll
        //Destroy(liveTroll);
        liveTroll.transform.localScale = new Vector3(0, 0, 0);

        //unparent club & make use gravity
    }
}
