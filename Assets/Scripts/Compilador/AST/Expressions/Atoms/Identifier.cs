using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Identifier : AtomExpression
{
    private ExpressionType type = ExpressionType.Identifier;
    public override ExpressionType Type
    {
        get
        {
            return type;
        }
        set {type = value; }
    }

    public override object? Value { get; set; }
    
    public Identifier(string value, CodeLocation location) : base(location)
    {
        Value = value;
    }
    
    public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
    {
        if (this.Value.ToString() == "target")
        {
            Type = ExpressionType.Card;
        }
        else if (this.Value.ToString() == "context")
        {
            Type = ExpressionType.Context;
        }
        return true;
    }

    public override void Evaluate()
    {
        
    }

    public override string ToString()
    {
        return String.Format("{0}",Value);
    }
}