
using UnityEngine;

public class SpellSelectionWheelManager : MonoBehaviour
{
    public static SpellSelectionWheelManager Instance { get; private set; }
    public GameObject[] spellButtons;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
