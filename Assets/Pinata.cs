using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{
    public FlyAndDash flydash;
    [SerializeField] GameObject itself;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && flydash.isDashing)
        {
            itself.SetActive(false);
        }
    }
}
