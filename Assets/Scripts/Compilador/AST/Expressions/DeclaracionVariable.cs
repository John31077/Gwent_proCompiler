public class DeclaracionVariable : ASTNode
{
    public string id { get; set; }
    public ASTNode value { get; set; }

    public DeclaracionVariable(string id, ASTNode value, CodeLocation location) : base (location)
    {
        this.id = id;
        this.value = value;
    }
}