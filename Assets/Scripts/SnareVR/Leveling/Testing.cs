using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private LevelView levelView;

    private void Start()
    {
        ScriptsConnector.Instance.OnSetXp += SetXp;
    }
    private void SetXp(string playerID, PlayerLevelingDetails levelDetails)
    {
        levelView.SetLevelSystem(levelDetails);
    }

    private void OnDestroy()
    {
        ScriptsConnector.Instance.OnSetXp -= SetXp;
    }
}
