using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MetodosUtilesUnity : MonoBehaviour //Metodos que no pude dejar solamente en C#
{   
    public static GameObject Horn; //variable necesaria para que funcione los cuernos de guerra, eliminarlo causaria la ruptura de los metodos de cuerno de guerra.


    //Añade las 10 cartas al principio de la partida
    public static void AddToListHand(GameObject origin, GameObject destiny)
    {
        List<GameObject> deck = origin.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> hand = destiny.GetComponent<ListaBaseConGObject>().cards;
        for (int i = 0; i < 10; i++)
        {
            hand.Add(deck[i]);
            deck[i].transform.SetParent(destiny.transform);
        }
        deck.RemoveRange(0,10);
    }

    //Añade una carta desde una lista a un lugar nuevo que contenga una lista (tambien la hace hija del nuevo objeto)
    public static void AddToListOneCard(GameObject card, GameObject origin, GameObject destiny)
    {
        List<GameObject> originList = origin.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> destinyList = destiny.GetComponent<ListaBaseConGObject>().cards;

        destinyList.Add(card);
        originList.Remove(card);
        card.transform.SetParent(destiny.transform);
    }

    //Añade todas las cartas de una seccion a otra (tambien la hace hija de la seccion destino)
    public static void AddToListCards(GameObject origin, GameObject destiny)
    {
        List<GameObject> originList = origin.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> destinyList = destiny.GetComponent<ListaBaseConGObject>().cards;

        foreach (GameObject card in originList)
        {
            if (card.GetComponent<PrefabUnitCard>().UnitType == Unit_Card.EType.Gold.ToString()) continue;

            destinyList.Add(card);
            card.transform.SetParent(destiny.transform);
        }
        
        if (originList.Count > 0)
        {
            for (int i = 0; i < originList.Count; i++)
            {
                if (originList[i].GetComponent<PrefabUnitCard>().UnitType == Unit_Card.EType.Gold.ToString()) continue;

                originList.Remove(originList[i]);
                i--;
            }
        }
    }

    //Añade la carta de la seccion de cuerno (normalmente al cementerio)
    public static void AddToListOneCardHorn(GameObject origin, GameObject destiny)
    {
        CartaBaseSola cartaBaseSola = origin.GetComponent<CartaBaseSola>();

        if (cartaBaseSola.card == null) return;

        List<GameObject> destinyList = destiny.GetComponent<ListaBaseConGObject>().cards;

        destinyList.Add(cartaBaseSola.card);
        cartaBaseSola.card.transform.SetParent(destiny.transform);
        cartaBaseSola.card = null;
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metodo para jugar una carta, se añade a su correspondiente seccion, se llama normalmente desde la clase "OnMouseEnterScript"
    //Requiere de varios metodos para funcionar (Se encuentran debajo de este metodo)
    public static void AddToSection(GameObject card, GameObject player, GameObject hand)
    {
        if (card.GetComponent<PrefabUnitCard>() != null) //Entra si la carta es de tipo unidad
        {
            if (card.GetComponent<PrefabUnitCard>().Effect == Unit_Card.E_Effect.Señuelo.ToString())
            {
                //Entra si la carta es el Señuelo
                AddToSectionDecoy(card);
                return;
            }
            else AddToSectionUnit(card, hand, player);
        }
        else if (card.GetComponent<PrefabWeatherCard>() != null) //Entra si la carta es de tipo clima (Soleado no cuenta como clima)
        {
            AddToSectionWeather(card, hand, player);
        }
        else if (card.GetComponent<PrefabSpecialCard>() != null) //Entra si la carta es de tipo special (soleado esta en este tipo)
        {
            AddToSectionSpecial(card, hand, player);
        }

        GameManager.ChangeTurn(); //Despues de que se juegue una carta, se cambia el turno
    }

    //Metodo que se llama desde el metodo "AddToSection", coloca la carta seleccionada en su seccion de unidad correspondiente
    private static void AddToSectionUnit(GameObject card, GameObject hand, GameObject player)
    {
        PrefabUnitCard prefabUnitCard = card.GetComponent<PrefabUnitCard>(); //Necesario para escribir menos en las condicionales
        GameObject destinySection;
        GameObject preDestiny; //objeto necesario en la jerarquia para llegar al necesario
        List<GameObject> handList = hand.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> destinyList;

        if (prefabUnitCard.Board_Section == Unit_Card.EBoard_Section.Melee.ToString()) //Evalua si la carta es melee
        {
            preDestiny = player.transform.Find(MetodosUtiles.melee).gameObject;
            destinySection = preDestiny.transform.Find(MetodosUtiles.meleeSection).gameObject;
        }
        else if (prefabUnitCard.Board_Section == Unit_Card.EBoard_Section.Range.ToString()) //Evalua si la carta es range
        {
            preDestiny = player.transform.Find(MetodosUtiles.range).gameObject;
            destinySection = preDestiny.transform.Find(MetodosUtiles.rangeSection).gameObject;
        }
        else //Entra si la carta es siege (solamente hay tres tipos de unidades)
        {
            preDestiny = player.transform.Find(MetodosUtiles.siege).gameObject;
            destinySection = preDestiny.transform.Find(MetodosUtiles.siegeSection).gameObject;
        }

        destinyList = destinySection.GetComponent<ListaBaseConGObject>().cards;
        MetodosUtiles.MoveList(card, handList, destinyList);
        card.transform.SetParent(destinySection.transform);
    }

    private static void AddToSectionDecoy(GameObject card)
    {
        GameObject decoyPanel;

        if (card.transform.IsChildOf(GameObject.Find("Player1").transform)) 
        {
            decoyPanel = GameObject.Find("DecoyPanel1");
            decoyPanel.transform.position = GameManager.DecoyPanel1;
        }
        else 
        {
            decoyPanel = GameObject.Find("DecoyPanel2");
            decoyPanel.transform.position = GameManager.DecoyPanel2;
        }
        
        GameManager.EnableDecoy = true;
    }


    //Metodo que se llama desde el metodo "AddToSection", coloca la carta seleccionada la seccion de clima
    private static void AddToSectionWeather(GameObject card, GameObject hand, GameObject player)
    {
        GameObject weatherSection = GameObject.Find("WeatherSection");
        List<GameObject> weatherList = weatherSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> handList = hand.GetComponent<ListaBaseConGObject>().cards;
        
        foreach (GameObject weatherCard in weatherList) //Se verifica si una carta igual ya se encuentra en la seccion
        {
            if (weatherCard.name == card.name)
            {
                //Si entra aqui, entonces hay que descartar la carta al cementerio ya que hay una haciendo el efecto en su seccion
                GameObject graveyard = player.transform.Find("Graveyard").gameObject;
                weatherList = graveyard.GetComponent<ListaBaseConGObject>().cards; //esta lista en un principio es de climas, pero en este caso pasa a ser para el cementerio.
                MetodosUtiles.MoveList(card, handList, weatherList); //weatherList es aqui "graveyardList"
                card.transform.SetParent(graveyard.transform);
                return;
            }
        }

        //Se añade la carta a la zona de climas
        MetodosUtiles.MoveList(card, handList, weatherList);
        card.transform.SetParent(weatherSection.transform);

        if (card.GetComponent<PrefabWeatherCard>().WeatherType == Weather_Card.EWeatherType.Frost.ToString()) GameManager.frostEffect = true;
        if (card.GetComponent<PrefabWeatherCard>().WeatherType == Weather_Card.EWeatherType.Fog.ToString()) GameManager.fogEffect = true;
        if (card.GetComponent<PrefabWeatherCard>().WeatherType == Weather_Card.EWeatherType.Rain.ToString()) GameManager.rainEffect = true;
    }

    //Metodo que se llama desde el metodo "AddToSection", coloca la carta seleccionada en su seccion correspondiente de carta special
    private static void AddToSectionSpecial(GameObject card, GameObject hand, GameObject player)
    {
        PrefabSpecialCard prefabSpecialCard = card.GetComponent<PrefabSpecialCard>();
        List<GameObject> handList = hand.GetComponent<ListaBaseConGObject>().cards;

        if (prefabSpecialCard.SpecialType == Special_Card.EType_Special.Sunny.ToString()||prefabSpecialCard.SpecialType == Special_Card.EType_Special.Burn.ToString()) //Entra si la carta es del tipo Sunny (soleado) o Burn (quemadura)
        {
            //Se encarga de enviar las cartas de este tipo a el cementerio de forma directa
            GameObject graveyard = player.transform.Find("Graveyard").gameObject;
            List<GameObject> graveyardList = graveyard.GetComponent<ListaBaseConGObject>().cards;
            MetodosUtiles.MoveList(card, handList, graveyardList);
            card.transform.SetParent(graveyard.transform);
        }
        else //Entra si la carta es del tipo Horn (Cuerno de Guerra)
        {
            //Solamente llama al panel que contiene los botones que llamaran al metodo para colocar el cuerno de guerra
            GameObject hornPanel = GameObject.Find("HornSelection");
            hornPanel.GetComponent<RectTransform>().position = GameManager.HornPanel;
            Horn = card;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metodo que ubica la carta cuerno seleccionada en su seccion correspondiente, se llama desde los metodos de botones en la zona inferior de la clase.
    public void AddToSectionHorn(int section) 
    {
        GameObject Player;
        GameObject Melee;
        GameObject HornSection;
        CartaBaseSola cartaBaseSola;
        string preSectionName;
        string sectionName;

        if (section == 0)
        {
            preSectionName = "Melee";
            sectionName = "MeleeHorn";
        }
        else if (section == 1)
        {
            preSectionName = "Range";
            sectionName = "RangeHorn";
        }
        else
        {
            preSectionName = "Siege";
            sectionName = "SiegeHorn";
        }


        if (Horn.transform.IsChildOf(GameObject.Find("Player1").transform)) //Entra aqui si la carta cuerno es hija de Player1
        {
            Player = GameObject.Find("Player1").gameObject;
        }
        else //Entra aqui si la carta cuerno es hija de Player2 (no puede estar en otro lado)
        {
            Player = GameObject.Find("Player2").gameObject;
        }
        Melee = Player.transform.Find(preSectionName).gameObject;
        HornSection = Melee.transform.Find(sectionName).gameObject;
        cartaBaseSola = HornSection.GetComponent<CartaBaseSola>();
        cartaBaseSola.card = Horn;
        Horn.transform.SetParent(HornSection.transform);

        if (preSectionName == "Melee") Player.GetComponent<ClaseJugador>().HornMelee = true;
        else if (preSectionName == "Range") Player.GetComponent<ClaseJugador>().HornMelee = true;
        else if (preSectionName == "Siege") Player.GetComponent<ClaseJugador>().HornMelee = true;
        
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Intercambia una carta si el contador de cartas intercambiadas es menor que 3
    public static void SwitchCard(GameObject card)
    {
        //Esta zona es debido a la imnposibilidad de llamar GameObjects de otra clase que no sean estaticos
        GameObject Player1 = GameObject.Find("Player1");
        GameObject Player2 = GameObject.Find("Player2");
        GameObject HandP11 = Player1.transform.Find("Hand").gameObject;
        GameObject HandP22 = Player2.transform.Find("Hand").gameObject;
        GameObject DeckPlayer11 = Player1.transform.Find("Deck").gameObject;
        GameObject DeckPlayer22 = Player2.transform.Find("Deck").gameObject;

        
        GameObject player;
        GameObject gameObjectDeck;
        GameObject gameObjectHand;
        
        if (card.transform.IsChildOf(Player1.transform)) 
        {
            gameObjectDeck = DeckPlayer11;
            gameObjectHand = HandP11;
            player = Player1;
        }
        else
        { 
            gameObjectDeck = DeckPlayer22;
            gameObjectHand = HandP22;
            player = Player2;
        }

        player.GetComponent<ClaseJugador>().CardsChanged++;

        if (player.GetComponent<ClaseJugador>().CardsChanged >= 3) return; // Si la cantidad de cartas cambiadas es 2, impide que siga.

        List<GameObject> deck = gameObjectDeck.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> hand = gameObjectHand.GetComponent<ListaBaseConGObject>().cards;

        //Mueve la carta al deck, la vuelve hija del deck y cambia sus coordenadas debido a que se queda visualmente en la mano
        deck.Add(card);
        card.transform.SetParent(gameObjectDeck.transform);
        hand.Remove(card);
        card.GetComponent<RectTransform>().position = new Vector3(-274.299988f,-4.5999999f,0);

        //Añade otra carta del deck a la mano, la primera de la lista del deck
        hand.Add(deck[0]);
        deck[0].transform.SetParent(gameObjectHand.transform);
        deck.Remove(deck[0]);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Simplemente mueve de la pantalla el objeto al que se le adjunte
    //Metodos para botones
    public void MovePanel()
    {
        GameObject gameObject = this.gameObject;
        gameObject.GetComponent<RectTransform>().position = new Vector3(-1000,-1000,0);
    }

    //Metodo que llama al metodo de colocar cuerno, se activa desde el boton de melee del panel
    public void HornSectionMeleeButton()
    {
        int section = 0;
        AddToSectionHorn(section);
    }

    //Metodo que llama al metodo de colocar cuerno, se activa desde el boton de range del panel
    public void HornSectionRangeButton()
    {
        int section = 1;
        AddToSectionHorn(section);
    }

    //Metodo que llama al metodo de colocar cuerno, se activa desde el boton de siege del panel
    public void HornSectionSiegeButton()
    {
        
        AddToSectionHorn(2);
    }
}
