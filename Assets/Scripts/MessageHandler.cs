using System.Collections;
using UnityEngine;
using TMPro;

public class MessageHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;

    [Header("Attributes")]
    [SerializeField] private float messageDelay = 1f;

    public static MessageHandler main;


    private void Awake()
    {
        anim.gameObject.SetActive(false);
        main = this;
    }

    public void ShowMessage()
    {
        anim.gameObject.SetActive(true);

        // Make sure it isn't already running
        if (anim.GetBool("ShowMessage") == true) return;

        StartCoroutine(ShowAndHideMessage());
    }

    private IEnumerator ShowAndHideMessage()
    {
        // Show message, wait ... (1) second and hide message
        anim.SetBool("ShowMessage", true);
        yield return new WaitForSeconds(messageDelay);
        anim.SetBool("ShowMessage", false);
    }
}
