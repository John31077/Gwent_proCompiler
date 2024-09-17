using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

class EffectCreation : MonoBehaviour
{
    //Diccionario con los identificadores con sus respectivas expresiones (se añaden los elementos en el Assign)
    public static Dictionary<string, Expression> identifiers = new Dictionary<string, Expression>();
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool card;
    public static int cardIndex = 0;
    public static List<GameObject> predicateList;
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
    public static GameObject siegeSection1 = preSectionS1.transform.Find("SiegeSection").gameObject;
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
    public static GameObject siegeSection2 = preSectionS2.transform.Find("SiegeSection").gameObject;
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
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

    public static void AddCompilerEffect(Dictionary<string, Effect> effects)
    {   
        foreach (var effect in effects)
        {
            DictionaryEffects.EffectsDictionary.Add(effect.Key, ApplyEffect);
        }
    }




    public static void ApplyEffect(GameObject card)
    {
       ApplyEffect(card, Programa.elementalProgram);
    }

    private static void ApplyEffect(GameObject card, ElementalProgram elementalProgram)
    {
        string cardName = card.name;

        Dictionary<string, CardG> cards = elementalProgram.Cards;
        
        CardG cardG = cards[cardName];
        List<ASTNode> onActivation = cardG.OnActivation;
        Debug.Log(onActivation.Count + " cantidad de cosas en onActivation de la carta");

        EffectOnActivation onEffect = null;
        SelectorOnActivation onSelector = null;
        PostAction postAction = null;


        if (onActivation.Count > 0)
        {
            for (int i = 0; i < onActivation.Count; i++)
            {
                if (onActivation[i] is EffectOnActivation) //Encuentra un efecto
                {
                    onEffect = (EffectOnActivation)onActivation[i];
                    if (i + 1 < onActivation.Count) //Entra si no se ha acabado la lista
                    {
                        i += 1;
                        if (onActivation[i] is SelectorOnActivation) //Entra si se encontró un Selector
                        {
                            onSelector = (SelectorOnActivation)onActivation[i];
                            if (i + 1 < onActivation.Count)
                            {
                                i += 1;
                                if (onActivation[i] is PostAction) //entra si despues de encontrar un selector encontro un posAction
                                {
                                    postAction = (PostAction)onActivation[i];
                                    //Effectua con un effect, selector y postAction
                                    ActivateEffect(card,elementalProgram,onEffect,onSelector,postAction);
                                    onEffect = null;
                                    onSelector = null;
                                    postAction = null;
                                }
                                else
                                {
                                    //
                                    i = i-1; //Entonces despues de un selector hay un effect
                                    ActivateEffect(card,elementalProgram,onEffect,onSelector,postAction);
                                    onEffect = null;
                                    onSelector = null;
                                    postAction = null;
                                } 
                            }
                            else 
                            {
                                // Efectua con effect y selector
                                ActivateEffect(card,elementalProgram,onEffect,onSelector,postAction);

                                onEffect = null;
                                onSelector = null;
                                postAction = null;
                                break;
                            } 
                        }
                        else if (onActivation[i] is PostAction) //Entra si hay directamente un postAction en vez de un Selector
                        {
                            postAction = (PostAction)onActivation[i];
                            // Efectua con un effect y un postAction
                            ActivateEffect(card,elementalProgram,onEffect,onSelector,postAction);
                            onEffect = null;
                            onSelector = null;
                            postAction = null;
                        }
                        else
                        {
                            i = i-1; //Entonces despues de un effect hay otro effect, se vira un elemento atras
                            ActivateEffect(card,elementalProgram,onEffect,onSelector,postAction);
                            onEffect = null;
                            onSelector = null;
                            postAction = null;
                        } 
                    }
                    else //Entra si despues de un effecto la lista se acabó
                    {
                        // Efectua solamente con un effect
                        ActivateEffect(card,elementalProgram,onEffect,onSelector,postAction);
                        onEffect = null;
                        onSelector = null;
                        postAction = null;
                        break;
                    }
                }
                
            }

        }
    } 


