using UnityEngine;

public class ButtonMessage : MonoBehaviour
{
    [SerializeField] private string messageText = "Это обманка, !!!ХАХАХАХА!!!";

    private bool isActivated = false;

    [SerializeField] private MessagePanel messagePanel;


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
