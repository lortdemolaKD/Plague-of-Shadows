using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class playermovment : MonoBehaviour
{
    [Header("Movment")]
    private float movmentSpeed;
    public float WalkSpeed;
    public float RunSpeed;

    public float groundDrag;

    [Header("jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplayer;
    bool readyToJump;

    [Header("Crouching")]
    public float crounchSpeed;
    public float crounchYScale;
    private float startYScale;

    [Header("Kebinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode AttackKey = KeyCode.Mouse0;
    public KeyCode placeKey = KeyCode.G;
    public KeyCode extractionKey = KeyCode.F;
    public KeyCode hand1 = KeyCode.Alpha1;
    public KeyCode hand2 = KeyCode.Alpha2;
    public KeyCode menuKey = KeyCode.Escape;

    [Header("Ground Check")]
    public float heighPlayer;
    public LayerMask whatIsGround;
    public LayerMask whatIsCollector;
    bool grounded;

    public Transform orientation;
    float horInput;
    float verInput;
    Vector3 moveDirect;
    Rigidbody rb;

    [Header("Attaking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;
    bool readyToAttack = true;
    bool attacking = false;

    [Header("Audio")]
    public AudioSource aSorce;
    public AudioClip walkA;

    public Animator []anims;
    public Camera cam;
    public bool gotLantern = true;
    public GameObject lanternPrefab;

    [Header("inventory")]
    public bool isMenuOpen;
    public GameObject menuPrefab;
    public List<Tuple<int, int>> invItems = new List<Tuple<int, int>>{};
    public light_setup lights;
    public Light Hendlights;

    public MoveState state;
    public enum MoveState 
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        gotLantern = true;
        isMenuOpen = false;
        startYScale = transform.localScale.y;
    }
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, heighPlayer * 0.5f + 0.2f, whatIsGround);
        MyInput();
        speedControl();
        StateHandler();
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        movePlayer();
    }
    private void StateHandler()
    {
        if (Input.GetKey(crouchKey))
        {
            state = MoveState.crouching;
            movmentSpeed = crounchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MoveState.sprinting;
            movmentSpeed = RunSpeed;
        }
        else if (grounded)
        {
            state = MoveState.walking;
            movmentSpeed = WalkSpeed;
        }
        else
        {
            state = MoveState.air;
        }
    }
    private void MyInput()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crounchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            
        }
        if (Input.GetKeyDown(AttackKey) && readyToAttack)
        {
            Attack();

        }
        if (Input.GetKeyUp(AttackKey))
        {
            if (anims[0].gameObject.activeSelf)
            {
                Hendlights.range = 18f;
            }
            anims[1].SetBool("attaking", false);

        }
        if (Input.GetKey(placeKey) && gotLantern )
        {
            
            placeLantern();
            
        }
        if (Input.GetKeyDown(extractionKey) )
        {

           
            collectItem();

        }
        if (Input.GetKeyDown(menuKey))
        {

            Open_close_Menu();

        }
        if (Input.GetKeyDown(hand1))
        {

            changeHand(0);

        }
        if (Input.GetKeyDown(hand2))
        {

            changeHand(1);

        }

    }
    private void Jump()
    {
       rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
       rb.AddForce(transform.up *jumpForce,ForceMode.Impulse);


    }
    public void Open_close_Menu()
    {
        if (isMenuOpen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            menuPrefab.SetActive(false);
            isMenuOpen = false;
        }
        else
        {
            menuPrefab.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            isMenuOpen= true;
        }


    }
    private void placeLantern()
    {

        Vector3 temp = cam.transform.position + cam.transform.forward;
        if (Physics.Raycast(temp, transform.up * -1.0f, out RaycastHit hit, attackDistance))
        {
            
            if (hit.collider.name == "Terrain")
            {
                
               
                if (Input.GetKey(crouchKey))
                    Instantiate(lanternPrefab, new Vector3(temp.x, cam.transform.position.y - 1f , temp.z), Quaternion.identity);
                else
                    Instantiate(lanternPrefab, new Vector3(temp.x, cam.transform.position.y - 1.8f, temp.z), Quaternion.identity);
            }
            
           
            gotLantern = false;
        }

    }
    private void collectItem()
    {
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, whatIsCollector))
        {
            if (hit.transform.tag == "lantern")
            {
                gotLantern = true;
                Destroy(hit.transform.gameObject);
            }
       
            if (hit.transform.gameObject.GetComponent<shadow_Collect_obj>() != null )
            {
                int idItem = hit.transform.gameObject.GetComponent<shadow_Collect_obj>().CollectOut();
                if (invItems.Find(eri => eri.Item1 == idItem) == null)
                {
                    invItems.Add(Tuple.Create(idItem, 1));

                }
                else
                {
                    int tIndex = invItems.FindIndex(eri => eri.Item1 == idItem);
                    invItems[tIndex] = Tuple.Create(invItems[tIndex].Item1, invItems[tIndex].Item2 + 1);
                }
            }
            if (hit.transform.gameObject.GetComponent<ITEM_Obj>() != null)
            {
                int idItem = hit.transform.gameObject.GetComponent<ITEM_Obj>().CollectOut();
              
                if (invItems.Find(eri => eri.Item1 == idItem) == null)
                {
                    invItems.Add(Tuple.Create(idItem, 1));
                    
                }
                else
                {
                    int tIndex = invItems.FindIndex(eri => eri.Item1 == idItem);
                    invItems[tIndex] = Tuple.Create(invItems[tIndex].Item1, invItems[tIndex].Item2 + 1);
                }
            }
         

        }
    }
    private void ResetJump()
    {
       readyToJump = true;


    }
    private void changeHand(int handNum)
    {
        if (handNum == 0 && lights.set_time_night) {
            anims[1].gameObject.SetActive(false);
            anims[0].gameObject.SetActive(true);
        }
        if (handNum == 1 )
        {
            anims[0].gameObject.SetActive(false);
            anims[1].gameObject.SetActive(true);
        }


    }
    private void speedControl()
    { 
        Vector3 flatVel = new Vector3(rb.velocity.x,0f, rb.velocity.z);
        if(flatVel.magnitude > movmentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movmentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void movePlayer()
    {
        moveDirect = orientation.forward * verInput + orientation.right * horInput;
        if (horInput > 0 || verInput > 0)
        {
            aSorce.clip = walkA;
            aSorce.enabled = true;
            aSorce.volume = rb.velocity.magnitude;
        }
        else
        {
            aSorce.clip = null;
            aSorce.enabled = false;
        }
        if (grounded)
            rb.AddForce(moveDirect.normalized * movmentSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirect.normalized * movmentSpeed * 10f * airMultiplayer, ForceMode.Force);

    }
    public void Attack()
    {
        if (anims[0].gameObject.activeSelf)
        {
            Hendlights.range = 28f;
        }
        else
        {
            if (!readyToAttack || attacking) return;
            anims[1].SetBool("attaking", true);
            readyToAttack = false;
            attacking = true;


            Invoke(nameof(ResetAttack), attackSpeed);
            AttackRaycast();
            /*audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(swordSwing);

            if (attackCount == 0)
            {
                ChangeAnimationState(ATTACK1);
                attackCount++;
            }
            else
            {
                ChangeAnimationState(ATTACK2);
                attackCount = 0;
            }*/
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
          
            Debug.Log(hit.transform.gameObject.name);
            hit.transform.gameObject.GetComponentInParent<EnemyAi>().TakeDamage(1);
        }
    }

}
