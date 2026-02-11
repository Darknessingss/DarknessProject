using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonDestroy : MonoBehaviour
{
    [Header("Target Object")]
    [SerializeField] private GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetObject != null)
            {
                Destroy(targetObject);
                Debug.Log($"Объект уничтожен: {targetObject.name}");

                Invoke("ReloadScene", 2f);
            }
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}