using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static bool arranque; //Mientras este en false, el juego en si no ha comenzado. Sirve para varias cosas como elegir cartas al comienzo
    public static Vector3 HornPanel= new Vector3(860,543,0); //Vector necesario para que el panel del cuerno de guerra se coloque en la posicion necesaria.
    public static Vector3 DecoyPanel1 = new Vector3(1100,830,0);
    public static Vector3 DecoyPanel2 = new Vector3(1100,280,0);
    public static bool PassP1; //Version estatica que dice si el jugador 1 pasó. Necesario para el metodo de cambio de turnos. Lo modifica el metodo de pasar turno.
    public static bool PassP2; //Version estatica que dice si el jugador 2 pasó. Necesario para el metodo de cambio de turnos Lo modifica el metodo de pasar turno.
    public static int RoundS; //variable que por cual ronda va el juego
    public static bool frostEffect; //Variable que dice si hay una carta de clima escarcha en el campo
    public static bool fogEffect; //Variable que dice si hay una carta de clima niebla en el campo
    public static bool rainEffect; //Variable que dice si hay una carta de clima lluvia en el campo
    public static bool EnableDecoy; //Variable que dice si se está aplicando un señuelo


    public GameObject TotalAttackP1; //Necesario para evaluar los ataques
    public GameObject TotalAttackP2; //Necesario para evaluar los ataques
    public GameObject LifePointsP1;
    public GameObject LifePointsP2;
    public GameObject Player1;
    public GameObject Player2;

    //Necesario para el final de ronda
    public GameObject MeleeSectionP1;
    public GameObject MeleeSectionP2;
    public GameObject RangeSectionP1;
    public GameObject RangeSectionP2;
    public GameObject SiegeSectionP1;
    public GameObject SiegeSectionP2;
    public GameObject HornSectionM1;
    public GameObject HornSectionR1;
    public GameObject HornSectionS1;
    public GameObject HornSectionM2;
    public GameObject HornSectionR2;
    public GameObject HornSectionS2;
    public GameObject WeatherSection;
    public GameObject Graveyard1;
    public GameObject Graveyard2;
    public GameObject DeckP1;
    public GameObject DeckP2;
    public GameObject HandP1;
    public GameObject HandP2;


    //Metodo para arrancar el juego desde el boton de continuar 2
    public void ArrancarJuego()
    {
        arranque = true;
    }
    
    //Metodo para cambiar turnos (en el inspector, el jugador 1 tiene el turno en true y el 2 en false, por tanto este metodo alternará el true de los jugadores)
    public static void ChangeTurn()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        ClaseJugador p1 = player1.GetComponent<ClaseJugador>();
        ClaseJugador p2 = player2.GetComponent<ClaseJugador>();

        if (PassP1 || PassP2) return;
        else 
        {
            p1.PlayerTurn = !p1.PlayerTurn;
            p2.PlayerTurn = !p2.PlayerTurn;
        }
    }

    //Metodo para pasar el turno del jugador 1, se activa en un principio desde el boton de pasar turno respectivo
    public static void PassTurnP1()
    {
        GameObject player1 = GameObject.Find("Player1");
        ClaseJugador p1 = player1.GetComponent<ClaseJugador>();
        if (p1.PlayerTurn)
        {
            if (!p1.HasPassed)
            {
                ChangeTurn();
                p1.HasPassed = true;
                PassP1 = true;
            }   
        }
    }

    //Metodo para pasar el turno del jugador 2, se activa en un principio desde el boton de pasar turno respectivo
    public static void PassTurnP2()
    {
        GameObject player2 = GameObject.Find("Player2");
        ClaseJugador p2 = player2.GetComponent<ClaseJugador>();
        if (p2.PlayerTurn)
        {
            if (!p2.HasPassed)
            {
                ChangeTurn();
                p2.HasPassed = true;
                PassP2 = true;
            }   
        }
    }

    public void EndedRound()
    {
        if (PassP1 && PassP2)
        {
            ClaseJugador claseJugadorP1 = Player1.GetComponent<ClaseJugador>();
            ClaseJugador claseJugadorP2 = Player2.GetComponent<ClaseJugador>();

            EvaluateAttackEndedRound();

            if (claseJugadorP1.PlayerLife == 0 && claseJugadorP2.PlayerLife == 0)
            {
                Debug.Log("Hubo un empate");
                return;
            }
            else if (claseJugadorP1.PlayerLife == 0)
            {
                Debug.Log("Ganó el jugador 2");
                return;
            }
            else if (claseJugadorP2.PlayerLife == 0)
            {
                Debug.Log("Ganó el jugador 1");
                return;
            }

            ClearLists();
            ClearHornSection();
            ResetVariables();
            ResetGraveyardCards(Graveyard1);
            ResetGraveyardCards(Graveyard2);

            ListaBaseConGObject DeckListP1 = DeckP1.GetComponent<ListaBaseConGObject>();
            ListaBaseConGObject DeckListP2 = DeckP2.GetComponent<ListaBaseConGObject>();
            AddTwoCardsToHand(DeckListP1, DeckP1, HandP1);
            AddTwoCardsToHand(DeckListP2, DeckP2, HandP2);
        }
    }

    //Metodo que se llama en EndedRound(), añade una carta a la mano del jugador correspondiente y si puede añade otra más.
    private void AddTwoCardsToHand(ListaBaseConGObject deckList,GameObject deck, GameObject hand)
    {
        if (deckList.cards.Count > 0)
        {
            MetodosUtilesUnity.AddToListOneCard(deckList.cards[0], deck, hand);
            if (deckList.cards.Count != 0)
            {
                MetodosUtilesUnity.AddToListOneCard(deckList.cards[0], deck, hand);
            }
        }
    }

    //Este metodo verifica quien gana la ronda en especifico en el momento que se llame (le resta la vida al que pierde)
    public void EvaluateAttackEndedRound()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        ClaseJugador p1 = player1.GetComponent<ClaseJugador>();
        ClaseJugador p2 = player2.GetComponent<ClaseJugador>();
        int totalAttackP1 = int.Parse(TotalAttackP1.GetComponent<TextMeshProUGUI>().text);
        int totalAttackP2 = int.Parse(TotalAttackP2.GetComponent<TextMeshProUGUI>().text);

        if (totalAttackP1 > totalAttackP2)
        {
            EvaluateAttackEndedRoundTools(Player2);
            p1.PlayerTurn = true;
            p2.PlayerTurn = false;
            
        }
        if (totalAttackP1 < totalAttackP2)
        {
            EvaluateAttackEndedRoundTools(Player1);
            p2.PlayerTurn = true;
            p1.PlayerTurn = false;
        }
        if (totalAttackP1 == totalAttackP2)
        {
            EvaluateAttackEndedRoundTools(Player1);
            EvaluateAttackEndedRoundTools(Player2);
            p1.PlayerTurn = true;
            p2.PlayerTurn = false;
        }
    }

    //Metodo que se llama desde el metodo EvaluateAttackEndedRound, hace el trabajo de restar la vida al jugador correspondiente
    private void EvaluateAttackEndedRoundTools(GameObject player)
    {
        ClaseJugador claseJugador;
        GameObject LifePoints;
        if (player == Player1)
        {
            claseJugador = Player1.GetComponent<ClaseJugador>();
            LifePoints = LifePointsP1;
        }
        else
        {
            claseJugador = Player2.GetComponent<ClaseJugador>();
            LifePoints = LifePointsP2;
        }
        claseJugador.PlayerLife--;
        LifePoints.GetComponent<TextMeshProUGUI>().text = claseJugador.PlayerLife.ToString();
    }


    //Metodo que despeja las secciones de unidades del terreno y envia las cartas al cementerio
    public void ClearLists()
    {
        //Player1
        List<GameObject> M1 = MeleeSectionP1.GetComponent<ListaBaseConGObject>().cards;
        ClearList(M1, MeleeSectionP1, Graveyard1);

        List<GameObject> R1 = RangeSectionP1.GetComponent<ListaBaseConGObject>().cards;
        ClearList(R1, RangeSectionP1, Graveyard1);

        List<GameObject> S1 = SiegeSectionP1.GetComponent<ListaBaseConGObject>().cards;
        ClearList(S1, SiegeSectionP1, Graveyard1);
        ////////////////////////////////////////////////////////////////////////////////////
        //Player2
        List<GameObject> M2 = MeleeSectionP2.GetComponent<ListaBaseConGObject>().cards;
        ClearList(M2, MeleeSectionP2, Graveyard2);

        List<GameObject> R2 = RangeSectionP2.GetComponent<ListaBaseConGObject>().cards;
        ClearList(R2, RangeSectionP2, Graveyard2);

        List<GameObject> S2 = SiegeSectionP2.GetComponent<ListaBaseConGObject>().cards;
        ClearList(S2, SiegeSectionP2, Graveyard2);
        
        //WeatherSection
        List<GameObject> W = WeatherSection.GetComponent<ListaBaseConGObject>().cards;
        ClearList(W, WeatherSection, Graveyard2);
    }

    //Metodo que se llama en el metodo ClearLists(), despeja la seccion indicada
    private void ClearList(List<GameObject> list,GameObject section, GameObject graveyard)
    {
        while (list.Count > 0) MetodosUtilesUnity.AddToListOneCard(list[0], section, graveyard);
    }

    //Metodo que despeja las secciones de cuerno de guerra y los envia al cementerio
    public void ClearHornSection()
    {
        MetodosUtilesUnity.AddToListOneCardHorn(HornSectionM1, Graveyard1);
        MetodosUtilesUnity.AddToListOneCardHorn(HornSectionR1, Graveyard1);
        MetodosUtilesUnity.AddToListOneCardHorn(HornSectionS1, Graveyard1);

        MetodosUtilesUnity.AddToListOneCardHorn(HornSectionM2, Graveyard2);
        MetodosUtilesUnity.AddToListOneCardHorn(HornSectionR2, Graveyard2);
        MetodosUtilesUnity.AddToListOneCardHorn(HornSectionS2, Graveyard2);
    }
    
    //Metodo que resetea las cariables de pasar turno
    public void ResetVariables()
    {
        PassP1 = false;
        PassP2 = false;

        Player1.GetComponent<ClaseJugador>().HasPassed = false;
        Player1.GetComponent<ClaseJugador>().HornMelee = false;
        Player1.GetComponent<ClaseJugador>().HornRange = false;
        Player1.GetComponent<ClaseJugador>().HornSiege = false;

        Player2.GetComponent<ClaseJugador>().HasPassed = false;
        Player2.GetComponent<ClaseJugador>().HornMelee = false;
        Player2.GetComponent<ClaseJugador>().HornRange = false;
        Player2.GetComponent<ClaseJugador>().HornSiege = false;

        frostEffect = false;
        fogEffect = false;
        rainEffect = false;
        EnableDecoy = false;
    }

    //Metodo que restaura los ataques originales de cada carta en el cementerio despues de un turno. Se llama desde EndedRound()
    public void ResetGraveyardCards(GameObject graveyard)
    {
        List<GameObject> cardList = graveyard.GetComponent<ListaBaseConGObject>().cards;
        foreach (GameObject card in cardList)
        {
            if (card.GetComponent<PrefabUnitCard>() != null) card.GetComponent<PrefabUnitCard>().Attack = card.GetComponent<PrefabUnitCard>().Backup_Atack;
        }
    }

}
