using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Card", menuName = "Unit Card")]
public class Unit_Card : Card
{
    public int Attack;
    public EBoard_Section Board_Section;
    public EType Type;

    public enum EBoard_Section
    {
        Melee,
        Range,
        Siege,
        Decoy
    }
    public enum EType
    {
        Gold,
        Silver
    }
}
