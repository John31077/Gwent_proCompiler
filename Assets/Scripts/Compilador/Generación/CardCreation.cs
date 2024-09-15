using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class CardCreation : MonoBehaviour
{
    public Sprite sprite;
    
    public void GenerateCode()
    {
        GenerateCode(Programa.elementalProgram);
    }
    public void GenerateCode(ElementalProgram elementalProgram)
    {
        if (!Programa.ValidToGenerate) return;

        Dictionary<string, CardG> cards = elementalProgram.Cards;
        Dictionary<string, Effect> effects = elementalProgram.Effects;


        foreach (var card in cards)
        {
            GenerateCard(card.Key, card.Value);
        }

    }

    private void GenerateCard(string name, CardG card)
    {

        GameObject player1 = GameObject.Find("Player1");
        GameObject handP1 = player1.transform.Find("Hand").gameObject;
        GameObject deckP1 = player1.transform.Find("Deck").gameObject;
        GameObject leaderS1 = player1.transform.Find("LeaderSection").gameObject;

        GameObject player2 = GameObject.Find("Player2");
        GameObject handP2 = player2.transform.Find("Hand").gameObject;
        GameObject deckP2 = player2.transform.Find("Deck").gameObject;
        GameObject leaderS2 = player2.transform.Find("LeaderSection").gameObject;


        ListaBaseConGObject deck1 = deckP1.GetComponent<ListaBaseConGObject>();
        ListaBaseConGObject handList1 = handP1.GetComponent<ListaBaseConGObject>();
        ListaBaseConGObject deck2 = deckP2.GetComponent<ListaBaseConGObject>();
        ListaBaseConGObject handList2 = handP2.GetComponent<ListaBaseConGObject>();

        string faction1 = VerificateFaction(deck1);
        string faction2 = VerificateFaction(deck2);


        if (card.Type == "Oro" || card.Type == "Plata")
        {
            if (faction1 == faction2)
            {
                if (card.faction != faction1) return;

                GameObject card1 = InstantiateUnitCard(name, card);
                GameObject card2 = InstantiateUnitCard(name, card);

                deck1.cards.Add(card1);
                card1.transform.SetParent(deckP1.transform);

                deck2.cards.Add(card2);
                card2.transform.SetParent(deckP2.transform);

            }
            else if (faction1 != faction2)
            {
                GameObject card1 = InstantiateUnitCard(name, card);

                if (card.faction == faction1) 
                {
                    deck1.cards.Add(card1);
                    card1.transform.SetParent(deckP1.transform);
                }
                else if (card.faction == faction2) 
                {
                    deck2.cards.Add(card1);
                    card1.transform.SetParent(deckP1.transform);
                }
            }
            
            foreach (ASTNode onActElement in card.OnActivation)
            {
                if (onActElement is EffectOnActivation)
                {
                    EffectOnActivation effect = (EffectOnActivation)onActElement;
                    DictionaryEffects.EffectsDictionary.Add(effect.Id, EffectCreation.ApplyEffect);
                }
            }
        }
        else if (card.Type == "Clima")
        {
            GameObject card1 = InstantiateWeatherCard(name, card);
            GameObject card2 = InstantiateWeatherCard(name, card);

            deck1.cards.Add(card1);
            card1.transform.SetParent(deckP1.transform);

            deck2.cards.Add(card2);
            card2.transform.SetParent(deckP2.transform);
        }
        else if (card.Type == "Aumento")
        {
            GameObject card1 = InstantiateHornCard(name, card);
            GameObject card2 = InstantiateHornCard(name, card);

            deck1.cards.Add(card1);
            card1.transform.SetParent(deckP1.transform);

            deck2.cards.Add(card2);
            card2.transform.SetParent(deckP2.transform);
        }
        else if (card.Type == "Lider")
        {
            if (faction1 == faction2)
            {
                if (card.faction != faction1) return;

                GameObject card1 = InstantiateLeaderCard(name, card);
                GameObject card2 = InstantiateLeaderCard(name, card);

                leaderS1.GetComponent<CartaBaseSola>().card = card1;
                card1.transform.SetParent(leaderS1.transform);

                leaderS2.GetComponent<CartaBaseSola>().card = card2;
                card2.transform.SetParent(leaderS2.transform);

                return;
            }
            else if (faction1 != faction2)
            {
                GameObject card1 = InstantiateUnitCard(name, card);

                if (card.faction == faction1) 
                {
                    leaderS1.GetComponent<CartaBaseSola>().card = card1;
                    card1.transform.SetParent(leaderS1.transform);
                
                }
                else if (card.faction == faction2) 
                {
                    leaderS2.GetComponent<CartaBaseSola>().card = card1;
                    card1.transform.SetParent(leaderS2.transform);
                }
                return;
            }
        }


        AddToListCompilerCard(handP1, deckP1);
        AddToListCompilerCard(handP2, deckP2);
        MetodosUtiles.Shuffle(deck1.GetComponent<ListaBaseConGObject>().cards);
        MetodosUtiles.Shuffle(deck2.GetComponent<ListaBaseConGObject>().cards);
        MetodosUtilesUnity.AddToListHand(deckP1, handP1);
        MetodosUtilesUnity.AddToListHand(deckP2, handP2);
    }

    public GameObject InstantiateUnitCard(string name, CardG card)
    {
            GameObject generatedCard;
            GameObject unitCard = Resources.Load<GameObject>("UnitCardPrefab");

            generatedCard = Instantiate(unitCard);
            generatedCard.name = name;
            UnityEngine.UI.Image image = generatedCard.GetComponent<UnityEngine.UI.Image>();
            image.sprite = sprite;
            PrefabUnitCard prefabUnitCard = generatedCard.GetComponent<PrefabUnitCard>();
            prefabUnitCard.UnitType = card.Type;
            prefabUnitCard.Faction = card.faction;
            card.Power.Evaluate();
            double power = (double)card.Power.Value;
            prefabUnitCard.Attack = (int)power;
            prefabUnitCard.Backup_Atack = prefabUnitCard.Attack;
            prefabUnitCard.Board_Section = card.range;

            return generatedCard;
    }

    public GameObject InstantiateWeatherCard(string name, CardG card)
    {
        GameObject generatedCard;
        GameObject weatherCard = Resources.Load<GameObject>("WeatherCardPrefab");

        generatedCard = Instantiate(weatherCard);
        generatedCard.name = name;
        UnityEngine.UI.Image image = generatedCard.GetComponent<UnityEngine.UI.Image>();
        image.sprite = sprite;
        PrefabWeatherCard prefabWeatherCard = generatedCard.GetComponent<PrefabWeatherCard>();

        if (card.range == "Melee") prefabWeatherCard.WeatherType = Weather_Card.EWeatherType.Frost.ToString();
        else if (card.range == "Ranged") prefabWeatherCard.WeatherType = Weather_Card.EWeatherType.Fog.ToString();
        else prefabWeatherCard.WeatherType = Weather_Card.EWeatherType.Rain.ToString();

        prefabWeatherCard.Faction = "Neutral";

        return generatedCard;

    }

    public GameObject InstantiateHornCard(string name, CardG card)
    {
        GameObject generatedCard;
        GameObject hornCard = Resources.Load<GameObject>("SpecialCardPrefab");

        generatedCard = Instantiate(hornCard);
        generatedCard.name = name;
        UnityEngine.UI.Image image = generatedCard.GetComponent<UnityEngine.UI.Image>();
        image.sprite = sprite;
        PrefabSpecialCard prefabSpecialCard = generatedCard.GetComponent<PrefabSpecialCard>();
        prefabSpecialCard.SpecialType = Special_Card.EType_Special.Horn.ToString();

        return generatedCard;
    }

    private GameObject InstantiateLeaderCard(string name, CardG card) 
    {
        GameObject generatedCard;
        GameObject leaderCard = Resources.Load<GameObject>("LeaderCardPrefab");

        generatedCard = Instantiate(leaderCard);
        generatedCard.name = name;
        UnityEngine.UI.Image image = generatedCard.GetComponent<UnityEngine.UI.Image>();
        image.sprite = sprite;

        return generatedCard;
    }


    public string VerificateFaction(ListaBaseConGObject listaBaseConGObject)
    {
        string faction = null;
        foreach (GameObject deckCard in listaBaseConGObject.cards)
            {
                if (deckCard.GetComponent<PrefabUnitCard>())
                {
                    if (deckCard.GetComponent<PrefabUnitCard>().Faction == "Empire" || deckCard.GetComponent<PrefabUnitCard>().Faction == "Oblivion")
                    {
                        faction = deckCard.GetComponent<PrefabUnitCard>().Faction;
                        break;
                    }
                }
            }
            return faction;
    }


    private void AddToListCompilerCard(GameObject origin, GameObject destiny)
    {
        List<GameObject> originList = origin.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> destinyList = destiny.GetComponent<ListaBaseConGObject>().cards;

        foreach (GameObject card in originList)
        {
            destinyList.Add(card);
            card.transform.SetParent(destiny.transform);
            card.transform.position = new Vector3(0,0,0);
        }
        originList.Clear();
    }
}


