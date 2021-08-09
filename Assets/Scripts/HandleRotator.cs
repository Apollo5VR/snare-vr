using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRotator : MonoBehaviour
{
    public float speed = 1.0f;
    private float t;
    void Update()
    {
        //TODO - have this only occur for a set amount of time?

        transform.rotation = Quaternion.Lerp(Quaternion.Euler(0,0,0), Quaternion.Euler(0, 0, 45), t * speed);

        // .. and increase the t interpolater
        t += 1.5f * Time.deltaTime;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            t = 0.0f;
        }
    }
}
