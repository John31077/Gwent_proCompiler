using System;
using System.Collections.Generic;

public class And : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public And(CodeLocation location) : base(location)
    {

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
        
        if (Right.Type!=ExpressionType.Bool || Left.Type!=ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "&& must be bool or identifier in both sides"));
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
            return String.Format("({0} && {1})", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        bool right = (bool)Right.Value;
        bool left = (bool)Left.Value;
        
        Value = right && left;
    }
}

public class Or : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public Or(CodeLocation location) : base(location)
    {

    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if ((Right.Type!=ExpressionType.Bool&&Right.Type!=ExpressionType.Identifier)||(Left.Type!=ExpressionType.Bool&&Left.Type!=ExpressionType.Identifier))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "|| must be bool or identifier in both sides"));
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
            return String.Format("({0} || {1})", Left, Right);
        }
        return Value.ToString();
    }    


    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        bool right = (bool)Right.Value;
        bool left = (bool)Left.Value;
        
        Value = right || left;
    }
}