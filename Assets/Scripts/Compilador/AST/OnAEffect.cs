using System.Collections.Generic;

public class EffectOnActivation : ASTNode
{
    public string Id;
    public List<ParametroValor> ParamsList;


    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }

    public EffectOnActivation(CodeLocation location) : base (location)
    {
        ParamsList = new List<ParametroValor>();
    }
}