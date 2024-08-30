using System;

public class Indexador : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public Indexador(CodeLocation location) : base(location)
    {
        
    }

    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("{0}[{1}]", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {

    }
}