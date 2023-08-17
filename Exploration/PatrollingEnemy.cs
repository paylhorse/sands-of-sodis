using UnityEngine;
using Pathfinding;

// **** Backbone of Patrolling Enemies in the Field

public class PatrollingEnemy : MonoBehaviour
{
    public Transform[] waypoints;
    public float stopDuration = 1f;
    public float patrolSpeed = 3.5f;

    private AIPath ai;
    private int currentWaypointIndex;
    private float stopTimer;

    private void Start()
    {
        ai = GetComponent<AIPath>();

        // Set the patrol speed
        ai.maxSpeed = patrolSpeed;

        if (waypoints.Length > 0)
        {
            ai.destination = waypoints[0].position;
        }
    }

    private void Update()
    {
        // Check if the AI has reached its current waypoint
        if (ai.reachedEndOfPath && !ai.pathPending)
        {
            stopTimer += Time.deltaTime;

            // Check if the stop duration has elapsed
            if (stopTimer >= stopDuration)
            {
                // Reset the timer
                stopTimer = 0f;

                // Randomly choose the next waypoint, making sure it's not the current one
                int nextWaypointIndex;
                do
                {
                    nextWaypointIndex = Random.Range(0, waypoints.Length);
                } while (nextWaypointIndex == currentWaypointIndex);

                // Update the current waypoint index
                currentWaypointIndex = nextWaypointIndex;

                // Set the new destination
                ai.destination = waypoints[currentWaypointIndex].position;
                ai.SearchPath();
            }
        }
    }
}
