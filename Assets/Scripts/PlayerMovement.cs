using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float health = 100;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    AudioSource audioSource;
    public float stepRate = 0.5f;
    float stepCooldown;
    public AudioClip footstep;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        //Checking if the player is touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Get the input for the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Move the player
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //Does the jump stuff
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Footstep Sounds
        
        stepCooldown -= Time.deltaTime;
        if (stepCooldown < 0 && isGrounded && (move.x != 0 || move.z != 0)) //pipes || mean or
        {
            stepCooldown = stepRate;
            audioSource.clip = footstep;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
             
        }
    }

    public void Hit(int _damage)
    {
        health -= _damage;
        //print("Player health:" + health);
        _AM.PlaySound(_AM.GetEnemyHitSound(), audioSource);
        if (health < 0)
        {
            _GM.gameState = GameState.GameOver;
            _AM.PlaySound(_AM.GetEnemyDieSound(), audioSource);
        }
                   
    }

    
}
