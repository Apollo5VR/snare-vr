using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseToggle : MonoBehaviour
{
    public Text[] texts;
    public float timePassed;
    public AudioSource teaserAudio;

    // Update is called once per frame
    void Update()
    {
        if(timePassed > 8)
        {
            //deactivate text
            texts[0].enabled = false;
            texts[1].enabled = true;

            teaserAudio.Play();
        }
        if(timePassed > 40)
        {
            //close game
            Application.Quit();
        }

        timePassed += Time.deltaTime;
    }
}
