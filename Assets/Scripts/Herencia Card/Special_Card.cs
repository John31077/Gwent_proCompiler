using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Special Card", menuName = "Special Card")]
public class Special_Card : Card
{
    [SerializeField]public EType_Special Type_Special;
    public enum EType_Special
    {
        Sunny,
        Horn,
        Burn
    }
}
