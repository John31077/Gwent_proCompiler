public class Compiling //Esto por lo visto si lo invocas con .Lexical basicamente tokeniza todo metiendo los tokens en los diccionarios correspondientes
{
    private static LexicalAnalyzer? __LexicalProcess;
    public static LexicalAnalyzer Lexical
    {
        get
        {
            if (__LexicalProcess == null)
            {
                __LexicalProcess = new LexicalAnalyzer();

                __LexicalProcess.RegisterOperator("+", TokenValues.Add); // +
                __LexicalProcess.RegisterOperator("+=", TokenValues.Add); // +=
                __LexicalProcess.RegisterOperator("++", TokenValues.Add); // ++
                __LexicalProcess.RegisterOperator("*", TokenValues.Mul); // *
                __LexicalProcess.RegisterOperator("*=", TokenValues.Mul); // *=
                __LexicalProcess.RegisterOperator("-", TokenValues.Sub); // -
                __LexicalProcess.RegisterOperator("-=", TokenValues.Sub); // -=
                __LexicalProcess.RegisterOperator("/", TokenValues.Div); // /
                __LexicalProcess.RegisterOperator("/=", TokenValues.Div); // /=
                __LexicalProcess.RegisterOperator("^", TokenValues.Pow); // ^

                __LexicalProcess.RegisterOperator("=", TokenValues.Assign); // =
                __LexicalProcess.RegisterOperator("=>", TokenValues.Implica); // =>
                __LexicalProcess.RegisterOperator(">", TokenValues.MayorQue); // >
                __LexicalProcess.RegisterOperator("<", TokenValues.MenorQue); // <
                __LexicalProcess.RegisterOperator(">=", TokenValues.MayorIgual); // >=
                __LexicalProcess.RegisterOperator("<=", TokenValues.MenorIgual); // <=
                __LexicalProcess.RegisterOperator("==", TokenValues.Igual); // ==
                __LexicalProcess.RegisterOperator("@", TokenValues.Concat); // @
                __LexicalProcess.RegisterOperator("@@", TokenValues.ConcatEspacio); // @@
                __LexicalProcess.RegisterOperator("&&", TokenValues.Conjuncion); // &&
                __LexicalProcess.RegisterOperator("||", TokenValues.Disyuncion); // ||

                __LexicalProcess.RegisterOperator(",", TokenValues.ValueSeparator); // ,
                __LexicalProcess.RegisterOperator(";", TokenValues.StatementSeparator); // ;
                __LexicalProcess.RegisterOperator(":", TokenValues.TwoPoints); // :
                __LexicalProcess.RegisterOperator(".", TokenValues.Point); // .


                __LexicalProcess.RegisterOperator("(", TokenValues.OpenBracket); // (
                __LexicalProcess.RegisterOperator(")", TokenValues.ClosedBracket); // )
                __LexicalProcess.RegisterOperator("{", TokenValues.OpenCurlyBraces); // {
                __LexicalProcess.RegisterOperator("}", TokenValues.ClosedCurlyBraces); // }
                __LexicalProcess.RegisterOperator("[", TokenValues.OpenCorchetes); // [
                __LexicalProcess.RegisterOperator("]", TokenValues.ClosedCorchetes); // ]

                __LexicalProcess.RegisterKeyword("effect", TokenValues.effect); // effect
                __LexicalProcess.RegisterKeyword("Name", TokenValues.Name); // Name
                __LexicalProcess.RegisterKeyword("Params", TokenValues.Params); // Params
                __LexicalProcess.RegisterKeyword("Number", TokenValues.Number); // Number
                __LexicalProcess.RegisterKeyword("String", TokenValues.String); // String
                __LexicalProcess.RegisterKeyword("Bool", TokenValues.Bool); // Bool
                __LexicalProcess.RegisterKeyword("Action", TokenValues.Action); // Action

                __LexicalProcess.RegisterKeyword("targets", TokenValues.targets); // targets
                __LexicalProcess.RegisterKeyword("target", TokenValues.target); // target
                __LexicalProcess.RegisterKeyword("HandOfPlayer", TokenValues.HandOfPlayer); // HandOfPlayer
                __LexicalProcess.RegisterKeyword("FieldOfPlayer", TokenValues.FieldOfPlayer); // FieldOfPlayer
                __LexicalProcess.RegisterKeyword("GraveyardOfPlayer", TokenValues.GraveyardOfPlayer); // GraveyardOfPlayer
                __LexicalProcess.RegisterKeyword("DeckOfPlayer", TokenValues.DeckOfPlayer); // DeckOfPlayer
                __LexicalProcess.RegisterKeyword("Hand", TokenValues.Hand); // Hand
                __LexicalProcess.RegisterKeyword("Field", TokenValues.Field); // Field
                __LexicalProcess.RegisterKeyword("Graveyard", TokenValues.Graveyard); // Graveyard
                __LexicalProcess.RegisterKeyword("Deck", TokenValues.Deck); // Deck
                __LexicalProcess.RegisterKeyword("Owner", TokenValues.Owner); // Owner
                __LexicalProcess.RegisterKeyword("TriggerPlayer", TokenValues.TriggerPlayer); // TriggerPlayer
                __LexicalProcess.RegisterKeyword("Board", TokenValues.Board); // Board

                __LexicalProcess.RegisterKeyword("context", TokenValues.context); // context
                __LexicalProcess.RegisterKeyword("Find", TokenValues.Find); // Find
                __LexicalProcess.RegisterKeyword("Push", TokenValues.Push); // Push
                __LexicalProcess.RegisterKeyword("SendBottom", TokenValues.SendBotttom); // SendBottom
                __LexicalProcess.RegisterKeyword("Pop", TokenValues.Pop); // Pop
                __LexicalProcess.RegisterKeyword("Remove", TokenValues.Remove); // Remove
                __LexicalProcess.RegisterKeyword("Shuffle", TokenValues.Shuffle); // Shuffle

                __LexicalProcess.RegisterKeyword("card", TokenValues.card); // card
                __LexicalProcess.RegisterKeyword("Type", TokenValues.Type); // Type
                __LexicalProcess.RegisterKeyword("Faction", TokenValues.Faction); // Faction
                __LexicalProcess.RegisterKeyword("Power", TokenValues.Power); // Power
                __LexicalProcess.RegisterKeyword("Range", TokenValues.Range); // Range
                __LexicalProcess.RegisterKeyword("OnActivation", TokenValues.OnActivation); // OnActivation
                __LexicalProcess.RegisterKeyword("Effect", TokenValues.Effect); // Effect
                __LexicalProcess.RegisterKeyword("Selector", TokenValues.Selector); // Selector
                __LexicalProcess.RegisterKeyword("Source", TokenValues.Source); // Source
                __LexicalProcess.RegisterKeyword("Single", TokenValues.Single); // Single
                __LexicalProcess.RegisterKeyword("Predicate", TokenValues.Predicate); // Predicate
                __LexicalProcess.RegisterKeyword("PostAction", TokenValues.PostAction); // PostAction

                __LexicalProcess.RegisterKeyword("true", TokenValues.TrueExpresion); // true
                __LexicalProcess.RegisterKeyword("false", TokenValues.FalseExpresion); // false
                
                __LexicalProcess.RegisterKeyword("for", TokenValues.For); // for
                __LexicalProcess.RegisterKeyword("in", TokenValues.In); // in
                __LexicalProcess.RegisterKeyword("while", TokenValues.While); // while
      
                __LexicalProcess.RegisterKeyword("id", TokenValues.id);

                /*  */
                __LexicalProcess.RegisterText("\"", "\"");
            }
            return __LexicalProcess;
        }
    }
}