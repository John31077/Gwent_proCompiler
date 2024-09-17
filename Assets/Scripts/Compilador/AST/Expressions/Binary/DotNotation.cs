using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class DotNotation : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public DotNotation(CodeLocation location) : base(location)
    {
        Type = ExpressionType.Anytype;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);

        
        
        if (Left is Identifier && !ListOfIdentifiers.IdentifiersList.Contains(Left.Value.ToString()))
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Left.Value.ToString(), scope);
            if (tuple.Item1)
            {
                Expression expression = tuple.Item2.varYValores[Left.Value.ToString()];
                Left.Type = expression.Type;
            }
        }


        if ((Right is Keyword) && (Right.Value.ToString()==IdentifierType.Power.ToString()||Right.Value.ToString()==IdentifierType.Name.ToString()||
            Right.Value.ToString()==IdentifierType.Type.ToString()||Right.Value.ToString()==IdentifierType.Faction.ToString()||
            Right.Value.ToString()==IdentifierType.Range.ToString()))
        {
            if (Left.Type != ExpressionType.Card)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            if ((Right is Keyword) && (Right.Value.ToString() == IdentifierType.Power.ToString()))
            {
                Type = ExpressionType.Number;
            }
            else 
            {
                Type = ExpressionType.String;
            }

            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString()==IdentifierType.TriggerPlayer.ToString()))
        {
            if (Left.Type != ExpressionType.Context)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Player;
            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Board.ToString()))
        {
            if (Left.Type != ExpressionType.Context)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.List;
            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString()==IdentifierType.Hand.ToString()||Right.Value.ToString()==IdentifierType.Deck.ToString()||
                 Right.Value.ToString()==IdentifierType.Field.ToString()||Right.Value.ToString()==IdentifierType.Graveyard.ToString()))
        {
            if (Left.Type != ExpressionType.Context)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.List;
            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Owner.ToString()))
        {
            if (Left.Type != ExpressionType.Card)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Player;
            return left && right;
        }
        else if (Right.Type == ExpressionType.Method)
        {
            if (Left.Type != ExpressionType.List && Left.Type != ExpressionType.ContextList)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a list"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Method;
            return right && left;
        }
        else if (Right.Type == ExpressionType.List)
        {
            if (Right is Bracket)
            {
                Bracket bracket = (Bracket)Right;
                if (bracket.Right is Predicate)
                {
                    if (Left.Type != ExpressionType.List)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a list"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                    Type = ExpressionType.List;
                    return right && left;
                }

                if (Left.Type != ExpressionType.Context)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }

                Type = ExpressionType.ContextList;
                return right && left;
            }
        }
        return false;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("<{0}.{1}>", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        bool Continue = false;

        if ((Left.Value is Identifier) && (Left.Value.ToString() == "context" || EffectCreation.identifiers.ContainsKey(Left.Value.ToString())))
        {
            if (EffectCreation.identifiers.ContainsKey(Left.Value.ToString()))
            {
                if (EffectCreation.identifiers[Left.Value.ToString()].Value.ToString() == "context")
                {
                    Left.Value = EffectCreation.identifiers[Left.Value.ToString()];
                    Left.Evaluate();
                    Continue = true;
                }
            }
            
            if (Continue) // Esto es para evitar que entre con un identificador incorrecto a ejecutarse como context
            {

            if (Right is Identifier)
            {
                if (Right.Value.ToString() == "TriggerPlayer")
                {
                    string triggerPlayer = EffectCreation.VerificatePlayer();
                    Value = triggerPlayer;
                    return;
                }
                else if (Right.Value.ToString() == "Board")
                {
                    EffectCreation.BoardList();
                    Value = EffectCreation.board;
                    return;
                }
                else if (Right.Value.ToString() == "Hand")
                {
                    List<GameObject> hand = new List<GameObject>();
                    string triggerPlayer = EffectCreation.VerificatePlayer();

                    if (triggerPlayer == EffectCreation.player1.name) hand = EffectCreation.h1;
                    else hand = EffectCreation.h2;

                    Value = hand;
                    return;
                }
                else if (Right.Value.ToString() == "Deck")
                {
                    List<GameObject> deck = new List<GameObject>();
                    string triggerPlayer = EffectCreation.VerificatePlayer();

                    if (triggerPlayer == EffectCreation.player1.name) deck = EffectCreation.deck1;
                    else deck = EffectCreation.deck2;

                    Value = deck;
                    return;
                }
                else if (Right.Value.ToString() == "Field")
                {
                    List<GameObject> field = new List<GameObject>();
                    string triggerPlayer = EffectCreation.VerificatePlayer();

                    field = EffectCreation.FieldOfPlayerList(triggerPlayer);

                    Value = field;
                    return;
                }
                else if (Right.Value.ToString() == "Graveyard")
                {
                    List<GameObject> graveyard = new List<GameObject>();
                    string triggerPlayer = EffectCreation.VerificatePlayer();

                    if (triggerPlayer == EffectCreation.player1.name) graveyard = EffectCreation.g1;
                    else graveyard = EffectCreation.g2;

                    Value = graveyard;
                    return;
                }
            }
            else if (Right is Bracket)
            {
                Right.Evaluate();
                Value = Right.Value;
                return;
            }
            else if (Right is Indexador) //El indexador despues del context. es complicado debido a su forma de parsear
            {
                Indexador index = (Indexador)Right;
                index.Right.Evaluate();
                int indexer = (int)index.Right.Value;
                
                if (index.Left is Bracket) //Parece que despues de un context. nunca viene un Find(Predicate) ni con el indexador
                {
                    Bracket bracket = (Bracket)index.Left;

                    bracket.Evaluate();
                    index.Left.Value = bracket.Value;
                    Value = EffectCreation.VerificateIndexer(indexer, (List<GameObject>)index.Left.Value);
                    return;
                }
                else if (index.Left is Identifier)
                {
                    Value = EffectCreation.VerificateIdentifierLeftIndexer(index.Left.ToString(), indexer);
                    return;
                }
            }
            }
        }
        else if (Left is DotNotation || (Left is Identifier && EffectCreation.identifiers.ContainsKey(Left.Value.ToString())))
        {
            if (Left is Identifier && EffectCreation.identifiers.ContainsKey(Left.Value.ToString()))
            {
                Left.Value = EffectCreation.identifiers[Left.Value.ToString()].Value;
                Left.Evaluate();
            }


            if (Left.Value is List<GameObject>)
            {
                List<GameObject> list = (List<GameObject>)Left.Value;

                if (Right is Bracket)
                {
                    Bracket bracket = (Bracket)Right;

                    if (bracket.Left.Value.ToString() == "Shuffle")
                    {
                        MetodosUtiles.Shuffle(list);
                        Value = list;
                        return;
                    }
                    else if (bracket.Left.Value.ToString() == "Push")
                    {
                        bracket.Right.Evaluate();
                        GameObject card = (GameObject)bracket.Right.Value;
                        EffectCreation.Push(list, card);
                        Value = list;
                        return;
                    }
                    else if (bracket.Left.Value.ToString() == "Remove")
                    {
                        bracket.Right.Evaluate();
                        GameObject card = (GameObject)bracket.Right.Value;
                        EffectCreation.Remove(list, card);
                        Value = list;
                        return;
                    }
                    else if (bracket.Left.Value.ToString() == "SendBottom")
                    {
                        bracket.Right.Evaluate();
                        GameObject card = (GameObject)bracket.Right.Value;
                        EffectCreation.SendBottom(list, card);
                        Value = list;
                        return;
                    }
                    else if (bracket.Left.Value.ToString() == "Pop")
                    {
                        GameObject card = EffectCreation.Pop(list);
                        Value = card;
                        return;
                    }
                    else if (bracket.Left.Value.ToString() == "Find")
                    {
                        EffectCreation.predicateList = list;
                        Value = EffectCreation.PredicateList(list, (Predicate)bracket.Right);
                        return;
                    }
                }
                else if (Right is Indexador) //Solamente puede venir un Find(predicate) antes de un indexer si hay una lista en la izq
                {
                    Indexador indexador = (Indexador)Right;
                    Bracket bracket = (Bracket)indexador.Left;
                    Predicate predicate = (Predicate)bracket.Right;

                    EffectCreation.predicateList = list;
                    List<GameObject> filterList = EffectCreation.PredicateList(list, predicate);
                    indexador.Right.Evaluate();
                    int indexer = (int)indexador.Right.Value;

                    if (filterList.Count == 0)
                    {
                        Debug.Log("No se puede indexar la lista");
                        Value = filterList;
                        return;
                    }

                    if (indexer < 0 || indexer >= filterList.Count)
                    {
                        Debug.Log("El indexer no está dentro del tamaño de la lista");
                        Value = filterList;
                        return;
                    }

                    Value = filterList[indexer];
                    return;
                }
            }
            else if (Left.Value is GameObject || (Left.Value is Identifier && Left.Value.ToString() == "card"))
            {
                GameObject card = null;

                //Todo esto del if y else es debido a el predicado, no se verá un "card" en un lugar que no sea el right de un predicate, no se rompe la asignacion del indexer debido a que el metodo del predicate verifica que la lista no este vacia
                if (Left.Value is Identifier && Left.Value.ToString() == "card") card = EffectCreation.predicateList[EffectCreation.cardIndex];
                else card = (GameObject)Left.Value;

                if (Right is Keyword || Right.Value.ToString() == "Owner")
                {
                    string property = EffectCreation.CardPropertyString(card, Right.Value.ToString());
                    Value = property;
                    return;
                }
            }
        }
    }
}