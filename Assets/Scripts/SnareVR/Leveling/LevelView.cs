using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public bool debugTest;

    [SerializeField] private Text levelText;
    [SerializeField] private Image experienceBarImage;

    private PlayerLevelingDetails levelDetails;

    private void Awake()
    {
        //levelText = transform.Find("levelText").GetComponent<Text>();
        //experienceBarImage = transform.Find("experienceBar").Find("bar").GetComponent<Image>();

        //transform.Find("experience5Btn").GetComponent<Button_UI>().ClickFunc = () => levelSystem.AddExperience(5);
        //transform.Find("experience50Btn").GetComponent<Button_UI>().ClickFunc = () => levelSystem.AddExperience(50);
        //transform.Find("experience500Btn").GetComponent<Button_UI>().ClickFunc = () => levelSystem.AddExperience(500);
    }
    public void Update()
    {
        if (debugTest)
        {
            levelDetails.AddXp(50);
            debugTest = false;
        }
    }

    private void SetExperienceBarSize(float experienceNormalized)
    {
        experienceBarImage.fillAmount = experienceNormalized;
    }

    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = "LVL: " + (levelNumber + 1);
    }

    public void SetLevelSystem(PlayerLevelingDetails levelDetails)
    {
        this.levelDetails = levelDetails;

        SetLevelNumber(levelDetails.GetLevelNumber());
        SetExperienceBarSize(levelDetails.GetXpNormalized());

        levelDetails.OnXpChanged += LevelSystem_OnXpChanged;
        levelDetails.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        // Level changed, update text
        SetLevelNumber(levelDetails.GetLevelNumber());
    }

    private void LevelSystem_OnXpChanged(object sender, System.EventArgs e)
    {
        // Experience changed, update bar size
        SetExperienceBarSize(levelDetails.GetXpNormalized());
    }

    private void OnDestroy()
    {
        if(levelDetails != null)
        {
            levelDetails.OnXpChanged -= LevelSystem_OnXpChanged;
            levelDetails.OnLevelChanged -= LevelSystem_OnLevelChanged;
        }
    }

    //required to save for the user manually quiting the game on an android device
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            //TODO - need to centralize all this end game / pause saving calls to one location / script
            ScriptsConnector.Instance.OnSaveXpToUGS?.Invoke("PLAYER_LEVELING_DETAILS", levelDetails);
        }
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        ScriptsConnector.Instance.OnSaveXpToUGS?.Invoke("PLAYER_LEVELING_DETAILS", levelDetails);
    }
#endif
}
