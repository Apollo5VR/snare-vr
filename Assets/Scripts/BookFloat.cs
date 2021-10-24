using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFloat : MonoBehaviour
{
    public bool testBool;

    private float minimum;
    private float maximum;
    private float t;
    private bool isFloating = true;

    // Start is called before the first frame update
    void Start()
    {
        minimum = this.transform.position.y - 0.1f;
        maximum = this.transform.position.y + 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("book triggered by: " + other.tag);
            isFloating = false;
        }
    }

    void Update()
    {
        if (isFloating)
        {
            // animate the position of the game object...
            this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp(minimum, maximum, t), this.transform.position.z);

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
