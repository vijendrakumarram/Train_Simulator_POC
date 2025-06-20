using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWaypointRandomBehavior : MonoBehaviour
{
    [Header("Waypoints and Movement")]
    public Transform[] waypoints;
    public float reachDistance = 0.5f;

    [Header("Idle Settings")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;
    [Range(0f, 1f)]
    public float idleChance = 0.5f; // 50% chance to idle

    [Header("Animation (Optional)")]
    public Animator animator;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0 || isWaiting) return;

        bool reachedDestination = !agent.pathPending && agent.remainingDistance <= reachDistance;

        // Handle animation
        if (animator)
        {
            bool isWalking = !reachedDestination && agent.velocity.magnitude > 0.05f;
            animator.SetBool("IsWalking", isWalking);
        }

        // If destination reached, start idle coroutine
        if (reachedDestination)
        {
            StartCoroutine(HandleIdleThenMove());
        }
    }

    IEnumerator HandleIdleThenMove()
    {
        isWaiting = true;
        agent.isStopped = true;

        if (Random.value < idleChance)
        {
            if (animator) animator.SetBool("IsWalking", false);
            float idleDuration = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleDuration);
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        agent.isStopped = false;
        isWaiting = false;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 current = waypoints[i].position;
            Vector3 next = waypoints[(i + 1) % waypoints.Length].position;
            Gizmos.DrawLine(current, next);
        }
    }
}
