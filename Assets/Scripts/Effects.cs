using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static GameObject player1;
    public static GameObject hand1;
    public static GameObject deck1;
    public static GameObject P1MeleeSection;
    public static GameObject P1RangeSection;
    public static GameObject P1SiegeSection;
    
    public static GameObject player2;
    public static GameObject hand2;
    public static GameObject deck2;
    public static GameObject P2MeleeSection;
    public static GameObject P2RangeSection;
    public static GameObject P2SiegeSection;

    public static GameObject Graveyard1;
    public static GameObject Graveyard2;

    public static GameObject WeatherSection;



    //Metodo que se llama cada vez que una carta carta se coloca en el campo (llama a su respectivo effecto)
    public static void CardEffect(GameObject card)
    {
        if (P1MeleeSection == null) //Entra si las variables estaticas de la clase estan vacias (normalmente si la primera lo está, todas lo estarán)
        {
            player1 = GameObject.Find("Player1");
            GameObject HandP1 = player1.transform.Find("Hand").gameObject;
            GameObject deckP1 = player1.transform.Find("Deck").gameObject;
            GameObject preMeleeSectionP1 = player1.transform.Find("Melee").gameObject;
            GameObject preRangeSectionP1 = player1.transform.Find("Range").gameObject;
            GameObject preSiegeSectionP1 = player1.transform.Find("Siege").gameObject;

            hand1 = HandP1;
            deck1 = deckP1;
            P1MeleeSection = preMeleeSectionP1.transform.Find("MeleeSection").gameObject;
            P1RangeSection = preRangeSectionP1.transform.Find("RangeSection").gameObject;
            P1SiegeSection = preSiegeSectionP1.transform.Find("SiegeSection").gameObject;
            Graveyard1 = player1.transform.Find("Graveyard").gameObject;

            player2 = GameObject.Find("Player2");
            GameObject HandP2 = player2.transform.Find("Hand").gameObject;
            GameObject deckP2 = player2.transform.Find("Deck").gameObject;
            GameObject preMeleeSectionP2 = player2.transform.Find("Melee").gameObject;
            GameObject preRangeSectionP2 = player2.transform.Find("Range").gameObject;
            GameObject preSiegeSectionP2 = player2.transform.Find("Siege").gameObject;

            hand2 = HandP2;
            deck2 = deckP2;
            P2MeleeSection = preMeleeSectionP2.transform.Find("MeleeSection").gameObject;
            P2RangeSection = preRangeSectionP2.transform.Find("RangeSection").gameObject;
            P2SiegeSection = preSiegeSectionP2.transform.Find("SiegeSection").gameObject;
            Graveyard2 = player2.transform.Find("Graveyard").gameObject;

            WeatherSection = GameObject.Find("WeatherSection");
        }
        
        PrefabCard prefabCard = card.GetComponent<PrefabCard>();

        if (DictionaryEffects.EffectsDictionary.TryGetValue(prefabCard.Effect, out DictionaryEffects.Effect effect))
        {
            effect(card);
        }
    }



    //Effecto conocido como quemadura, destruye la mayor carta del campo en ataque
    public static void Burn(GameObject card)
    {
        List<GameObject> m1 = P1MeleeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> r1 = P1RangeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> s1 = P1SiegeSection.GetComponent<ListaBaseConGObject>().cards;

        List<GameObject> m2 = P2MeleeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> r2 = P2RangeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> s2 = P2SiegeSection.GetComponent<ListaBaseConGObject>().cards;

        int M1MaxAttack = MaxAttack(m1);
        int R1MaxAttack = MaxAttack(r1);
        int S1MaxAttack = MaxAttack(s1);

        int M2MaxAttack = MaxAttack(m2);
        int R2MaxAttack = MaxAttack(r2);
        int S2MaxAttack = MaxAttack(s2);

        int P1MaxAttackTotal = Math.Max(M1MaxAttack, Math.Max(R1MaxAttack, S1MaxAttack));
        int P2MaxAttackTotal = Math.Max(M2MaxAttack, Math.Max(R2MaxAttack, S2MaxAttack));

        int TotalMaxAttack = Math.Max(P1MaxAttackTotal, P2MaxAttackTotal);

        RemoveCardBurn(TotalMaxAttack, m1, P1MeleeSection, Graveyard1);
        RemoveCardBurn(TotalMaxAttack, r1, P1RangeSection, Graveyard1);
        RemoveCardBurn(TotalMaxAttack, s1, P1SiegeSection, Graveyard1);

        RemoveCardBurn(TotalMaxAttack, m2, P2MeleeSection, Graveyard2);
        RemoveCardBurn(TotalMaxAttack, r2, P2RangeSection, Graveyard2);
        RemoveCardBurn(TotalMaxAttack, s2, P2SiegeSection, Graveyard2);
    }

    //Metodo que se llama en Burn(), saca la carta con mayor ataque de una seccion
    private static int MaxAttack(List<GameObject> list)
    {
        if (list.Count > 0)
        {
            GameObject x = list[0];
            PrefabUnitCard prefabUnitCard = x.GetComponent<PrefabUnitCard>();
            int result = prefabUnitCard.Attack;
            PrefabUnitCard prefabUnitCard2;


            if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) result = 0;

            foreach (GameObject card in list)
            {
                prefabUnitCard2 = card.GetComponent<PrefabUnitCard>();

                if (prefabUnitCard2.UnitType == Unit_Card.EType.Gold.ToString()) continue;

                if (result < prefabUnitCard2.Attack)
                {
                    result = prefabUnitCard2.Attack;
                }
            } 
            return result;
        }
        return 0;
    }

    //Metodo que se llama desde Burn(), se encarga de remover la carta con mayor ataque del campo en si.
    private static void RemoveCardBurn(int maxAttack, List<GameObject> list, GameObject section, GameObject graveyard)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PrefabUnitCard prefabUnitCard = list[i].GetComponent<PrefabUnitCard>();

            if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) continue;

            if (maxAttack == prefabUnitCard.Attack)
            {
                MetodosUtilesUnity.AddToListOneCard(list[i], section, graveyard);
                i--;
            }       
        }
    }

    //Aplica el effecto de eliminar la carta con menor ataque del campo del rival
    public static void LittleBurn(GameObject card)
    {
        GameObject meleeSection;
        GameObject rangeSection;
        GameObject siegeSection;
        GameObject graveyard;

        if (card.transform.IsChildOf(player1.transform))
        {
            meleeSection = P2MeleeSection;
            rangeSection = P2RangeSection;
            siegeSection = P2SiegeSection;
            graveyard = Graveyard2;
        }
        else
        {
            meleeSection = P1MeleeSection;
            rangeSection = P1RangeSection;
            siegeSection = P1SiegeSection;
            graveyard = Graveyard1;
        }

        List<GameObject> meleeList = meleeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> rangeList = rangeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> siegeList = siegeSection.GetComponent<ListaBaseConGObject>().cards;

        int MeleeMinAttack = MinAttack(meleeList);
        int RangeMinAttack = MinAttack(rangeList);
        int SiegeMinAttack = MinAttack(siegeList);

        int totalMin = MinValueNoCero(MeleeMinAttack, RangeMinAttack, SiegeMinAttack);

        RemoveCardLittleBurn(totalMin, meleeList, meleeSection, graveyard);
        RemoveCardLittleBurn(totalMin, rangeList, rangeSection, graveyard);
        RemoveCardLittleBurn(totalMin, siegeList, siegeSection, graveyard);
    }

    //Metodo que se llama en el metodo LittleBurn(), devuelve un int con el ataque menor de la lista de cartas dadas.
    private static int MinAttack(List<GameObject> list)
    {
        if (list.Count > 0)
        {
            GameObject x = list[0];
            PrefabUnitCard prefabUnitCard = x.GetComponent<PrefabUnitCard>();
            int result = prefabUnitCard.Attack;
            PrefabUnitCard prefabUnitCard2;
            foreach (GameObject card in list)
            {
                prefabUnitCard2 = card.GetComponent<PrefabUnitCard>();

                if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) continue;

                if (result > prefabUnitCard2.Attack)
                {
                    result = prefabUnitCard2.Attack;
                }
            }
            return result;
        }
        return 0;
    }

    //Metodo que halla el valor minimo distinto de cero en tres elementos, se llama desde LittleBurn() y Clear()
    private static int MinValueNoCero(int a, int b, int c)
    {
        if (a == 0) a = int.MaxValue;
        if (b == 0) b = int.MaxValue;
        if (c == 0) c = int.MaxValue;
        int min = Math.Min(a,Math.Min(b,c));
        return min;
    }

    //Metodo que se llama desde LittleBurn(), remueve las cartas con el ataque minimo del rival.
    private static void RemoveCardLittleBurn(int minAttack, List<GameObject> list, GameObject section, GameObject graveyard)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PrefabUnitCard prefabUnitCard = list[i].GetComponent<PrefabUnitCard>();

            if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) continue;

            if (minAttack == prefabUnitCard.Attack)
            {
                MetodosUtilesUnity.AddToListOneCard(list[i], section, graveyard);
                i--;
            }       
        }
    }

    //Aplica el metodo Steal(), roba una carta del deck y la añade a la mano del jugador.
    public static void StealEffect(GameObject card)
    {
        GameObject hand;
        GameObject deck;

        if (card.transform.IsChildOf(player1.transform))
        {
            hand = hand1;
            deck = deck2;
        }
        else
        {
            hand = hand2;
            deck = deck2;
        }

        StealEffect(deck, hand);
    }

    //Sobrecarga del metodo StealEffect(), se encarga de mover la carta de una lista a la otra en si (tambien cambia de padre a la carta)
    private static void StealEffect(GameObject origin, GameObject destiny)
    {
        List<GameObject> originList = origin.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> destinyList = destiny.GetComponent<ListaBaseConGObject>().cards;

        if (originList.Count > 0)
        {
            destinyList.Add(originList[0]);
            originList[0].transform.SetParent(destiny.transform);
            originList.Remove(originList[0]);
        }
    }

    //Metodo que halla todo el promedio de ataque de las cartas y convierte el ataque de todas las cartas del campo a ese promedio (las cartas se quedaran con ese ataque mientras dure la partida)
    public static void Average(GameObject card)
    {
        List<GameObject> M1 = P1MeleeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> R1 = P1RangeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> S1 = P1SiegeSection.GetComponent<ListaBaseConGObject>().cards;

        List<GameObject> M2 = P2MeleeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> R2 = P2RangeSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> S2 = P2SiegeSection.GetComponent<ListaBaseConGObject>().cards;

        int M1_TotalAttack = TotalAttackList(M1);
        int R1_TotalAttack = TotalAttackList(R1);
        int S1_TotalAttack = TotalAttackList(S1);

        int M2_TotalAttack = TotalAttackList(M2);
        int R2_TotalAttack = TotalAttackList(R2);
        int S2_TotalAttack = TotalAttackList(S2);

        int TotalAtack = M1_TotalAttack + M2_TotalAttack + R1_TotalAttack + R2_TotalAttack + S1_TotalAttack + S2_TotalAttack;
        int TotalCardCount = M1.Count + M2.Count + R1.Count + R2.Count + S1.Count + S2.Count;

        if (TotalCardCount == 0)
        {
            TotalCardCount = 1;
        }
        int Total = TotalAtack/TotalCardCount;
        
        SetAverageAttackList(Total, M1);
        SetAverageAttackList(Total, R1);
        SetAverageAttackList(Total, S1);

        SetAverageAttackList(Total, M2);
        SetAverageAttackList(Total, R2);
        SetAverageAttackList(Total, S2);
    }

    //Metodo que halla el total de ataque de una lista de cartas (se llama desde el metodo Average)
    private static int TotalAttackList(List<GameObject> list)
    {
        int result = 0;
        PrefabUnitCard prefabUnitCard;

        foreach (GameObject card in list)
        {
            prefabUnitCard = card.GetComponent<PrefabUnitCard>();

            if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) continue;

            result += prefabUnitCard.Attack;
        }
        return result;
    }

    //Metodo que coloca el mismo ataque a todas las cartas de una lista, se llama desde el metodo Average()
    private static void SetAverageAttackList(int average, List<GameObject> list)
    {
        PrefabUnitCard prefabUnitCard;
        foreach (GameObject element in list)
        {
            prefabUnitCard = element.GetComponent<PrefabUnitCard>();

            if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) continue;
            
            prefabUnitCard.Attack = average;
        }
    }

    //Metodo que limpia la fila del campo con menos cartas
    public static void Clear(GameObject card)
    {
        int m1Count = P1MeleeSection.GetComponent<ListaBaseConGObject>().cards.Count;
        int r1Count = P1RangeSection.GetComponent<ListaBaseConGObject>().cards.Count;
        int s1Count = P1SiegeSection.GetComponent<ListaBaseConGObject>().cards.Count;

        int m2Count = P2MeleeSection.GetComponent<ListaBaseConGObject>().cards.Count;
        int r2Count = P2RangeSection.GetComponent<ListaBaseConGObject>().cards.Count;
        int s2Count = P2SiegeSection.GetComponent<ListaBaseConGObject>().cards.Count;

        int p1Count = MinValueNoCero(m1Count, r1Count, s1Count);
        int p2Count = MinValueNoCero(m2Count, r2Count, s2Count);

        int Count = 0;
        
        if (p1Count == int.MaxValue && p2Count != int.MaxValue) Count = p2Count; 
        else if (p1Count != int.MaxValue && p2Count == int.MaxValue) Count = p1Count; 
        else if (p1Count != int.MaxValue && p2Count != int.MaxValue) Count = Math.Min(p1Count,p2Count);

        RemoveClear(Count);
    }

    //Despues de obtener la fila con menor cantidad de cartas, este metodo se encarga de limpiar aquellas listas con menor cantidad de cartas
    private static void RemoveClear(int value)
    {
        GameObject[] gameObjects = new GameObject[6]{P1MeleeSection,P1RangeSection,P1SiegeSection,P2MeleeSection,P2RangeSection,P2SiegeSection};
        List<GameObject> gameObjectsList;
        GameObject graveyard;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjectsList = gameObjects[i].GetComponent<ListaBaseConGObject>().cards;

            if (i < gameObjects.Length/2) graveyard = Graveyard1;
            else graveyard = Graveyard2;

            if (gameObjectsList.Count == value)
            {
                MetodosUtilesUnity.AddToListCards(gameObjects[i], graveyard);
            }
        }
    }

    //Metodo que multiplica el ataque de la carta por la cantidad de veces que esté repetida en el momento de colocarla
    public static void Companion(GameObject card)
    {
        GameObject[] sections = DetectCardSection(card);
        List<GameObject> sectionList1 = sections[0].GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> sectionList2 = sections[1].GetComponent<ListaBaseConGObject>().cards;

        int count = 0;

        foreach (GameObject element in sectionList1)
        {
            if (element.name == card.name) count++;
        }
        foreach (GameObject element in sectionList2)
        {
            if (element.name == card.name) count++;
        }

        PrefabUnitCard prefabUnitCard = card.GetComponent<PrefabUnitCard>();
        if (count > 0) 
        {
            count++;
            prefabUnitCard.Attack *= count;
            Debug.Log("funciona el if");
        }
    }


    //Metodo que se llama desde Companion(), devuelve un array de GameObject que contiene las dos posibles secciones donde puede estar la carta (cada sector de jugador)
    private static GameObject[] DetectCardSection(GameObject card)
    {
        GameObject preSection1;
        GameObject section1;

        GameObject preSection2;
        GameObject section2;

        GameObject[] result = new GameObject[2];

        if (card.GetComponent<PrefabUnitCard>().Board_Section == "Melee")
        {
            preSection1 = player1.transform.Find("Melee").gameObject;
            section1 = preSection1.transform.Find("MeleeSection").gameObject;

            preSection2 = player2.transform.Find("Melee").gameObject;
            section2 = preSection2.transform.Find("MeleeSection").gameObject;
        }
        else if (card.GetComponent<PrefabUnitCard>().Board_Section == "Range")
        {
            preSection1 = player1.transform.Find("Range").gameObject;
            section1 = preSection1.transform.Find("RangeSection").gameObject;

            preSection2 = player2.transform.Find("Range").gameObject;
            section2 = preSection2.transform.Find("RangeSection").gameObject;
        }
        else
        {
            preSection1 = player1.transform.Find("Siege").gameObject;
            section1 = preSection1.transform.Find("SiegeSection").gameObject;

            preSection2 = player2.transform.Find("Siege").gameObject;
            section2 = preSection2.transform.Find("SiegeSection").gameObject;
        }

        result[0] = section1;
        result[1] = section2;

        return result;
    }

    //Metodo que se llama desde el metodo Update(), si en una seccion esta un cuerno de guerra, mientras que esté, esa seccion se le duplica los ataques (los heroes no)
    public static void HornEffect(GameObject section)
    {
        List<GameObject> sectionList = section.GetComponent<ListaBaseConGObject>().cards;
        PrefabUnitCard prefabUnitCard;

        foreach (GameObject card in sectionList)
        {
            prefabUnitCard = card.GetComponent<PrefabUnitCard>();
            if (prefabUnitCard.UnitType == Unit_Card.EType.Gold.ToString()) continue;

            prefabUnitCard.Attack = prefabUnitCard.Backup_Atack * 2;
        }
    }

    //Metodo que se llama desde el metodo Update(), aplica el efecto de clima correspondiente a las secciones que estan de parametros
    public static void WeatherEffect(GameObject section1, GameObject section2)
    {
        List<GameObject> section1List = section1.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> section2List = section2.GetComponent<ListaBaseConGObject>().cards;

        foreach (GameObject card in section1List)
        {
            if (card.GetComponent<PrefabUnitCard>().UnitType == Unit_Card.EType.Gold.ToString()) continue;

            card.GetComponent<PrefabUnitCard>().Attack = 1;
        }
        foreach (GameObject card in section2List)
        {
            if (card.GetComponent<PrefabUnitCard>().UnitType == Unit_Card.EType.Gold.ToString()) continue;

            card.GetComponent<PrefabUnitCard>().Attack = 1;
        }
    }

    //Metodo que limpia cualquier clima que se encuentre activo en el juego
    public static void SunnyEffect(GameObject card)
    {
        Debug.Log("entro al soleado");
        List<GameObject> weatherList = WeatherSection.GetComponent<ListaBaseConGObject>().cards;
        List<GameObject> graveyardList = Graveyard2.GetComponent<ListaBaseConGObject>().cards;    

        if (weatherList.Count == 0) return;

        for (int i = 0; i < weatherList.Count; i++)
        {
            graveyardList.Add(weatherList[i]);
            weatherList[i].transform.SetParent(Graveyard2.transform);
        }
        weatherList.Clear();

        GameManager.frostEffect = false;
        GameManager.fogEffect = false;
        GameManager.rainEffect = false;

        ResetWeatherAttack(P1MeleeSection);
        ResetWeatherAttack(P1RangeSection);
        ResetWeatherAttack(P1SiegeSection);

        ResetWeatherAttack(P2MeleeSection);
        ResetWeatherAttack(P2RangeSection);
        ResetWeatherAttack(P2SiegeSection);
    }

    //Metodo que se llama desde el SunnyEffect(). Resetea los ataques de las cartas de la seccion corresponidente
    private static void ResetWeatherAttack(GameObject section)
    {
        List<GameObject> list = section.GetComponent<ListaBaseConGObject>().cards;

        foreach (GameObject card in list)
        {
            if (card.GetComponent<PrefabUnitCard>().UnitType == Unit_Card.EType.Gold.ToString()) continue;

            card.GetComponent<PrefabUnitCard>().Attack = card.GetComponent<PrefabUnitCard>().Backup_Atack;
        }
    }

    public static void DecoyEffect(GameObject card, GameObject hand)
    {
       Transform cardTransform = card.transform;
       Transform padreTransform = cardTransform.parent;

       GameObject section = padreTransform.gameObject; 
       GameObject decoy = null;

       List<GameObject> handList = hand.GetComponent<ListaBaseConGObject>().cards;
       foreach (GameObject element in handList)
       {
            if (element.GetComponent<PrefabUnitCard>() != null)
            {
                if (element.GetComponent<PrefabUnitCard>().Effect == Unit_Card.E_Effect.Señuelo.ToString() && element.name == Unit_Card.E_Effect.Señuelo.ToString())
                {
                    decoy = element;
                }
            }
       }

       MetodosUtilesUnity.AddToListOneCard(decoy, hand, section);
       MetodosUtilesUnity.AddToListOneCard(card, section, hand);
    }
}
