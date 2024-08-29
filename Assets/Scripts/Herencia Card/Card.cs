using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    [SerializeField]public Sprite Image;
    [SerializeField]public string Title;
    [SerializeField]public E_Effect Effect;
    [SerializeField]public EFaction Faction;


    public enum EFaction
    {
        Empire,
        Oblivion,
        Neutral
    }
    public enum E_Effect
    {
        None, //No efecto
        Buff, //Aumento en fila
        Wheather, //Clima
        Burn,  //Quemadura
        little_Burn, //Quemadura menor
        Steal, //Robar carta
        Companion, // multiplicar por n el ataque de las cartas con el mismo nombre (siendo n la cantidad de cartas)
        Clear, //Limpia la fila con menos unidades
        Average, //Promedio
        Señuelo, //Vamos, el señuel
        Sunny
    }
}
