using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float InitialHealth { get; set; }
    float CurrentHealth { get; set; }
    void InitializeHealth(float initialHealth);
    void RestoreHealth(float restoreAmount);
    void TakeDamage(float damage);
}
