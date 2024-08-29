using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class MetodosUtiles : MonoBehaviour
{
    //Variables estaticas para evitar llamar a los objetos de forma manual escribiendo el nombre mas de una vez
    public static string melee = "Melee";
    public static string range = "Range";
    public static string siege = "Siege";
    public static string meleeSection = "MeleeSection";
    public static string rangeSection = "RangeSection";
    public static string siegeSection  = "SiegeSection";

   

    //variable para Shuffle
    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(IList<T> list) //Metodo para barajar las cartas de una lista
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void MoveList<T>(T element, List<T> origin, List<T> destiny) //Metodo para mover un elemento de una lista a otra
    {
        destiny.Add(element);
        origin.Remove(element);
    }

    
}
