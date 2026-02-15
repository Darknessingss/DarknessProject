using UnityEngine;

public class WinnerButton : MonoBehaviour
{
    [SerializeField] private string messageText = "Молодец Ты победил, возьми с полки огурец и закрывай игру";

    private bool isActivated = false;

    [SerializeField] private WinnerPanel messagePanel;


    void Start()
    {

        if (messagePanel != null)
        {
            messagePanel.SetMessage(messageText);
            messagePanel.HidePanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateButton();
        }
    }


    private void ActivateButton()
    {
        if (isActivated) return;

        isActivated = true;

        if (messagePanel != null)
        {
            messagePanel.ShowPanel();
        }

        Debug.Log("Кнопка активирована");
    }

    private void DeactivateButton()
    {
        if (!isActivated) return;

        isActivated = false;

        if (messagePanel != null)
        {
            messagePanel.HidePanel();
        }

        Debug.Log("Кнопка деактивирована");
    }
}
