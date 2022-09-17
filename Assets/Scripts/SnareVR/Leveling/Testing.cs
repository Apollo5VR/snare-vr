using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private LevelView levelView;

    private void Awake()
    {
        PlayerLevelingDetails levelDetails = new PlayerLevelingDetails();
        levelView.SetLevelSystem(levelDetails);
    }
}
