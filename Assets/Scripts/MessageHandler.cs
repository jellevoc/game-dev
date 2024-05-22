using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI messageText;

    public static MessageHandler main;

    private string baseText;

    private void Awake()
    {
        anim.gameObject.SetActive(false);
        main = this;
    }

    private void Start()
    {
        baseText = messageText.text;
    }

    public void ShowMessage()
    {
        anim.gameObject.SetActive(true);
        if (anim.GetBool("ShowMessage") == true) return;
        StartCoroutine(ShowAndHideMessage());
    }

    private IEnumerator ShowAndHideMessage()
    {
        anim.SetBool("ShowMessage", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("ShowMessage", false);
    }

    public void ShowCustomMessage(string _message)
    {
        messageText.text = _message;
        anim.gameObject.SetActive(true);
        if (anim.GetBool("ShowMessage") == true) return;
        StartCoroutine(ShowAndHideMessage());
        messageText.text = baseText;
    }
}
