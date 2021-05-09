using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

	public float delay = 0.1f;
    public float delay1 = 5.0f;
    public float delayProving = 15.0f;
	public string fullText;
    public string fullText1;
    public string fullText2;
    public string fullText3;
    public string fullText4;
    public string fullText5;
    public string fullText6;
    public string fullText7 = "Collection complete, your response has been recorded. ";
    private string currentText = "";
    public int fullTextCount;
    public bool hasFinishedTypingz;
    public Animator doorAnimator;

    // Use this for initialization
    void Start () {
		StartCoroutine(ShowText());
        hasFinishedTypingz = false;

    }
	
    void Update()
    {
        if (fullTextCount > 0 && hasFinishedTypingz)
        {
            StartCoroutine(ShowText());
        }
        //opens door once  walk instructions given
        if (fullTextCount == 6)
        {
            doorAnimator.SetBool("doorOpenNowBool", true);
        }
    }

	public IEnumerator ShowText(){
        {
            if (fullTextCount == 1)
            {
                fullText = fullText1;
                hasFinishedTypingz = false;
            }
            else if (fullTextCount == 2)
            {
                fullText = fullText2;
                hasFinishedTypingz = false;
            }
            else if (fullTextCount == 3)
            {
                fullText = fullText3;
                hasFinishedTypingz = false;
            }
            else if (fullTextCount == 4)
            {
                fullText = fullText4;
                hasFinishedTypingz = false;
            }
            else if (fullTextCount == 5)
            {
                fullText = fullText5;
                hasFinishedTypingz = false;
            }
            else if (fullTextCount == 6)
            {
                fullText = fullText6;
                hasFinishedTypingz = false;
            }
            //specifically for Proving Scene Completion Text
            else if (fullTextCount == 7)
            {
                fullText = fullText7;
                hasFinishedTypingz = false;
                delay1 = delayProving;
            }

            for (int i = 0; i < fullText.Length; i++)
            {
                currentText = fullText.Substring(0, i);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(delay1);
            fullTextCount = fullTextCount + 1;
            hasFinishedTypingz = true;

            if (fullTextCount == 7)
            {
                hasFinishedTypingz = false;
            }
        }
	}
}
