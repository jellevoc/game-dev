using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomQuote : MonoBehaviour
{
    private TextMeshProUGUI quoteText;

    private Quotes quotes;

    private void Start()
    {
        quotes = new Quotes();
        quoteText = gameObject.GetComponent<TextMeshProUGUI>();
        quoteText.text = quotes.GetRandomQuote();
    }

}
