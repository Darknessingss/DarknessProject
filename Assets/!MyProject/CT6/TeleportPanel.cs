using UnityEngine;
using TMPro;

public class TeleportPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private string panelMessage = "Добро пожаловать во вторую локацию!";

    private void Start()
    {
        if (messageText != null)
        {
            messageText.text = panelMessage;
        }
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf && Input.anyKeyDown)
        {
            HidePanel();
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
}