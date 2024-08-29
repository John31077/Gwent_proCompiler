 /* Binary expressions has Left and Right expressions.
 5 + 5 is a binary expresion. In this case, Left and Right expressions are Number atoms */
public abstract class BinaryExpression : Expression
    {
        public Expression? Right { get; set; }
        public Expression? Left { get; set; }
        
        public BinaryExpression(CodeLocation location) : base(location){}
    }