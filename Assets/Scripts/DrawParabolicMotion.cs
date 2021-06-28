using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class draws the parabolic motion of a projectile depending on the given force and angle
 * it also shoots a projectile along the ark
 */
public class DrawParabolicMotion : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Range(3, 30)]
    private int lineSegmentCount = 20;

    private List<Vector3> linePoints = new List<Vector3>();

    [SerializeField]
    [Range(0, 90)]
    float angle = 45;
    [SerializeField]
    [Range(0, 700)]
    float shotForce = 200;

    [SerializeField]
    [Range(0, 10)]
    int shotSpeed = 5;

    public GameObject projectile;

    private void Start()
    {
        // start coroutine once
        StartCoroutine(SpawnBullet());
    }

    private void Update()
    {
        DrawLineTrajectory(RotateVectorByAngle(transform.right, transform.forward, angle) * shotForce, transform.position + transform.right * 1);
    }

    // Shoot an object in given interval in the arc that has been drawn
    public IEnumerator SpawnBullet()
    {
        while (true)
        {
            // Spawn Bullet in front of gameObject
            GameObject bullet = Instantiate(projectile, transform.position + transform.right * 1, Quaternion.identity) as GameObject;
            // Apply Force at a given angle in defined direction
            // function parameters: direction, pivot, angle
            bullet.GetComponent<Rigidbody>().AddForce(RotateVectorByAngle(transform.right, transform.forward, angle) * shotForce);

            // wait for given duration
            yield return new WaitForSeconds(shotSpeed);
        }
    }

    // rotate a vector by a given angle around a given pivot
    public Vector3 RotateVectorByAngle(Vector3 originalVector, Vector3 pivot, float angle)
    {
        return Quaternion.AngleAxis(angle, pivot) * originalVector;
    }

    public void DrawLineTrajectory(Vector3 forceVector, Vector3 startingPoint, float projectileMass = 1)
    {
        /* calculate velocity based on F = m * a
         * m = mass, a = acceleration
         * a = v / delta_t (accelartion is change in velocity over a time)
         * 
         * so we can calculate the velocity by:
         * v = (F * delta_t) / m
         */
        Vector3 velocity = (forceVector / projectileMass) * Time.fixedDeltaTime;

        // calculate the range of the trajectory
        // http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html#tracon (Range of Trajectory)
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;

        // get the timestep for each point on lineRenderer
        float stepTime = FlightDuration / lineSegmentCount;

        // clear the previous values
        linePoints.Clear();

        // calculate linePoint for every lineRenderer segment
        for (int i = 0; i < lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;

            // calculate the current location of the projectile depending on the current timeStep
            // http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html#tracon (General Ballistic Trajectory)
            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed
            );

            // add newly calculated point to Array
            linePoints.Add(-MovementVector + startingPoint);
        }

        // add all the calculated points from the array to the lineRenderer
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    // function to hide points (for debugging)
    public void HideLine()
    {
        lineRenderer.positionCount = 0;
    }
}
