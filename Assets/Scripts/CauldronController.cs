using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //note: heavy on processor, wouldnt use if this functionality happened frequently
using Unity.Services.Analytics;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public GameObject bubbleGO;
    public bool testBubble = false;
    [Range(0.0f, 5.0f)]
    public float minimumCreate;
    [Range(0.0f, 5.0f)]
    public float maximiumCreate;
    [Range(0.0f, 5.0f)]
    public float minimumDelete;
    public float bubbleFxDuration;
    public AudioSource[] audioData;

    public GameObject potionsText;
    public GameObject potionsBottle;
    //public MeshRenderer potionsMeshRenderer;
    //public Material[] potionMats;
    public Transform returnTranform;

    public Color[] potionsColor;
    public GameObject surfaceGO;
    private Material surfaceMat;
    private Material bubbleMat;

    private string[,] potionCombos = new string[4, 2] { {"Sword", "Crystal"}, {"Scroll", "Skull"}, 
        {"Flower", "Crystal"}, {"Scroll", "Flower"} };

    private bool finalResponseSent = false;
    private string[] responseObjNames = new string [2];
    private List<CommonEnums.HouseResponses> responseHouseEnums = new List<CommonEnums.HouseResponses>();

    private string[] GetRow(string[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }

    private void Start()
    {
        potionsBottle.SetActive(false);
        potionsText.SetActive(false);

        audioData = GetComponents<AudioSource>();

        surfaceMat = surfaceGO.GetComponent<Renderer>().material;
        bubbleMat = bubbleGO.GetComponent<Renderer>().material;

        surfaceGO.SetActive(false);
        bubbleGO.SetActive(false);
    }

    public void Update()
    {
        if (testBubble)
        {
            StartCoroutine(BubbleFx());
            testBubble = false;
        }
        else
        {
            //StopAllCoroutines();
        }
    }

    private void ActivatePotionBottle()
    {
        potionsBottle.SetActive(true);
        potionsText.SetActive(true);
    }

    public void bottleGrabbedDrink()
    {
        audioData[2].Play();
        ProgressionController.Instance.OnLoadNextScene?.Invoke(6);
    }

    private IEnumerator BubbleFx()
    {
        float currentTimePassed = 0;

        audioData[0].Play();

        while (currentTimePassed < bubbleFxDuration)
        {
            float randomNum = UnityEngine.Random.Range(minimumCreate, maximiumCreate);

            yield return new WaitForSeconds(randomNum);

            CreateBubble();

            currentTimePassed += randomNum;
        }

        audioData[0].Stop();

        ActivatePotionBottle();
    }

    private void CreateBubble()
    {
        //instantiate 1s bubbles on the same y height, at different scale, with the same color as the rest of the liquid but slightly darker
        Vector3 position = UnityEngine.Random.insideUnitCircle * 0.5f;
        position = new Vector3(bubbleGO.transform.position.x + position.x, bubbleGO.transform.position.y, bubbleGO.transform.position.z + position.y);
        GameObject newBubble = Instantiate(bubbleGO, position, Quaternion.Euler(0, 0, 0));
        newBubble.transform.localScale *= UnityEngine.Random.Range(0.5f, 3.0f);
        newBubble.SetActive(true);

        StartCoroutine(DeleteBubble(newBubble));
    }

    private IEnumerator DeleteBubble(GameObject bubble)
    {
       yield return new WaitForSeconds(minimumDelete);

       Destroy(bubble);
    }

    private void SetColor(int index)
    {
        surfaceMat.color = potionsColor[index];
        surfaceGO.SetActive(true);

        Color darkerShade = potionsColor[index] * 1.25f;
        darkerShade.a = 1;
        bubbleMat.color = darkerShade; //to recorrect the transparency

        //potionsMeshRenderer.material.SetColor("_Color", darkerShade);
    }

    private void OnTriggerEnter(Collider other)
    {
        //depreciated
        //Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        //rb.useGravity = true;

        other.gameObject.transform.position = returnTranform.position + new Vector3(0, 0, UnityEngine.Random.Range(-1, 1));

        if (!finalResponseSent)
        {
            CommonEnums.HouseResponses response = ResponseCollector.Instance.OnCheckAcceptableTags.Invoke(other.tag);
            responseHouseEnums.Add(response);
            responseObjNames[responseHouseEnums.Count - 1] = other.name;

            if (responseHouseEnums.Count == 2)
            {
                //4 for the existing length of the first index of the multi-array
                for (int i = 0; i < 4; i++)
                {
                    bool isEqual = responseObjNames.SequenceEqual(GetRow(potionCombos, i));
                    bool isEqual2 = responseObjNames.Reverse().SequenceEqual(GetRow(potionCombos, i));

                    if (isEqual || isEqual2)
                    {
                        //send the response for the 2 items
                        foreach (CommonEnums.HouseResponses houseResponse in responseHouseEnums)
                        {
                            ResponseCollector.Instance.OnResponseSelected?.Invoke(houseResponse);

                            //TODO - refactor to one location (so we only need 1 script to have Analytics dependency)
                            /*
                            if (!Application.isEditor)
                            {
                                //Analytics Beta
                                Dictionary<string, object> parameters = new Dictionary<string, object>()
                                {
                                    { "specificQuestion", "Potion" },
                                    { "houseIndex", (int)houseResponse },
                                };
                                AnalyticsService.Instance.CustomData("questionResponse", parameters);
                            }
                            */
                        }

                        finalResponseSent = true;

                        SetColor(i);

                        StartCoroutine(BubbleFx());

                        break;
                    }
                    else
                    {
                        //on last array item, if no matches, clear array - clear reverts back to base values for each index ie ""
                        if (i == 3)
                        {
                            audioData[1].Play();

                            //failed color
                            SetColor(i + 1);

                            Array.Clear(responseObjNames, 0, responseObjNames.Length);
                            responseHouseEnums = new List<CommonEnums.HouseResponses>();
                        }
                    }
                }
            }
        }
    }
}
