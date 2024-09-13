using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class EffectOnActivation : ASTNode
{
    public string Id;
    public List<ParametroValor> ParamsList;


    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkParamsExpression = true;

        if (!context.effects.Contains(Id))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Effect (" + Id + ") are not previously defined"));
            return false;
        }

        if (ParamsList.Count == 0)
        {
            return true;
        }

        if (Dictionarys.effects.Count == 0)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Effect not defined previously"));
            return false;
        }

        Effect effect = Dictionarys.effects[Id];
        if (effect.ParamsExpresions.Count != ParamsList.Count)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Params diferent of the original effect"));
            return false;
        }


        for (int i = 0; i < ParamsList.Count; i++)
        {
            TypeOfValue paramsType = effect.ParamsExpresions[i].typeOfValue;
            checkParamsExpression = ParamsList[i].Expression.CheckSemantic(context, scope, errors);
            
            if (paramsType == TypeOfValue.Bool)
            {
                if (ParamsList[i].Expression.Type != ExpressionType.Bool)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Params diferent of the original effect"));
                }
                continue;
            }
            else if (paramsType == TypeOfValue.Number)
            {
                if (ParamsList[i].Expression.Type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Params diferent of the original effect"));
                }
                continue;
            }
            else if (paramsType == TypeOfValue.String)
            {
                if (ParamsList[i].Expression.Type != ExpressionType.String)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Params diferent of the original effect"));
                }
                continue;
            }
        }
        
        return checkParamsExpression;
    }

    public EffectOnActivation(CodeLocation location) : base (location)
    {
        ParamsList = new List<ParametroValor>();
    }
}