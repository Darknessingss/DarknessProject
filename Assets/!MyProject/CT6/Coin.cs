using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int coinValue = 1;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 0.2f;

    [Header("UI Reference")]
    [SerializeField] private WalletUI walletUI;

    private Vector3 startPosition;
    private float floatOffset;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPosition + Vector3.up * floatOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (walletUI != null)
            {
                walletUI.AddCoins(coinValue);
            }

            Destroy(gameObject);
            Debug.Log($"Монетка собрана! +{coinValue}");
        }
    }
}