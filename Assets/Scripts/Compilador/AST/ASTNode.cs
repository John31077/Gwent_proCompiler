/* Our AST root, every node inherits from him. 
Every AST node knows his location and has a method to ckeck the semantic */
public abstract class ASTNode
{
    public CodeLocation Location {get; set;}
    //public abstract bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors);
    public ASTNode(CodeLocation location)
    {
        Location = location;
    }
}