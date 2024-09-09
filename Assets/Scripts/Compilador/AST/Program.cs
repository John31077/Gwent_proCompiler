/* This node represents a program. The program has an errors list and some cards
and elements represented by dictionaries. Every Card and Element is acceced by his id */
using System.Collections.Generic;

public class ElementalProgram : ASTNode
{
    public List<CompilingError> Errors {get; set;}
    public Dictionary<string, Effect> Effects {get; set;}
    public Dictionary<string, CardG> Cards {get; set;}


    public ElementalProgram(CodeLocation location) : base (location)
    {
        Errors = new List<CompilingError>();
        Effects = new Dictionary<string, Effect>();
        Cards = new Dictionary<string, CardG>();
    }
    
    /* To check a program semantic we sould first collect all the existing effects and store them in the context.
    Then, we check semantics of effects and cards */
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkEffects = true;
        foreach (Effect effect in Effects.Values)
        {
            checkEffects = checkEffects && effect.CollectElements(context, scope.CreateChild(), errors);
        }
        foreach (Effect effect in Effects.Values)
        {
            checkEffects = checkEffects && effect.CheckSemantic(context, scope.CreateChild(), errors);
        }

        bool checkCards = true;
        foreach (CardG card in Cards.Values)
        {
            checkCards = checkCards && card.CheckSemantic(context, scope, errors);
        }

        return checkCards && checkEffects;
    }

    /*public void Evaluate()
    {
        foreach (CardG card in Cards.Values)
        {
            card.Evaluate();
        }
    }
*/
    public override string ToString()
    {
        string s = "";
        foreach (Effect effect in Effects.Values)
        {
            s = s + "\n" + effect.ToString();
        }
        foreach (CardG card in Cards.Values)
        {
            s += "\n" + card.ToString();
        }
        return s;
    }
}