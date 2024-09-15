using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

class EffectCreation : MonoBehaviour
{
    //Diccionario con los identificadores con sus respectivas expresiones (se añaden los elementos en el Assign)
    public static Dictionary<string, Expression> identifiers = new Dictionary<string, Expression>();
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static GameObject player1 = GameObject.Find("Player1");
    public static GameObject preSectionM1 = player1.transform.Find("Melee").gameObject;
    public static GameObject preSectionR1 = player1.transform.Find("Range").gameObject;
    public static GameObject preSectionS1 = player1.transform.Find("Siege").gameObject;

    public static GameObject handP1 = player1.transform.Find("Hand").gameObject;
    public static GameObject deckP1 = player1.transform.Find("Deck").gameObject;
    public static GameObject graveyard1 = player1.transform.Find("Graveyard").gameObject;
    public static GameObject meleeSection1 = preSectionM1.transform.Find("MeleeSection").gameObject;
    public static GameObject rangeSection1 = preSectionR1.transform.Find("RangeSection").gameObject;
    public static GameObject siegeSection1 = preSectionM1.transform.Find("SiegeSection").gameObject;
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static GameObject player2 = GameObject.Find("Player2");
    public static GameObject preSectionM2 = player2.transform.Find("Melee").gameObject;
    public static GameObject preSectionR2 = player2.transform.Find("Range").gameObject;
    public static GameObject preSectionS2 = player2.transform.Find("Siege").gameObject;

    public static GameObject handP2 = player2.transform.Find("Hand").gameObject;
    public static GameObject deckP2 = player2.transform.Find("Deck").gameObject;
    public static GameObject graveyard2 = player2.transform.Find("Graveyard").gameObject;
    public static GameObject meleeSection2 = preSectionM2.transform.Find("MeleeSection").gameObject;
    public static GameObject rangeSection2 = preSectionR2.transform.Find("RangeSection").gameObject;
    public static GameObject siegeSection2 = preSectionM2.transform.Find("SiegeSection").gameObject;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Listas P1
    public static List<GameObject> h1 = handP1.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> deck1 = deckP1.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> g1 = graveyard1.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> m1 = meleeSection1.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> r1 = rangeSection1.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> s1 = siegeSection1.GetComponent<ListaBaseConGObject>().cards;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Listas P2
    public static List<GameObject> h2 = handP2.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> deck2 = deckP2.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> g2 = graveyard2.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> m2 = meleeSection2.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> r2 = rangeSection2.GetComponent<ListaBaseConGObject>().cards;
    public static List<GameObject> s2 = siegeSection2.GetComponent<ListaBaseConGObject>().cards;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Lista General (Board)
    public static List<GameObject> board = new List<GameObject>();

    


    public static void BoardList()
    {
        List<GameObject>[] lists = new List<GameObject>[12]
        {h1, deck1, g1, m1, r1, s1, h2, deck2, g2, m2, r2, s2};

        foreach (List<GameObject> list in lists)
        {
            foreach (GameObject card in list)
            {
                board.Add(card);
            }
        }
    }

    public static List<GameObject> FieldOfPlayerList(string player)
    {
        List<GameObject> fieldList = new List<GameObject>();
        if (player == player1.name)
        {
            foreach (GameObject card in m1)
            {
                fieldList.Add(card);
            }
            foreach (GameObject card in r1)
            {
                fieldList.Add(card);
            }
            foreach (GameObject card in s1)
            {
                fieldList.Add(card);
            }
        }
        else 
        {
            foreach (GameObject card in m2)
            {
                fieldList.Add(card);
            }
            foreach (GameObject card in r2)
            {
                fieldList.Add(card);
            }
            foreach (GameObject card in s2)
            {
                fieldList.Add(card);
            }
        }
        return fieldList;
    }

    public static string VerificatePlayer()
    {
        ClaseJugador jugador1 = player1.GetComponent<ClaseJugador>();
        string triggerPlayer = null;
        if (jugador1.PlayerTurn) triggerPlayer = player1.name;
        else triggerPlayer = player2.name;
        return triggerPlayer;
    } 










    public static void ApplyEffect(GameObject card)
    {

    }












    public static void NewWhile(Expression condition, List<ASTNode> instructions)
    {
        condition.Evaluate();
        bool booleanCondition = (bool)condition.Value;


        while (booleanCondition)
        {

        }
    }
}