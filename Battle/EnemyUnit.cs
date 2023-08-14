using System.Collections;
using TMPro;
using UnityEngine;

// **** Enemy Class

public class EnemyUnit : BUnit
{
    public void SetVitBar(VitBar healthBarComponent)
    {
        healthBar = healthBarComponent;
    }
}
