using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    //Script Purpose: To keep track of how many many wolves have gotten to the eggs, if 4, end.
    //if win, get to choose from 4 eggs, if lose, get a chicken egg (which is NONE value, but at least still fun)

    public GameObject UICanvas;
    public GameObject Reticule;
    public int playerHit;

    //create array of hearts (amount of health)
    public GameObject[] hearts;

    private void OnTriggerEnter(Collider other)
    {
        //if wolf
        if (other.gameObject.layer == 8)
        {
            PlayerAttacked();
            Destroy(other.gameObject);
        }
    }

    public void PlayerAttacked()
    {
        //add up ever time the player is hit
        playerHit = playerHit + 1;

        TurretProgressionController.Instance.OnDecrementLP();

        //for each time player hit, deactivate an egg
        if(playerHit <= hearts.Length)
        {
            hearts[playerHit - 1].SetActive(false);

            Debug.Log("Wolf Bit You:" + playerHit + "Time(s)");
        }

        //if hit three times, game over - bring up restart screen
        if (playerHit == 4)
        {
            Debug.Log("Wolves have attacked the eggs " + playerHit + " times, its all over.");

            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
