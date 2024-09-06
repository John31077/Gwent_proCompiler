using System.Collections.Generic;

public class EffectOnActivation : ASTNode
{
    public string Id;
    public List<ParametroValor> ParamsList;




    public EffectOnActivation(CodeLocation location) : base (location)
    {
        ParamsList = new List<ParametroValor>();
    }
}