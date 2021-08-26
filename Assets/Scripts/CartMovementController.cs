using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovementController : MonoBehaviour
{
    public GameObject cart;
    public GameObject moveTrickObjects;
    public GameObject petObjects;
    private float moveSpeed = 0.075f;
    public bool go;

    private void OnTriggerEnter(Collider other)
    {
        //TODO - refactor this to not need such specific
        switch (other.gameObject.tag)
        {
            case "Ravenclaw":
                go = true;
                break;
            case "Gryfindor":
                go = true;
                break;
            case "Hufflepuff":
                go = true;
                break;
            case "Slytherin":
                go = true;
                break;
            default:
                go = false;
                break;
        }

        if (go)
        {
            StartCoroutine(DelayedMoveTrick());
        }
    }

    /*
    public void FixedUpdate()
    {
        moveTrickObjects.transform.position = Vector3.MoveTowards(moveTrickObjects.transform.position, gameObject.transform.position + new Vector3(0, 0, -3), moveSpeed);
    }
    */
    

    private IEnumerator DelayedMoveTrick()
    {
        yield return new WaitForSeconds(12);

        petObjects.SetActive(false);

        while (moveTrickObjects.transform.position != (gameObject.transform.position + new Vector3(0, 0, -3)))
        {
            moveTrickObjects.transform.position = Vector3.MoveTowards(moveTrickObjects.transform.position, gameObject.transform.position + new Vector3(0, 0, -3), moveSpeed);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
