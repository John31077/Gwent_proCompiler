using System.Collections.Generic;

public class Effect : ASTNode
{
    public string Id {get; set;}
    
    public List<Parametro> ParamsExpresions;

    public List<ASTNode> ActionList;

    public Effect(string id, CodeLocation location) : base (location)
    {
        this.Id = id;
        ParamsExpresions = new List<Parametro>();
        ActionList = new List<ASTNode>();
    }

}