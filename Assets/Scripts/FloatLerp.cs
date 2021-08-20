using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLerp : MonoBehaviour
{
    private float minimum = -2.0f;
    private float maximum = -1.5f;

    // starting value for the Lerp
    static float t = 0.0f;

    private void Start()
    {
        minimum = gameObject.transform.position.y - 0.1f;
        maximum = gameObject.transform.position.y + 0.1f;
    }


    void Update()
    {
            // animate the position of the game object...
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Lerp(minimum, maximum, t), gameObject.transform.position.z);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
