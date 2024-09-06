using System.Collections.Generic;

public class SelectorOnActivation : ASTNode
{
    public string Source;
    public Expression Single;
    public Predicate Predicate;
    




    public SelectorOnActivation(CodeLocation location) : base (location)
    {
        
    }
}