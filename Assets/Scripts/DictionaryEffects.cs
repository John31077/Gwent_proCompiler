using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryEffects : MonoBehaviour
{
    //Delegado para poder colocar metodos dentro del diccionario
    public delegate void Effect(GameObject card);
    public static Dictionary<string, Effect> EffectsDictionary = new Dictionary<string, Effect>(); //Diccionario que contiene todos los effectos con su nombre como clave


    //Metodo que se llama normalmente desde el boton Continuar despues de elegir los deck. Añade los effectos al diccionario.
    public void AddToDictionary()
    {
        EffectsDictionary.Add("Burn", Effects.Burn);  
        EffectsDictionary.Add("little_Burn", Effects.LittleBurn);
        EffectsDictionary.Add("Steal", Effects.StealEffect);  
        EffectsDictionary.Add("Average", Effects.Average);
        EffectsDictionary.Add("Clear", Effects.Clear);
        EffectsDictionary.Add("Companion", Effects.Companion);
        EffectsDictionary.Add("Sunny", Effects.SunnyEffect);
        
    }
}
