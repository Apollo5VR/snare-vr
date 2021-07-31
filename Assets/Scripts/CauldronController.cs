using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //note: heavy on processor, wouldnt use if this functionality happened frequently
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public string[,] potionCombos = new string[4, 2] { {"Sword", "Crystal"}, {"Scroll", "Skull"}, 
        {"Flower", "Crystal"}, {"Scroll", "Flower"} };

    private bool finalResponseSent = false;
    public string[] responseObjNames = new string [2];
    private List<CommonEnums.HouseResponses> responseHouseEnums = new List<CommonEnums.HouseResponses>();

    private string[] GetRow(string[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        //for(int i = 0; i < responseHouseEnums.Count; i++)
        //{
        //if(responseObjNames[i] == "")
        //{
        if (!finalResponseSent)
        {
            CommonEnums.HouseResponses response = ResponseCollector.OnCheckAcceptableTags.Invoke(other.tag);
            responseHouseEnums.Add(response);
            responseObjNames[responseHouseEnums.Count - 1] = other.name;
            //break;
            //}
            //}

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
                            ResponseCollector.OnResponseSelected?.Invoke(houseResponse);
                        }

                        finalResponseSent = true;

                        //TODO - GG - some kind of scene progression or button activation here?

                        break;
                    }
                    else
                    {
                        //on last array item, if no matches, clear array - clear reverts back to base values for each index ie ""
                        if (i == 3)
                        {
                            Array.Clear(responseObjNames, 0, responseObjNames.Length);
                            responseHouseEnums = new List<CommonEnums.HouseResponses>();
                        }
                    }
                }
            }
        }  
    }
}
