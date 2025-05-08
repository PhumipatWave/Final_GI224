using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : Character
{
    public int EnemyKilled;
    public InputActionAsset actionAsset;

    private float horizontalInput;
    private InputAction moveAction;
    private InputAction shootAction;
    private InputActionMap currentActionMap;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        Health = 100;
        Damage = 25;
        Speed = 175f;
        AttackCooldown = 3f;

        PlayerInputSetUp();
    }

    private void Update()
    {
        Attack();
        Death();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInputSetUp()
    {
        if (this.CompareTag("Player1"))
        {
            currentActionMap = actionAsset.FindActionMap("Player1");
            Debug.Log(currentActionMap);
        }
        else if (this.CompareTag("Player2"))
        {
            currentActionMap = actionAsset.FindActionMap("Player2");
            Debug.Log(currentActionMap);
        }

        if (currentActionMap != null)
        {
            currentActionMap.Enable();
            moveAction = currentActionMap.FindAction("Move");
            shootAction = currentActionMap.FindAction("Shoot");

            Debug.Log($"Enabled action map: {currentActionMap.name}");
        }
    }

    public override void Death()
    {
        if (Health <= 0f)
        {
            anim.SetBool("isDeath", true);
        }
    }

    public override void Move()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;

        if (horizontalInput != 0f)
        {
            Vector3 moveDir = new Vector3(horizontalInput, 0f, 0f);
            rb.linearVelocity = moveDir.normalized * Speed * Time.deltaTime;
            anim.SetFloat("moveSpeed", 1f);
        }
        else
        {
            anim.SetFloat("moveSpeed", 0f);
        }
    }

    public override void Attack()
    {
        if (shootAction.triggered)
        {
            Debug.Log("Fire!!!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
}
