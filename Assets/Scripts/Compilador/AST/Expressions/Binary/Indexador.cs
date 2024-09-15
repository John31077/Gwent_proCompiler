using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class Indexador : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public Indexador(CodeLocation location) : base(location)
    {
        Type = ExpressionType.Anytype;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);

        if (Left is Identifier)
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Left.Value.ToString(), scope);
            if (tuple.Item1)
            {
                Expression expression = tuple.Item2.varYValores[Left.Value.ToString()];
                Left.Type = expression.Type;
            }
        }

        if (Right is Identifier)
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Right.Value.ToString(), scope);
            if (tuple.Item1)
            {
                Expression expression = tuple.Item2.varYValores[Right.Value.ToString()];
                Left.Type = expression.Type;
            }
        }

        
        if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.List)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid indexer"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Card;
        return right && left;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("<{0}[{1}]>", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        List<GameObject> list = new List<GameObject>();

        double indexer1 = (double)Right.Value;
        int indexer = (int)indexer1;

        if (Left is Identifier && !EffectCreation.identifiers.ContainsKey(Left.Value.ToString()))
        {
            if (Left.ToString() == "Hand")
            {
                string triggerPlayer = EffectCreation.VerificatePlayer();

                if (triggerPlayer == EffectCreation.player1.name) list = EffectCreation.h1;
                else list = EffectCreation.h2;
            }
            else if (Left.ToString() == "Deck")
            {
                string triggerPlayer = EffectCreation.VerificatePlayer();

                if (triggerPlayer == EffectCreation.player1.name) list = EffectCreation.deck1;
                else list = EffectCreation.deck2;
            }
            else if (Left.ToString() == "Field")
            {
                string triggerPlayer = EffectCreation.VerificatePlayer();

                list = EffectCreation.FieldOfPlayerList(triggerPlayer);
            }
            else if (Left.ToString() == "Graveyard")
            {
                string triggerPlayer = EffectCreation.VerificatePlayer();

                if (triggerPlayer == EffectCreation.player1.name) list = EffectCreation.g1;
                else list = EffectCreation.g2;
            }
        }
        else if (Left is Identifier && EffectCreation.identifiers.ContainsKey(Left.Value.ToString()))
        {
            EffectCreation.identifiers[Left.Value.ToString()].Evaluate();
            list = (List<GameObject>)EffectCreation.identifiers[Left.Value.ToString()].Value;
        }
        else list = (List<GameObject>)Left.Value;

        if (indexer >= list.Count) 
        {
            if (list.Count > 0) indexer = list.Count-1;
            else UnityEngine.Debug.Log("Indexador fuera de rango de la lista, el indexador debe ser menor que la cantidad de elementos de la lista");
        }
        else if (indexer < 0) UnityEngine.Debug.Log("Indexador fuera del rango de la lista, indexador debe ser mayor que 0");
        else 
        {
            GameObject card = list[indexer];
            Value = card;
        }
    }
}