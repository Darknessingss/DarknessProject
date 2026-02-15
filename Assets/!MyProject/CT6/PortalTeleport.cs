using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private TeleportPanel portalMessagePanel;
    [SerializeField] private WalletUI walletUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetObject != null && teleportTarget != null)
            {
                targetObject.transform.position = teleportTarget.position;
                Debug.Log($"Телепортация в: {teleportTarget.position}");

                if (portalMessagePanel != null)
                {
                    portalMessagePanel.ShowPanel();
                }

                if (walletUI != null)
                {
                    walletUI.ShowWallet();
                }
            }
        }
    }
}