using UnityEngine;
using UnityEngine.AI;

public class NavmeshMovement : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Camera myCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Agent.SetDestination(hit.point);
            }
        }
    }
}