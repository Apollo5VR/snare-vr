using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class HPSpeechRecognitionEngine : MonoBehaviour
{
    public string[] questionKeywords; //also contains spell keywords
    public ConfidenceLevel confidence = ConfidenceLevel.Low; //change to low?
    public string questionAnswer;
    public string conclusiveAnswer;
    public string spellSaid;
    public OculusSpells oculusSpells;


    public Text results;

    protected PhraseRecognizer recognizer;
    public string word = "right"; //needs change

    private void Start()
    {
        questionKeywords = new string[] { "Glory", "Wisdom", "Love", "Power", "Wingardium Leviosa", "Akio", "Stupify", "Incendio", "Gryffindor", "Slytherin", "Hufflepuff", "Ravenclaw"};
        if (questionKeywords != null) 
        {
            recognizer = new KeywordRecognizer(questionKeywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "You said: <b>" + word + "</b>";
    }

    private void Update()
    {
        //ADD: "bool" Hat Has asked question as "if" parameter...MAYBE DON'T, they wont say anything till asked.
        switch (word)
        {
            case "Glory":
                questionAnswer = "Glory";
                //y += speed;
                break;
            case "Wisdom":
                questionAnswer = "Wisdom";
                break;
            case "Love":
                questionAnswer = "Love";
                break;
            case "Power":
                questionAnswer = "Power";
                break;
            case "Wingardium Leviosa":
                oculusSpells.spellTimerTicking = true;
                spellSaid = "Wingardium Leviosa";
                word = null;
                break;
            case "Akio":
                oculusSpells.spellTimerTicking = true;
                spellSaid = "Akio";
                word = null;
                break;
            case "Stupify":
                oculusSpells.spellTimerTicking = true;
                spellSaid = "Stupify";
                word = null;
                break;
            case "Incendio":
                oculusSpells.spellTimerTicking = true;
                spellSaid = "Incendio";
                word = null;
                break;
            case "Gryffindor":
                conclusiveAnswer = "Gryffindor";
                //y += speed;
                break;
            case "Slytherin":
                conclusiveAnswer = "Slytherin";
                //y += speed;
                break;
            case "Hufflepuff":
                conclusiveAnswer = "Hufflepuff";
                //y += speed;
                break;
            case "Ravenclaw":
                conclusiveAnswer = "Ravenclaw";
                //y += speed;
                break;
        }


    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
