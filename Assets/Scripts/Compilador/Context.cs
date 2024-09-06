/* Our context stores the declared elements and cards.
This project doesn't collect and check the cards existence.
That could be a fine exercise for you */
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