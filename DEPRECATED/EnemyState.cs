using System.Collections;
using TMPro;
using UnityEngine;

// **** Enemy Class

public class EnemyState : CharacterState
{
    public void SetVitBar(VitBar healthBarComponent)
    {
        healthBar = healthBarComponent;
    }
}
