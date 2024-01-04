using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GameBehaviour
{
    public int damage = 20;

    void Start()
    {
        //Destroy projectile after 5 seconds
        Destroy(this.gameObject, 5);
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Check if we hit the object tagged Target
        if (collision.gameObject.CompareTag("Target"))
        {
            if (collision.gameObject.GetComponent<Target>() != null)
            {
                collision.gameObject.GetComponent<Target>().Hit();
            }
            //Change the colour of the target
            //collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Destroy the target after 1 second
            //Destroy(collision.gameObject, 1);
            //Destroy this object
            //Destroy(this.gameObject);
        
        }    
    }
}
