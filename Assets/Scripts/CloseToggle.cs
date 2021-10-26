using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseToggle : MonoBehaviour
{
    public GameObject[] texts;
    public float timePassed;
    public AudioSource teaserAudio;

    // Update is called once per frame
    void Update()
    {
        if(timePassed > 8 && timePassed < 9)
        {
            //deactivate text
            texts[0].SetActive(false);
            texts[1].SetActive(true);

            teaserAudio.Play();

            timePassed = 9;
        }
        if(timePassed > 60 && timePassed < 61)
        {
            timePassed = 61;

            //close game
            Application.Quit();
        }

        timePassed += Time.deltaTime;
    }
}
