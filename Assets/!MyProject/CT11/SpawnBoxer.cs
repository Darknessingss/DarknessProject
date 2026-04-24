using UnityEngine;

public class SpawnBoxer : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Camera Cameramain;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Cameramain.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Instantiate(obstaclePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}