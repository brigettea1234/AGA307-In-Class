using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public int myDamage = 20;
    EnemyHealthBar healthBar;

    public string myName;

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    Transform startPos;         //Needed for loop patrol movement
    Transform endPos;           //Needed for loop patrol movement
    bool reverse;               //Needed for loop patrol movement
    int patrolPoint = 0;        //Needed for linear patrol movement
    Animator anim;
    AudioSource audioSource;

    public float attackDistance = 5;
    public float detectTime = 5f;
    public float detectDistance = 10f;
    int currentWaypoint;    //Invisible navigation point
    NavMeshAgent agent;



    void Start()
    {


        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        SetName(_EM.GetEnemyName());

        switch (myType)
        {
            case EnemyType.OneHand:
                health = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Patrol;
                myScore = 100;
                myDamage = 20;
                break;
            case EnemyType.TwoHand:
                health = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Patrol;
                myScore = 200;
                myDamage = 30;
                break;
            case EnemyType.Archer:
                health = maxHealth = baseHealth / 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Patrol;
                myScore = 300;
                myDamage = 40;
                break;
        }

        SetupAI();
        if (GetComponentInChildren<EnemyWeapon>() != null)
        GetComponentInChildren<EnemyWeapon>().damage = myDamage;
    }

    
    void SetupAI()
    {
        //Getting a random waypoint
        currentWaypoint = UnityEngine.Random.Range(0, _EM.spawnPoints.Length);
        //The nav mesh agent of this enemy. Agent move to this place
        agent.SetDestination(_EM.spawnPoints[currentWaypoint].position);
        //Changing speed so they are not all the same speed
        ChangeSpeed(mySpeed);
    }

    /// <summary>
    /// Changing the speed of the enemies
    /// </summary>
    /// <param name="_speed"></param>
    void ChangeSpeed(float _speed)
    {
        agent.speed = _speed;
    }

    private void Update()
    {
        if (myPatrol == PatrolType.Die)
            return;

        //A float value between 2 vectors, Distance between the player and the enemy
        float distToPlayer = Vector3.Distance(transform.position, _PLAYER.transform.position);
        //If within the distance, not attacking, we are in the detect state
        if (distToPlayer <= detectDistance && myPatrol != PatrolType.Attack)
        {
            if (myPatrol != PatrolType.Chase)
            {
                myPatrol = PatrolType.Detect;
            }
        }

        //Set the animators speed parameter to that of my speed
        anim.SetFloat("Speed", mySpeed);

        //Switching patrol states logic
        switch(myPatrol)
        {
            case PatrolType.Patrol:
                //Get the distance between us and the current waypoint
                float distToWaypoint = Vector3.Distance(transform.position, _EM.spawnPoints[currentWaypoint].position);
                //If the distance is close enough, get a new waypoint
                if (distToWaypoint < 1)
                    SetupAI();
                //Reset the detect time
                detectTime = 5;
                break;
            
            case PatrolType.Detect:
                //Set the destination to ourself, essentially stopping us
                agent.SetDestination(transform.position);
                //stop our speed
                ChangeSpeed(0);
                //Decrement our detect time
                detectTime -= Time.deltaTime;
                if(distToPlayer <= detectDistance)
                {
                    myPatrol = PatrolType.Chase;
                    detectTime = 5;
                }
                //if detect timge gets to 0, set up AI again
                if (detectTime <= 0)
                {
                    myPatrol = PatrolType.Patrol;
                    SetupAI();
                }
                break;
            case PatrolType.Chase:
                //Set the destination to that of the player
                agent.SetDestination(_PLAYER.transform.position);
                //Increase the speed of which to chase the player
                ChangeSpeed(mySpeed * 2);
                //If the player gets outside the detect distance, go back to the detect state
                if (distToPlayer > detectDistance)      //Single line if statement
                    myPatrol = PatrolType.Detect;
                //If we are close to the player, then attack
                if (distToPlayer <= attackDistance)
                    StartCoroutine(Attack());
                break;
        }
    }

    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(_name);
    }
    
    IEnumerator Attack()
    {
        //Attack state
        myPatrol = PatrolType.Attack;
        ChangeSpeed(0);
        PlayAnimation("Attack");
        _AM.PlaySound(_AM.GetEnemyAttackSound(), audioSource);
        yield return new WaitForSeconds(1);
        ChangeSpeed(mySpeed);
        myPatrol = PatrolType.Chase;
        
    }

    private void Hit(int _damage)
    {
        //Damage because different enemeies have different healths
        health -= _damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        //ScaleObject(this.gameObject, transform.localScale * 1.1f);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            PlayAnimation("Hit");
            OnEnemyHit?.Invoke(this.gameObject);

            _AM.PlaySound(_AM.GetEnemyHitSound(), audioSource);
        }
        
    }

    public void Die()
    {
        myPatrol = PatrolType.Die;
        ChangeSpeed(0);
        GetComponent<Collider>().enabled = false;
        PlayAnimation("Die");
        StopAllCoroutines();
        //Because the event has a game object, we put the game object here
        OnEnemyDie?.Invoke(this.gameObject);
        
        //_GM.AddScore(myScore * 2);
        //If the coroutines running when the object gets destroyed, it still runs in the background
        
        //Removes the enemeny from our EnemyManager
        _EM.KillEnemy(this.gameObject);
        _AM.PlaySound(_AM.GetEnemyDieSound(), audioSource);
    }

    void PlayAnimation(string _animationName)
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        anim.SetTrigger(_animationName + rnd);
    }

    
    public void PlayFootstep()
    {
        _AM.PlaySound(_AM.GetEnemyFootstepSound(), audioSource, 0.1f);
      
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
