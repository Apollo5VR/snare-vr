using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
    public float delay1 = 4.0f;
    public float delayProving = 15.0f;
    public List<string> fullTextList; 
    private string chosenText = "";
    public int fullTextCount;
    public Animator optionalDoorAnimator;
    private bool hasFinishedTypingz = true;

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
                chosenText = fullTextList[0].Substring(0, i);
                this.GetComponent<Text>().text = chosenText;
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(delay1);
            fullTextList.Remove(fullTextList[0]);
            //fullTextCount = fullTextCount + 1;
            hasFinishedTypingz = true;

            if (fullTextList.Count == 0)
            {
                if(optionalDoorAnimator != null)
                {
                    optionalDoorAnimator.SetBool("doorOpenNowBool", true);
                }
            }
        }
	}
}