    public static void ActivateEffect(GameObject card, ElementalProgram elementalProgram, EffectOnActivation onEffect, SelectorOnActivation onSelector, PostAction postAction)
    {
        Dictionary<string,Effect> effects = elementalProgram.Effects; //Se llama al diccionario de los effectos
        
        string effectName = onEffect.Id; //Se guarda el nombre del effecto que tenia la carta

        Effect effect = effects[effectName]; //Se llama al effecto especifico que la carta llamó
        Debug.Log(effect.Id +  " fggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");



        List<ParametroValor> paramsList = onEffect.ParamsList;
        foreach (ParametroValor parametroValor in paramsList)
        {
            parametroValor.Expression.Evaluate();
            identifiers[parametroValor.Id] = parametroValor.Expression;
        }





        if (onSelector != null) //Entra si el selector no es nulo
        {
            List<GameObject> result = new List<GameObject>(); //Lista que se le enviará al effecto original para targets
            string source = onSelector.Source;

            List<GameObject> sourceList = SourceList(source); //Se guarda la lista que llamó el source de selector
            List<GameObject> predicateList = PredicateList(sourceList, onSelector.Predicate); //Se filtra la lista anterior mediante el predicado

            onSelector.Single.Evaluate();
            bool single = (bool)onSelector.Single.Value;

            if (predicateList.Count != 0)
            {
                if (single) result.Add(predicateList[0]);
                else result = predicateList;
            }

            effect.listSelector = result;
            //Hasta aqui, el efecto tiene un selector y se llenó la lista de listSelector (targets) del efecto
        }

        List<ASTNode> effectInstructions = effect.ActionList;

        foreach (ASTNode instruction in effectInstructions)
        {
            if (instruction is Expression)
            {
                Expression expression = (Expression)instruction;
                expression.Evaluate();
            }
            else if (instruction is While)
            {
                NewWhile((While)instruction);
            }
            else if (instruction is For)
            {
                //Metodo for
            }
        }

        if (postAction != null)
        {
            ApplyEffect(card, elementalProgram);
        }
    }

    public static List<GameObject> SourceList(string source)
    {
        string player = VerificatePlayer();

        if (source == "board")
        {
            BoardList();
            return board;
        }
        else if (source == "hand")
        {
            if (player == player1.name) return h1;
            else return h2;
        }
        else if (source == "otherHand")
        {
            if (player == player1.name) return h2;
            else return h1;
        }
        else if (source == "deck")
        {
            if (player == player1.name) return deck1;
            else return deck2;
        }
        else if (source == "otherDeck")
        {
            if (player == player1.name) return deck2;
            else return deck1;
        }
        else if (source == "field")
        {
            if (player == player1.name) return FieldOfPlayerList(player1.name);
            else return FieldOfPlayerList(player2.name);
        }
        else if (source == "otherField")
        {
            if (player == player1.name) return FieldOfPlayerList(player2.name);
            else return FieldOfPlayerList(player1.name);
        }

        return null;
    }

