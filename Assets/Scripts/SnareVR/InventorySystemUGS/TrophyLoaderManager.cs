using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyLoaderManager : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        ScriptsConnector.Instance.OnRequestItems?.Invoke();
    }

    void Start()
    {
        //TODO - refactor these two requests to 1 async
        //TODO - very horrible race condition, refactor once out of proto
        StartCoroutine(WaitThis());
    } 

    private IEnumerator WaitThis()
    {
        yield return new WaitForSeconds(3f);

        List<string> itemNames = ScriptsConnector.Instance.OnRequestItemNames?.Invoke();

        foreach (var go in gameObjects)
        {
            if (itemNames.Contains(go.name))
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }

        }
    }
}
