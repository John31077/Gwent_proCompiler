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
        if (this.Value.ToString() == "target" || this.Value.ToString() == "unit" || this.Value.ToString() == "card")
        {
            Type = ExpressionType.Card;
        }
        else if (this.Value.ToString() == "context")
        {
            Type = ExpressionType.Context;
        }
        else if (this.Value.ToString() == "false" || this.Value.ToString() == "true")
        {
            Type = ExpressionType.Bool;
        }

        return true;
    }

    public override void Evaluate()
    {
        if (EffectCreation.identifiers.ContainsKey(this.Value.ToString()))
        {
            UnityEngine.Debug.Log(EffectCreation.identifiers[this.Value.ToString()].Value + " fffffffffffffffffffffffffff");
            if (this.Value == null)
            {
                return;
            }
            Value = EffectCreation.identifiers[this.Value.ToString()].Value;
        }
    }

    public override string ToString()
    {
        return String.Format("{0}",Value);
    }
}