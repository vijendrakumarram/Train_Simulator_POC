using UnityEngine;
using System.Collections;

public class NPCWaypointRandomBehavior : MonoBehaviour
{
    [Header("Waypoints and Movement")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float reachDistance = 0.2f;

    [Header("Idle Settings")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;
    [Range(0f, 1f)]
    public float idleChance = 0.5f; // 50% chance to idle

    [Header("Animation (Optional)")]
    public Animator animator;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Update()
    {
        if (waypoints.Length == 0 || isWaiting) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (animator) animator.SetBool("IsWalking", true);

        if (Vector3.Distance(transform.position, targetWaypoint.position) <= reachDistance)
        {
            StartCoroutine(HandleIdleThenMove());
        }
    }

    IEnumerator HandleIdleThenMove()
    {
        isWaiting = true;

        if (Random.value < idleChance)
        {
            float idleDuration = Random.Range(minIdleTime, maxIdleTime);
            if (animator) animator.SetBool("IsWalking", false);
            yield return new WaitForSeconds(idleDuration);
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isWaiting = false;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 current = waypoints[i].position;
            Vector3 next = waypoints[(i + 1) % waypoints.Length].position;
            Gizmos.DrawLine(current, next);
        }
    }
}
