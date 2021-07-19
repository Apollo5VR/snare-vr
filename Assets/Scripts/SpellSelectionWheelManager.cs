using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellSelectionWheelManager : MonoBehaviour
{
    public GameObject[] spellButtons;
    public BNG.WandSpellsController wandSpellsController;

    public static Action<GameObject> OnSpellSelected;

    //depreciated - redundant since we use Action based sub 
    /*
    public static SpellSelectionWheelManager Instance { get; private set; }

    //singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    */

    public void Start()
    {
        OnSpellSelected += UpdateSpellSelected;
    }

    //Observer in an Observer Pattern
    private void UpdateSpellSelected(GameObject spellButton)
    {
        switch (spellButton.gameObject.name)
        {
            case "Accio":
                wandSpellsController.spellSelected = CommonEnums.AvailableSpells.Accio;
                //TODO - notify ResponseCollector - 
                //CommonEnums.houseResponses.Ravenclaw
                break;
            case "Stupify":
                wandSpellsController.spellSelected = CommonEnums.AvailableSpells.Stupify;
                break;
            case "WingardiumLeviosa":
                wandSpellsController.spellSelected = CommonEnums.AvailableSpells.WingardiumLeviosa;
                break;
            case "Incendio":
                wandSpellsController.spellSelected = CommonEnums.AvailableSpells.Incendio;
                break;
            default:
                wandSpellsController.spellSelected = CommonEnums.AvailableSpells.None;
                break;
        }

        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        OnSpellSelected -= UpdateSpellSelected;
    }
}