    public static void BoardList()
    {
        board.Clear();
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

    public static GameObject VerificateIndexer(int indexer, List<GameObject> list)
    {
        if (indexer >= list.Count)
        {
            if (list.Count == 0)
            {
                Debug.Log("No hay elementos en la lista para indexar");
                return null;
            }
            indexer = list.Count-1;
        }
        else if (indexer < 0)
        {
            if (list.Count == 0)
            {
                Debug.Log("No hay elementos en la lista para indexar");
                return null;
            }
            indexer = 0;
        }
        return list[indexer];
    }

    public static GameObject VerificateIdentifierLeftIndexer(string identifier, int indexer)
    {
        List<GameObject> list = new List<GameObject>();
        GameObject card;

        if (identifier == "Board")
        {
            BoardList();
            list = board;
        }
        else if (identifier == "Hand")
        {
            string triggerPlayer = VerificatePlayer();

            if (triggerPlayer == player1.name) list = h1;
            else list = h2;
        }
        else if (identifier == "Deck")
        {
            string triggerPlayer = VerificatePlayer();

            if (triggerPlayer == player1.name) list = deck1;
            else list = deck2;
        }
        else if (identifier == "Graveyard")
        {
            string triggerPlayer = VerificatePlayer();

            if (triggerPlayer == player1.name) list = g1;
            else list = g2;
        }
        else if (identifier == "Field")
        {
            string triggerPlayer = VerificatePlayer();

            list = FieldOfPlayerList(triggerPlayer);
        }

        card = VerificateIndexer(indexer, list);
        return card;
    }

    public static void Push(List<GameObject> list, GameObject card)
    {
        list.Add(card);
    }

    public static void Remove(List<GameObject> list, GameObject card)
    {
        if (list.Contains(card)) list.Remove(card);
        else Debug.Log("La lista no contiene la carta a remover");
    }

    public static void SendBottom(List<GameObject> list, GameObject card)
    {
        list.Add(card);
        
        if (list.Count > 1)
        {
            GameObject card1 = list[list.Count-1];
            for (int i = list.Count-1 ; i >= 1; i--)
            {
                list[i] = list[i-1];
            }
            list[0] = card1;
        }
    }

    public static GameObject Pop(List<GameObject> list)
    {
        GameObject card;

        if (list.Count != 0)
        {
            card = list[list.Count-1];
            list.RemoveAt(list.Count-1);
            return card;
        }
        else
        {
            Debug.Log("El metodo Pop no encontró carta para devolver, se devuelve null");
        }
        return null;
    }

    public static string CardPropertyString(GameObject card, string property)
    {
        if (card.GetComponent<PrefabUnitCard>())
        {
            PrefabUnitCard prefabUnitCard = card.GetComponent<PrefabUnitCard>();
            if (property == "Type") return prefabUnitCard.UnitType;
            else if (property == "Name") return card.name;
            else if (property == "Faction") return prefabUnitCard.Faction;
            else if (property == "Power") return prefabUnitCard.Attack.ToString();
            else if (property == "Range") return prefabUnitCard.Board_Section;
            else if (property == "Owner") return prefabUnitCard.Owner;
        }
        else if (card.GetComponent<PrefabSpecialCard>())
        {
            PrefabSpecialCard prefabSpecialCard = card.GetComponent<PrefabSpecialCard>();
            if (property == "Type") return "Aumento";
            else if (property == "Name") return card.name;
            else if (property == "Faction") return "Neutral";
            else if (property == "Power") return null;
            else if (property == "Range") return null;
            else if (property == "Owner") return prefabSpecialCard.Owner;
        }
        else if (card.GetComponent<PrefabWeatherCard>())
        {
            PrefabWeatherCard prefabWeatherCard = card.GetComponent<PrefabWeatherCard>();
            if (property == "Type") return "Clima";
            else if (property == "Name") return card.name;
            else if (property == "Faction") return "Neutral";
            else if (property == "Power") return null;
            else if (property == "Range") return null;
            else if (property == "Owner") return prefabWeatherCard.Owner;
        }
        return "error";
    }

    public static List<GameObject> PredicateList(List<GameObject> list, Predicate predicate)
    {
        if (list.Count != 0)
        {
            bool condition = false;
            List<GameObject> resultList = new List<GameObject>();

            for (int i = 0; i < list.Count; i++)
            {
                cardIndex = i;
                predicate.Right.Evaluate();
                if (predicate.Right.Value == null) condition = false;
                else condition = (bool)predicate.Right.Value;
                
                if (condition)
                {
                    resultList.Add(list[i]);
                } 
            }
            return resultList;
        }
        return list;
    }


    public static void NewWhile(While While)
    {
        Expression conditionExp = While.Condition;
        List<ASTNode> instructions = While.ActionList;

        conditionExp.Evaluate();
        bool condition = (bool)conditionExp.Value;

        while (condition)
        {
            foreach (ASTNode instruction in instructions)
            {
                if (instruction is Expression)
                {
                    Expression expression = (Expression)instruction;
                    expression.Evaluate();
                }
                else if (instruction is While)
                {
                    NewWhile((While)instruction);
                }
                else if (instruction is For)
                {
                    //Metodo for
                }

                conditionExp.Evaluate();
                condition = (bool)conditionExp.Value;
            }
        }
    }
}