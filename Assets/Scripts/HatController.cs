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

    //TODO - Refactor to get rid of pointerArrow checks (script used in 3 locations, only 1 controls arrow anim)

    private void Start()
    {
        if (pointerArrow != null)
        {
            minimum = pointerArrow.transform.position.y - 0.25f;
            maximum = pointerArrow.transform.position.y + 0.25f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hat triggered by: " + other.tag);

            if (isAnimTrigger)
            {
                this.GetComponentInParent<Animator>().enabled = false;

                if (pointerArrow != null)
                {
                    pointerArrow.SetActive(false);
                }
            }
            else
            {
                SendTimeBasedResponse();
                //note: replaced loading questions scene with just auto loading into the first question
                ProgressionController.Instance.OnLoadChallengeScene(1);
                //ProgressionController.Instance.OnLoadNextScene?.Invoke(0.5f);
            }
        }
    }

    //TODO - adjust this to actually mean something
    private void SendTimeBasedResponse()
    {
        float time = Time.timeSinceLevelLoad;
        CommonEnums.HouseResponses response = CommonEnums.HouseResponses.None;

        if(time > 90)
        {
            response = CommonEnums.HouseResponses.Ravenclaw;
        }
        else if(time > 60)
        {
            response = CommonEnums.HouseResponses.Hufflepuff;
        }
        else if (time > 30)
        {
            response = CommonEnums.HouseResponses.Gryfindor;
        }
        else if (time > 0)
        {
            response = CommonEnums.HouseResponses.Slytherin;
        }

        ResponseCollector.Instance.OnResponseSelected?.Invoke(response);
    }

    void Update()
    {
        if (pointerArrow != null && pointerArrow.activeSelf)
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
