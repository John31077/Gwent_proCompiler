using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Assign : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public Assign(CodeLocation location) : base(location){}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);

        if (Left is Identifier)
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Left.Value.ToString(), scope);
            if (tuple.Item1)
            {
                tuple.Item2.varYValores[Left.Value.ToString()] = Right;
            }
            else
            {
                scope.varYValores[Left.Value.ToString()] = Right;
            }

            if (EffectCreation.identifiers.ContainsKey(Left.Value.ToString()))
            {
                EffectCreation.identifiers[Left.Value.ToString()] = Right;
            }
            else EffectCreation.identifiers.Add(Left.Value.ToString(), Right);




            EffectCreation.identifiers[Left.Value.ToString()].Evaluate();
        

            if (EffectCreation.identifiers[Left.Value.ToString()].Value is GameObject) 
            EffectCreation.identifiers[Left.Value.ToString()].Type = ExpressionType.Card;
            else if (EffectCreation.identifiers[Left.Value.ToString()].Value is List<GameObject>)
            EffectCreation.identifiers[Left.Value.ToString()].Type = ExpressionType.List;
            else
            Type = ExpressionType.Anytype;


            return right && left;
        }


        if (Right.Type == ExpressionType.Number)
        {
            double a;
            if (Left.Type != ExpressionType.Number || double.TryParse(Left.Value.ToString(), out a))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid type for assign"));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        else if (Right.Type == ExpressionType.String)
        {
            string a = Left.Value.ToString();
            if (Left.Type != ExpressionType.String || a[0] == '"')
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid type for assign"));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        else if (Right.Type == ExpressionType.Bool)
        {
            bool a;
            if (Left.Type != ExpressionType.Bool || bool.TryParse(Left.Value.ToString(), out a))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid type for assign"));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }


        
        Type = ExpressionType.Anytype;
        return right && left;
    }

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        

        if (Left is Identifier)
        {
            
            if (EffectCreation.identifiers.ContainsKey(Left.Value.ToString()))
            {
                EffectCreation.identifiers[Left.Value.ToString()] = Right;
            }

        }
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("{0} = {1}", Left, Right);
        }
        return Value.ToString();
    }
}

