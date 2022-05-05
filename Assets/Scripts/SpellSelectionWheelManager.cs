using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Analytics;

    public class SpellSelectionWheelManager : MonoBehaviour
    {
        public GameObject[] spellButtons;

        public static Action<GameObject> OnSpellSelected;

        private bool tutorialResponseRecorded;
        private BNG.WandSpellsController wandSpellsController;

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
            gameObject.SetActive(false);
            OnSpellSelected += UpdateSpellSelected;
        }
        
    /*
        public void Update()
        {
            if (BNG.InputBridge.Instance.YButton)
            {
                if(gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }
    */



        //Observer in an Observer Pattern
        private void UpdateSpellSelected(GameObject spellButton)
        {
            CommonEnums.HouseResponses houseResponse = CommonEnums.HouseResponses.None;

            switch (spellButton.gameObject.name)
            {
                case "Accio":
                    wandSpellsController.spellSelected = CommonEnums.AvailableSpells.Accio;
                    houseResponse = CommonEnums.HouseResponses.Ravenclaw;
                    break;
                case "Baubil":
                    wandSpellsController.spellSelected = CommonEnums.AvailableSpells.Baubil; //now Lightning
                    houseResponse = CommonEnums.HouseResponses.Gryfindor;
                    break;
                case "WingardiumLeviosa":
                    wandSpellsController.spellSelected = CommonEnums.AvailableSpells.WingardiumLeviosa;
                    houseResponse = CommonEnums.HouseResponses.Hufflepuff;
                    break;
                case "Incendio":
                    wandSpellsController.spellSelected = CommonEnums.AvailableSpells.Incendio;
                    houseResponse = CommonEnums.HouseResponses.Slytherin;
                    break;
                default:
                    wandSpellsController.spellSelected = CommonEnums.AvailableSpells.None;
                    break;
            }

            if (!tutorialResponseRecorded)
            {
                ResponseCollector.Instance.OnResponseSelected?.Invoke(houseResponse);

                if (!Application.isEditor)
                {
                    //Analytics Beta
                    Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "specificQuestion", "TutorialSpellSelection" },
                { "houseIndex", (int)houseResponse },
            };
                    Events.CustomData("questionResponse", parameters);
                }

                tutorialResponseRecorded = true;
            }

            gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            OnSpellSelected -= UpdateSpellSelected;
        }
    }
