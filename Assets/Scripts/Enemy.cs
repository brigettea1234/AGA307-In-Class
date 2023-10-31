using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameBehaviour
{
    public static event Action<GameObject> OnEnemyHit = null;
    public static event Action<GameObject> OnEnemyDie = null;

    public PatrolType myPatrol;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    //1000 frames
    float moveDistance = 1000;

    int baseHealth = 100;
    int maxHealth;
    public int health;
    public int myScore;
    EnemyHealthBar healthBar;

    public string myName;

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    Transform startPos;         //Needed for loop patrol movement
    Transform endPos;           //Needed for loop patrol movement
    bool reverse;               //Needed for loop patrol movement
    int patrolPoint = 0;        //Needed for linear patrol movement


    void Start()
    {

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        SetName(_EM.GetEnemyName());

        switch (myType)
        {
            case EnemyType.OneHand:
                health = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                myScore = 100;
                break;
            case EnemyType.TwoHand:
                health = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Random;
                myScore = 200;
                break;
            case EnemyType.Archer:
                health = maxHealth = baseHealth / 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Loop;
                myScore = 300;
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
        if (Input.GetKeyDown(KeyCode.Escape))
            StopAllCoroutines();
        if (Input.GetKeyDown(KeyCode.H))
            Hit(30);
    }

    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(_name);
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

    public void Hit(int _damage)
    {
        //Damage because different enemeies have different healths
        health -= _damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        ScaleObject(this.gameObject, transform.localScale * 1.1f);
        if (health <= 0)
        {
            Die();
        }
        else
        {
            OnEnemyHit?.Invoke(this.gameObject);
            //_GM.AddScore(myScore);

        }
        
    }

    public void Die()
    {
        StopAllCoroutines();
        //Because the event has a game object, we put the game object here
        OnEnemyDie?.Invoke(this.gameObject);
        //_GM.AddScore(myScore * 2);
        //If the coroutines running when the object gets destroyed, it still runs in the background
        
        //Removes the enemeny from our EnemyManager
        _EM.KillEnemy(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            Hit(collision.gameObject.GetComponent<Projectile>().damage);
            Destroy(collision.gameObject);
        }
            
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
