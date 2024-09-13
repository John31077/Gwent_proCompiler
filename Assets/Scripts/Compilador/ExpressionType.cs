using System.Collections.Generic;

public enum ExpressionType
{
    Anytype,
    String,
    Number,
    Identifier,
    Bool,
    Card,
    List,
    Method,
    Property,
    Context,
    Player,
    ContextList,
    ErrorType
}

public enum IdentifierType
{
    context,
    TriggerPlayer,
    Board,
    HandOfPlayer,
    FieldOfPlayer,
    GraveyardOfPlayer,
    DeckOfPlayer,
    Hand,
    Field,
    Deck,
    Graveyard,
    Owner,
    Find,
    Push,
    SendBottom,
    Pop,
    Remove,
    Shuffle,
    Type,
    Name,
    Faction,
    Power,
    Range

}
public class ListOfIdentifiers
{
    public static List<string> IdentifiersList = new List<string>(){"Power","Name","Type","Faction","Range","TriggerPlayer","Board","Hand",
                                                                    "Deck","Field","Graveyard","Owner"};
}