using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;

    public static MessageHandler main;

    private void Awake()
    {
        anim.gameObject.SetActive(false);
        main = this;
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
}
