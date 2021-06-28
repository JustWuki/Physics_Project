using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastReflectinon : MonoBehaviour
{
    public LineRenderer lineRenderer;

    // ray and hit variables
    private Ray2D ray;
    private RaycastHit2D hit;

    //  angle of incidence
    private Vector2 inDirection;

    // how many times the ray reflects before it does not get reflected anymore
    public int numReflections = 2;

    // starting amount of points
    private int points = 1;

    // Update is called once per frame
    void Update()
    {
        // clear all previously drawn rays
        points = 1;
        lineRenderer.positionCount = 0;

        // add starting point for line renderer
        lineRenderer.positionCount = points;
        lineRenderer.SetPosition(points - 1, this.gameObject.transform.position);
        
        DrawReflection(this.gameObject.transform.position, transform.right, numReflections); 
    }

    private void DrawReflection(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        // stop recursion if no more reflections should be done
        if (reflectionsRemaining == 0)
        {
            return;
        }

        // set starting positions of ray
        Vector3 startingPosition = position;

        Ray2D ray = new Ray2D(position, direction);
        // cast ray
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 200);

        // if ray hit something
        if (hit.collider != null)
        {

            // calculate angle of incidence
            // alternatively you could get the angle by calculating the dot product Vector2.Dot(-direction, hit.normal) and rotate the reflectecd vector by it
            direction = Vector3.Reflect(direction, hit.normal);
            
            position = hit.point;  
        }
        else 
        {
            // otherwise set far away position for lineRenderer to draw
            position += direction * 200;
        }

        // add a point to the lineRenderer
        points++;
        lineRenderer.positionCount = points;
        lineRenderer.SetPosition(points - 1, position);

        Debug.DrawLine(startingPosition, position, Color.blue);
        // call function recursively
        // angle of refelction is angle of incidence
        DrawReflection(position, direction, reflectionsRemaining - 1);
    }
}
