using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Windows.Speech;

    //TODO - potentially renable
public class SpeechRecognitionEngine : MonoBehaviour
{
    //public string[] keywords = new string[] { "up", "down", "left", "right" }; //Needs Change
    //public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    //public float speed = 1; //remove

    //public Text results;
    //public Image target; //needs change

    //protected PhraseRecognizer recognizer;
    //protected string word = "right"; //needs change

    //private void Start()
    //{
    //    if (keywords != null)
    //    {
    //        recognizer = new KeywordRecognizer(keywords, confidence);
    //        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
    //        recognizer.Start();
    //    }
    //}

    //private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    //{
    //    word = args.text;
    //    results.text = "You said: <b>" + word + "</b>";
    //}

    //private void Update()
    //{
    //    //Needs Change
    //    var x = target.transform.position.x;
    //    var y = target.transform.position.y;

    //    switch (word)
    //    {
    //        case "up":
    //            y += speed;
    //            break;
    //        case "down":
    //            y -= speed;
    //            break;
    //        case "left":
    //            x -= speed;
    //            break;
    //        case "right":
    //            x += speed;
    //            break;
    //    }

    //    target.transform.position = new Vector3(x, y, 0);
    //}

    //private void OnApplicationQuit()
    //{
    //    if (recognizer != null && recognizer.IsRunning)
    //    {
    //        recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
    //        recognizer.Stop();
    //    }
    //}
}
