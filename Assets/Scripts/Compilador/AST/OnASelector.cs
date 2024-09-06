using System.Collections.Generic;

public class SelectorOnActivation : ASTNode
{
    public string Source;
    public Expression Single;
    public Predicate Predicate;
    
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }



    public SelectorOnActivation(CodeLocation location) : base (location)
    {
        
    }
}