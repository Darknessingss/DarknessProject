using UnityEngine;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text messageText;

    void Start()
    {
        if (messageText == null)
        {
            messageText = GetComponentInChildren<TMP_Text>();
        }
    }

    public void SetMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanelWithFade(float fadeTime = 0.5f)
    {
        gameObject.SetActive(true);
    }
}