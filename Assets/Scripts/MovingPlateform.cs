using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public FlyAndDash flyDash;
    public float speed; //speed of the plateform
    public int startingPoint; //starting index (position of the plateform)
    public Transform[] points; // An array of transform points

    private int i; //inde of the array

    void Start()
    {
        transform.position = points[startingPoint].position; //Setting the position of the plateform to
                                                             // the position of one of the point using the index
    }

    void Update()
    {
        // Checking the distance of the pateform and the point
        if(Vector2.Distance(transform.position, points[i].position) < 0.02f) 
        {
            i++; //increase the index
            if(i== points.Length) // Check if the plat was on the last point after the index increase
            { 
                i = 0; //Reset the index
            }
        }

        //moving the plateform to the point position with the index "i"
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(flyDash.is_flying == false)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (flyDash.is_flying == false)
        {
            collision.transform.SetParent(null);
        }
            
    }
}
