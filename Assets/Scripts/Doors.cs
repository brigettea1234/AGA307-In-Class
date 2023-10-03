using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{

    public GameObject door1;
    public GameObject door2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Turn off the doors
            door1.SetActive(false);
            door2.SetActive(false);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Turn on the doors
            door1.SetActive(true);
            door2.SetActive(true);
        }
                        
    }
}
