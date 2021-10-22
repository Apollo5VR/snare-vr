using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TypeWriterEffect : MonoBehaviour
{

    public float delay = 0.1f;
    public float delay1 = 4.0f;
    public float delayProving = 15.0f;
    public List<string> fullTextList;
    private string chosenText = "";
    public int fullTextCount;
    public Animator optionalDoorAnimator;
    private bool hasFinishedTypingz = true;

    private Text updateText;

    private void Start()
    {
        updateText = this.GetComponent<Text>();
    }

    void Update()
    {
        if (fullTextList.Count > 0 && hasFinishedTypingz)
        {
            StartCoroutine(ShowText());
        }
    }

    public IEnumerator ShowText()
    {
        {
            hasFinishedTypingz = false;

            for (int i = 0; i < fullTextList[0].Length; i++)
            {
                //TODO - to help it only progress if users reading (incase they wander around the room)
                /*
                if (gazeNotDetected)
                {
                    yield return null;
                }
                */

                chosenText = fullTextList[0].Substring(0, i);
                this.GetComponent<Text>().text = chosenText;
                yield return new WaitForSeconds(delay);
            }

            float timeTicker = 0;
            int waitingLength = fullTextList[0].Length - 1;
            float extendedWaitingDelay = delay * 10;

            //waiting period
            while (timeTicker < delay1)
            {
                if(waitingLength == fullTextList[0].Length - 1)
                {
                    waitingLength = fullTextList[0].Length - 2;
                }
                else
                {
                    waitingLength = fullTextList[0].Length - 1;
                }

                chosenText = fullTextList[0].Substring(0, waitingLength);
                updateText.text = chosenText;
                timeTicker += extendedWaitingDelay;
                yield return new WaitForSeconds(extendedWaitingDelay);
            }

            //yield return new WaitForSeconds(delay1);

            fullTextList.Remove(fullTextList[0]);
            hasFinishedTypingz = true;

            if (fullTextList.Count == 0)
            {
                if (optionalDoorAnimator != null)
                {
                    optionalDoorAnimator.SetBool("doorOpenNowBool", true);
                }
            }
        }
    }

    //TBD - thoughts are we could add another condition to the update ("actionRecieved")
    /*
    public IEnumerator ShowTextResponseActivated()
    {
        {
            hasFinishedTypingz = false;

            for (int i = 0; i < fullTextList[0].Length; i++)
            {
                chosenText = fullTextList[0].Substring(0, i);
                this.GetComponent<Text>().text = chosenText;
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(delay1);
            fullTextList.Remove(fullTextList[0]);

            hasFinishedTypingz = true;

            if (fullTextList.Count == 0)
            {
                if (optionalDoorAnimator != null)
                {
                    optionalDoorAnimator.SetBool("doorOpenNowBool", true);
                }
            }
        }
    }
    */
}
