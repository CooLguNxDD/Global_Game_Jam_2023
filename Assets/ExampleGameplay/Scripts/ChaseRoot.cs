using UnityEngine;
using UnityEngine.AI;

public class ChaseRoot : MonoBehaviour
{
    public bool rootFound;

    public GameObject target;
    private NavMeshAgent agent;
    private bool stop;

    public bool showPath;
    public bool showAhead;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rootFound)
        {
            // get target location

            // navigate
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Navigate.DrawGizmos(agent, showPath, showAhead);
    }
}
