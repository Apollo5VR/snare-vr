using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour
{
    public bool isAnimTrigger;
    public GameObject pointerArrow;
    // animate the game object from -1 to +1 and back
    private float minimum = -2.0f;
    private float maximum = -1.5f;

    // starting value for the Lerp
    static float t = 0.0f;

    private void Start()
    {
        minimum = pointerArrow.transform.position.y - 0.25f;
        maximum = pointerArrow.transform.position.y + 0.25f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hat triggered by: " + other.tag);

            if (isAnimTrigger)
            {
                this.GetComponent<Animator>().enabled = false;
                pointerArrow.SetActive(false);
            }
            else
            {
                SendTimeBasedResponse();
                ProgressionController.OnLoadNextScene?.Invoke();
            }
        }
    }

    //TODO - adjust this to actually mean something
    private void SendTimeBasedResponse()
    {
        float time = Time.timeSinceLevelLoad;
        CommonEnums.HouseResponses response = CommonEnums.HouseResponses.None;

        if(time > 120)
        {
            response = CommonEnums.HouseResponses.Ravenclaw;
        }
        else if(time > 90)
        {
            response = CommonEnums.HouseResponses.Hufflepuff;
        }
        else if (time > 60)
        {
            response = CommonEnums.HouseResponses.Gryfindor;
        }
        else if (time > 30)
        {
            response = CommonEnums.HouseResponses.Slytherin;
        }

        ResponseCollector.OnResponseSelected?.Invoke(response);
    }

    void Update()
    {
        if (pointerArrow.activeSelf)
        {
            // animate the position of the game object...
            pointerArrow.transform.position = new Vector3(pointerArrow.transform.position.x, Mathf.Lerp(minimum, maximum, t), pointerArrow.transform.position.z);

            // .. and increase the t interpolater
            t += 1.5f * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (t > 1.0f)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                t = 0.0f;
            }
        }   
    }
}
