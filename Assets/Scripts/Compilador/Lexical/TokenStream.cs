using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/* This stream has functions to operate over a list of tokens.
The methods are simple, you can understand them easily  "flujo de tokens"*/
public class TokenStream : IEnumerable<Token>
{
    private List<Token> tokens;
    private int position;
    public int Position { get { return position; } }
    public int Count { get { return tokens.Count; } }

    public TokenStream(IEnumerable<Token> tokens)
    {
        this.tokens = new List<Token>(tokens);
        position = 0;
    }

    public bool End => position == tokens.Count-1;

    public void MoveNext(int k)
    {
        position += k;
    }

    public void MoveBack(int k)
    {
        position -= k;
    }

     /* The next methods are used to scroll through the token list 
     if a condition is satisfied */

     /* In this case, the condition is to have a next position */
    public bool Next()
    {
        if (position < tokens.Count - 1)
        {
            position++;
        }

        return position < tokens.Count;
    }

    /* In this case, the next position must match the given type */
    public bool Next( TokenType type )
    {
        if (position < tokens.Count-1 && LookAhead(1).Type == type)
        {
            position++;
            return true;
        }

        return false;
    }

    /* In this cas, the next position must match the given value */
    public bool Next(string value)
    {            
        if (position < tokens.Count-1 && LookAhead(1).Value == value)
        {
            position++;
            return true;
        }

        return false;
    }

    public bool CanLookAhead(int k = 0)
    {
        return tokens.Count - position > k;
    }

    public Token LookAhead(int k = 0)
    {
        return tokens[position + k];
    }

    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < tokens.Count; i++)
            yield return tokens[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

