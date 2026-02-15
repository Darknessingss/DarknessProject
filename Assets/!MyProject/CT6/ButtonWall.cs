using UnityEngine;

public class ButtonWall : MonoBehaviour
{
    [Header("Target Object")]
    [SerializeField] private GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player"))
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false);
                Debug.Log($"Объект выключен: {targetObject.name}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") || other.CompareTag("Player"))
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                Debug.Log($"Объект включен: {targetObject.name}");
            }
        }
    }
}