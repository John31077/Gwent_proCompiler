using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class UtilesDeInstanciar : MonoBehaviour
{
    string oblivionFaction = "Oblivion"; 
    string empireFaction = "Empire";
    public GameObject Player1;
    public GameObject HandP1;
    public GameObject EmpireButton1;
    public GameObject OblivionButton1;
    public GameObject Player2;
    public GameObject HandP2;
    public GameObject EmpireButton2;
    public GameObject OblivionButton2;


    public GameObject scriptableDeckEmpire;
    public GameObject scriptableDeckOblivion;

    public GameObject DeckPlayer1;
    public GameObject DeckPlayer2;


    //En esta seccion se encuentra la base para instanciar los prefabs (el prefab y su correspondiente prefab instanciado)
    public GameObject UnitCardPrefab;
    private GameObject InstUnitCardPrefab; //No modificar desde el inspector
    public GameObject WeatherCardPrefab;
    private GameObject InstWeatherCardPrefab; //No modificar desde el inspector
    public GameObject SpecialCardPrefab;
    private GameObject InstSpecialCardPrefab; //No modificar desde el inspector
    public GameObject LeaderCardPrefab;
    private GameObject InstLeaderCardPrefab; //No modificar desde el inspector

    //En esta seccion se encuentra los scriptable objects de los lideres de cada faccion para llamarlos a su seccion correspondiente
    public Card EmpireLeader;
    public Card OblivionLeader;

    //Objeto donde se encuentra la seccion del lider
    public GameObject LeaderSectionP1;
    public GameObject LeaderSectionP2;

    //Imagenes de los deck de cada faccion
    public Sprite spriteOblivionDeck;
    public Sprite spriteEmpireDeck;
    

    //Simplemente coloca la faccion del deck que tendra cada jugador mediante los botones (solamente se llama en los botones de elegir deck)
    public void SetFactionToPlayer() 
    {
        if (!EmpireButton1.activeSelf) Player1.GetComponent<ClaseJugador>().Faction = empireFaction;
        if (!OblivionButton1.activeSelf) Player1.GetComponent<ClaseJugador>().Faction = oblivionFaction;
        if (!EmpireButton2.activeSelf) Player2.GetComponent<ClaseJugador>().Faction = empireFaction;
        if (!OblivionButton2.activeSelf) Player2.GetComponent<ClaseJugador>().Faction = oblivionFaction;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metodo dependiente de los dos que se encuentran debajo, crea los deck para cada jugador y coloca sus lideres.
    public void CreateDeck()
    {
        CreateDeck(Player1, DeckPlayer1);
        MetodosUtiles.Shuffle(DeckPlayer1.GetComponent<ListaBaseConGObject>().cards);
        MetodosUtilesUnity.AddToListHand(DeckPlayer1, HandP1);
        CreateDeck(Player2, DeckPlayer2);
        MetodosUtiles.Shuffle(DeckPlayer2.GetComponent<ListaBaseConGObject>().cards);
        MetodosUtilesUnity.AddToListHand(DeckPlayer2, HandP2);
    }
    private void CreateDeck(GameObject player, GameObject playerDeck)
    {
        string playerFaction = player.GetComponent<ClaseJugador>().Faction;
        if (playerFaction == empireFaction) Instanciar(scriptableDeckEmpire, playerDeck);    
        else Instanciar(scriptableDeckOblivion, playerDeck);
        InstanciarLider(playerFaction, player);
        InstanciarIconDeck(player, playerDeck, playerFaction);
    }
    private void Instanciar(GameObject scriptList, GameObject playerDeck)
    {
        List<Card> scriptCards = scriptList.GetComponent<ListaBase>().PseudoDeck;
        foreach (Card scriptCard in scriptCards)
        {
            if (scriptCard is Unit_Card)
            {
                InstUnitCardPrefab = Instantiate(UnitCardPrefab);
                InstUnitCardPrefab.name = scriptCard.Title;
                Unit_Card unit_Card = (Unit_Card)scriptCard;
                UnityEngine.UI.Image image = InstUnitCardPrefab.GetComponent<UnityEngine.UI.Image>();
                image.sprite = unit_Card.Image;
                PrefabUnitCard prefabUnitCard = InstUnitCardPrefab.GetComponent<PrefabUnitCard>();
                prefabUnitCard.ImageBackup = image.sprite;
                prefabUnitCard.Faction = unit_Card.Faction.ToString();
                prefabUnitCard.Board_Section = unit_Card.Board_Section.ToString();
                prefabUnitCard.UnitType = unit_Card.Type.ToString();
                prefabUnitCard.Effect = unit_Card.Effect.ToString();
                prefabUnitCard.Attack = unit_Card.Attack;
                prefabUnitCard.Backup_Atack = unit_Card.Attack;
                playerDeck.GetComponent<ListaBaseConGObject>().cards.Add(InstUnitCardPrefab);
                InstUnitCardPrefab.transform.SetParent(playerDeck.transform);
            }
            else if (scriptCard is Weather_Card)
            {
                InstWeatherCardPrefab = Instantiate(WeatherCardPrefab);
                InstWeatherCardPrefab.name = scriptCard.Title;
                Weather_Card weather_Card = (Weather_Card)scriptCard;
                UnityEngine.UI.Image image = InstWeatherCardPrefab.GetComponent<UnityEngine.UI.Image>();
                image.sprite = weather_Card.Image;
                PrefabWeatherCard prefabWeatherCard = InstWeatherCardPrefab.GetComponent<PrefabWeatherCard>();
                prefabWeatherCard.ImageBackup = image.sprite;
                prefabWeatherCard.Faction = weather_Card.Faction.ToString();
                prefabWeatherCard.WeatherType = weather_Card.WeatherType.ToString();
                prefabWeatherCard.Effect = weather_Card.Effect.ToString();
                playerDeck.GetComponent<ListaBaseConGObject>().cards.Add(InstWeatherCardPrefab);
                InstWeatherCardPrefab.transform.SetParent(playerDeck.transform);
            }
            else if (scriptCard is Special_Card)
            {
                InstSpecialCardPrefab = Instantiate(SpecialCardPrefab);
                InstSpecialCardPrefab.name = scriptCard.Title;
                Special_Card special_Card = (Special_Card)scriptCard;
                UnityEngine.UI.Image image = InstSpecialCardPrefab.GetComponent<UnityEngine.UI.Image>();
                image.sprite = special_Card.Image;
                PrefabSpecialCard prefabSpecialCard = InstSpecialCardPrefab.GetComponent<PrefabSpecialCard>();
                prefabSpecialCard.ImageBackup = image.sprite;
                prefabSpecialCard.Faction = special_Card.Faction.ToString();
                prefabSpecialCard.SpecialType = special_Card.Type_Special.ToString();
                prefabSpecialCard.Effect = special_Card.Effect.ToString();
                playerDeck.GetComponent<ListaBaseConGObject>().cards.Add(InstSpecialCardPrefab);
                InstSpecialCardPrefab.transform.SetParent(playerDeck.transform);
            }
        }
    }
    private void InstanciarIconDeck(GameObject player, GameObject playerDeck, string faction)
    {
        GameObject deck;
        if (player == Player1)
        {
            deck = DeckPlayer1;
        }
        else
        {
            deck = DeckPlayer2;
        }
        UnityEngine.UI.Image deckImage = deck.GetComponent<UnityEngine.UI.Image>();
        if (faction == empireFaction)
        {
            deckImage.sprite = spriteEmpireDeck;
        }
        else
        {
            deckImage.sprite = spriteOblivionDeck;
        }
    }
    private void InstanciarLider(string faction, GameObject player) //Metodo que se dedica a instanciar los lideres de cada jugador
    {
        Card Leader;
        GameObject leaderSection;

        if (faction == empireFaction) Leader = EmpireLeader;
        else Leader = OblivionLeader;

        if (player == Player1) leaderSection = LeaderSectionP1;
        else leaderSection = LeaderSectionP2;

        InstLeaderCardPrefab = Instantiate(LeaderCardPrefab);
        InstLeaderCardPrefab.name = Leader.name;
        UnityEngine.UI.Image image = InstLeaderCardPrefab.GetComponent<UnityEngine.UI.Image>();
        image.sprite = Leader.Image;
        UnityEngine.UI.Image sectionImage = leaderSection.GetComponent<UnityEngine.UI.Image>();
        sectionImage.sprite = Leader.Image;
        
        PrefabLeaderCard prefabLeaderCard = InstLeaderCardPrefab.GetComponent<PrefabLeaderCard>();
        prefabLeaderCard.ImageBackup = Leader.Image;
        prefabLeaderCard.Effect = Leader.Effect.ToString();
        leaderSection.GetComponent<CartaBaseSola>().card = InstLeaderCardPrefab;
        InstLeaderCardPrefab.transform.SetParent(leaderSection.transform);
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
}
