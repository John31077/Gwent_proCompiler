using System;
using System.Collections.Generic;

public class Igual : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}
    public override void Evaluate()
    {

    }
    public Igual(CodeLocation location) : base(location){}

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
        
        if (Right.Type != Left.Type)
        {
            UnityEngine.Debug.Log(Left.Type + " dddddddddddddddddddddddddddddddd  " + Right.Type);
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "diferents expresion types in both sides of =="));
            Type = ExpressionType.ErrorType;
            return false;
        }
        
        Type = ExpressionType.Bool;
        return right && left;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} == {1})", Left, Right);
        }
        return Value.ToString();
    }
}