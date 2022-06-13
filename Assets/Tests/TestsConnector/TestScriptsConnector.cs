using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestScriptsConnector
{
    public static Action SpawnTrap;
    public static Action<string, float> OnModifyHealth;
    public static Func<string, float> GetHealth;
}
