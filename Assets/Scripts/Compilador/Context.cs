/* Our context stores the declared effects and cards.
Doesn't collect and check the cards existence for now.*/
using System.Collections.Generic;

public class Context
{
    public List<string> effects;
    public List<string> cards;

    public Context()
    {
        effects = new List<string>();
        cards = new List<string>();
    }
}