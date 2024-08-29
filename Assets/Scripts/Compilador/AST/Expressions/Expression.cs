 /* All the expressions inherit from this abstract class.
 Every expression can be evaluated and has a type and a value.
 After check the expression's semantic, Type will store the corresponding ExpressionType.
 After Evaluate the expression, Value will store the calculated value. */
public abstract class Expression : ASTNode
{
    public abstract void Evaluate();

    public abstract ExpressionType Type { get; set; }

    public abstract object? Value { get; set; }

    public Expression(CodeLocation location) : base (location) { }
}