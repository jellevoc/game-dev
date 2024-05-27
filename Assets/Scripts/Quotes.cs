using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quotes
{
    public List<string> GeorgeOrwellQuotes { get; set; }
    public List<string> AtlasShruggedQuotes { get; set; }

    public Quotes()
    {
        GeorgeOrwellQuotes = new List<string>
        {
            "“Big Brother is Watching You.”",
            "“It's a beautiful thing, the destruction of words.”",
            "“Who controls the past controls the future. Who controls the present controls the past.”",
            "“In a time of deceit telling the truth is a revolutionary act.”",
            "“War is peace. Freedom is slavery. Ignorance is strength.”",
            "“If you want to keep a secret, you must also hide it from yourself.”"
        };

        AtlasShruggedQuotes = new List<string>
        {
            "“Wealth is the product of man’s capacity to think.”",
            "“There are no evil thoughts except one: the refusal to think.”",
            "“The world you desire can be won. It exists.. it is real.. it is possible.. it’s yours.”",
            "“If you don’t know, the thing to do is not to get scared, but to learn.”"
        };
    }

    public string GetRandomQuote()
    {
        int totalQuotes = GeorgeOrwellQuotes.Count + AtlasShruggedQuotes.Count;
        int randomIndex = Random.Range(0, totalQuotes);

        if (randomIndex < GeorgeOrwellQuotes.Count)
        {
            return GeorgeOrwellQuotes[randomIndex] + " \n - George Orwell";
        }
        else
        {
            return AtlasShruggedQuotes[randomIndex - GeorgeOrwellQuotes.Count] + " \n - Atlas Shrugged";
        }
    }
}
