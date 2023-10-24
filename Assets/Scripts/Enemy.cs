using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public PatrolType myPatrol;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    //1000 frames
    float moveDistance = 1000;

    int baseHealth = 100;
    public int health;

    [Header ("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    public EnemyManager _EM;    //Referencing the Enemy Manager script
    Transform startPos;         //Needed for loop patrol movement
    Transform endPos;           //Needed for loop patrol movement
    bool reverse;               //Needed for loop patrol movement
    int patrolPoint = 0;        //Needed for linear patrol movement


    void Start()
    {
        _EM = FindObjectOfType<EnemyManager>();

        switch (myType)
        {
            case EnemyType.OneHand:
                health = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                break;
            case EnemyType.TwoHand:
                health = baseHealth * 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Random;
                break;
            case EnemyType.Archer:
                health = baseHealth / 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Loop;
                break;
        }

        SetupAi();
    }

    void SetupAi()
    {
        
        startPos = Instantiate(new GameObject(), transform.position, transform.rotation).transform;
        endPos = _EM.GetRandomSpawnPoint();
        moveToPos = endPos;
        //Starts coroutine loop
        StartCoroutine(Move());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            StopAllCoroutines();
    }
    
    IEnumerator Move()
    {
        switch (myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.spawnPoints[patrolPoint];
                patrolPoint = patrolPoint != _EM.spawnPoints.Length ? patrolPoint + 1 : 0;
                break;
            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;
            case PatrolType.Loop:
                moveToPos = reverse ? startPos : endPos; //if reverse is true its start, if not it's end
                reverse = !reverse;
                break;
        }



        transform.LookAt(moveToPos);
        //While our distance is greater than 0.3
        while (Vector3.Distance(transform.position, moveToPos.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1);
                
        StartCoroutine(Move());
    }

    /*IEnumerator Move()
    {
        //Coroutine loop
        for (int i = 0; i < moveDistance; i++)
        {
            //Move the skeleton forward
            transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
            yield return null;
        }


        transform.Rotate(Vector3.up * 180);
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(Move());
    }*/

}
