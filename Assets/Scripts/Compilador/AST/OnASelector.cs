using System.Collections.Generic;
using System.Diagnostics;

public class SelectorOnActivation : ASTNode
{
    public string Source;
    public Expression Single;
    public Predicate Predicate;
    
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkSource = false;
        if (Source=="board"||Source=="hand"||Source=="otherHand"||Source=="deck"||Source=="otherDeck"||Source=="field"||Source=="otherField"||Source=="parent")
        {
            checkSource = true;
        }
        else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The source must be board, deck, otherDeck, hand, field, otherHand and otherField"));

        bool checkSingle = Single.CheckSemantic(context, scope, errors);
        
        if (Single.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The single must be bool"));
        }

        bool checkPredicate = Predicate.CheckSemantic(context, scope, errors);
        if (Predicate.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The predicate must be bool"));
        }

        return checkSource && checkSingle && checkPredicate;
    }



    public SelectorOnActivation(CodeLocation location) : base (location)
    {
        
    }
}