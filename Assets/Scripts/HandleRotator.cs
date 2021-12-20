using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRotator : MonoBehaviour
{
    public enum AnimType
    {
        Rock,
        DoorHandle
    };
    public float rotationMod;
    public float speed = 1.0f;
    private float t;
    public float travel;
    public AnimType type;

    public float parentRotation;

    private void Start()
    {
        parentRotation = transform.parent.rotation.y;
 
        if (type == AnimType.DoorHandle)
        {
            rotationMod = 45;
        }
        else if (type == AnimType.Rock)
        {
            parentRotation = -48;
            rotationMod = 20;
        }
    }

    void Update()
    {
        //TODO - have this only occur for a set amount of time?

        if(type == AnimType.DoorHandle)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, rotationMod), t * speed);
        }
        else if(type == AnimType.Rock)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(parentRotation, 0, 0), Quaternion.Euler(parentRotation - rotationMod, 0, 0), t * speed);
        }

        // .. and increase the t interpolater
        t += travel * Time.deltaTime;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f)
        {
            if (type == AnimType.Rock)
            {
                travel += Random.Range(-1.5f, 1.5f);
                travel = Mathf.Clamp(travel, 0.75f, 2.0f);
            }

                transform.rotation = Quaternion.Euler(parentRotation, 0, 0);
            t = 0.0f;
        }
    }
}
