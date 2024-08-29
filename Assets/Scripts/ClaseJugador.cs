using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaseJugador : MonoBehaviour
{
    public string Faction; //Representa la faccion que el jugador eligio
    public int CardsChanged; //Representa las cartas intercambiadas al principio del juego
    public bool HasPassed; //Representa si el jugador ha pasado su turno
    public int PlayerLife; //Representa las "Vidas" de cada jugador en el juego, si es cero, ese jugador pierde
    public bool ActivateLeader; //Booleano que avisa si el jugador ya jugó su carta de lider
    public bool PlayerTurn; //Si es true entonces es el turno del jugador
    public bool HornMelee; //Booleano que avisa si un jugador tiene un cuerno de guerra en la posicion correspondiente
    public bool HornRange; //Booleano que avisa si un jugador tiene un cuerno de guerra en la posicion correspondiente
    public bool HornSiege; //Booleano que avisa si un jugador tiene un cuerno de guerra en la posicion correspondiente

}
