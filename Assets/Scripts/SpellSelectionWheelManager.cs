using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellSelectionWheelManager : MonoBehaviour
{
    public GameObject[] spellButtons;
    public BNG.WandSpellsController wandSpellsController;

    public static Action<GameObject> OnSpellSelected;

    public static SpellSelectionWheelManager Instance { get; private set; }

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

    public void Start()
    {
        OnSpellSelected += UpdateSpellSelected;
    }

    private void UpdateSpellSelected(GameObject spellButton)
    {
        switch (spellButton.gameObject.name)
        {
            case "Accio":
                wandSpellsController.spellSelected = CommonEnums.availableSpells.Accio;
                break;
            case "Stupify":
                wandSpellsController.spellSelected = CommonEnums.availableSpells.Stupify;
                break;
            case "WingardiumLeviosa":
                wandSpellsController.spellSelected = CommonEnums.availableSpells.WingardiumLeviosa;
                break;
            case "Incendio":
                wandSpellsController.spellSelected = CommonEnums.availableSpells.Incendio;
                break;
            default:
                wandSpellsController.spellSelected = CommonEnums.availableSpells.None;
                break;
        }

        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        OnSpellSelected -= UpdateSpellSelected;
    }
}
