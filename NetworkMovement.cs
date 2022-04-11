using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class NetworkMovement : NetworkBehaviour
{
    Animator animate; 
    [SerializeField]
    public float walkingspeed = 0.05f;
    [SerializeField]
    public float turnspeed = 120f;
    [SerializeField]
    private float attackspeed = 1f;
    private float attacktimer = 0f;
    private float attackcd = 0.5f;
    Rigidbody rigidbody;
    Quaternion rotation = Quaternion.identity;
    private Vector2 defaultPos = new Vector2(-4, 4);
    private NetworkVariable<float> z = new NetworkVariable<float>();
    private NetworkVariable<float> x = new NetworkVariable<float>();
    private float oldZ;
    private float oldX;
    bool attacking; 
    private bool serverAttack;
    private bool walking = false;
    private bool running = false;

    private bool itemRange = false;

    GameObject collided;
    

    private void Awake()
    {
        animate = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }    
    private void Start() {
        rigidbody.position = new Vector3(Random.Range(defaultPos.x, defaultPos.y), 0, Random.Range(defaultPos.x, defaultPos.y));
        if(IsClient && IsOwner) {
            transform.position = new Vector3(Random.Range(defaultPos.x, defaultPos.y), 0, Random.Range(defaultPos.x, defaultPos.y));
            Camera.main.GetComponent<CameraMovement>().setTarget(gameObject.transform);
        }
    }

    private void Update() {
        if(IsServer) {
            UpdateServer();
        }
        if(IsClient && IsOwner) {
            UpdateClient();
        }
    }
    private void UpdateServer() {
        animate.SetBool("IsWalking", walking);
        animate.SetBool("IsRunning", running);
        rigidbody.position = new Vector3(transform.position.x + x.Value, transform.position.y, transform.position.z + z.Value);
        rigidbody.MoveRotation(rotation);
        if(serverAttack) {
            animate.SetFloat("AttackSpeed", attackspeed);
            animate.SetTrigger("Attack");
        }
    }

    public void UpdateClient() {
        //var setup
        attacking = false;
        bool isWalking = false;
        bool isRunning = false;
        float z1 = 0;
        float x1 = 0;

        //MoveChecks
        if(Input.GetKey(KeyCode.UpArrow)) {
            z1 += walkingspeed;
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            z1 -= walkingspeed;
        }
        if(Input.GetKey(KeyCode.LeftArrow)) {
            x1 -= walkingspeed;
        }
        if(Input.GetKey(KeyCode.RightArrow)) {
            x1 += walkingspeed;
        }

        if(Input.GetKey(KeyCode.LeftShift)) {
            walkingspeed = 0.12f;
            isRunning = true;
        }

        //Attack Check
        if(Input.GetKeyDown(KeyCode.Z) && !attacking) 
        {
            attacking = true;
        }

        if(Input.GetKeyDown(KeyCode.F) && itemRange) 
        {
            Debug.Log("Entered Keydown for F");
            ItemPickup tmp = collided.GetComponent<ItemPickup>();
            tmp.handlePickup();
            itemRange = false;
            GameObject hud = GameObject.Find("HUD");
            HUD var = hud.GetComponent<HUD>();
            if(collided.tag == "Item") {
                var.closeMessage();
            }
        }

        //Chop Tree
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if(Physics.Raycast(transform.position, fwd, out hit, 3)) 
        {
            if(hit.collider.tag == "Tree" && Input.GetKeyDown(KeyCode.Z)) {
                treeBehavior obj = hit.collider.gameObject.GetComponent<treeBehavior>();
                obj.treeHp = obj.treeHp - 1;
            }
        }

        //movement checker
        if(z1 != 0 || x1 != 0) {
            isWalking = true;
        } else {
            isWalking = false;
        }

        //Movement cleaing
        var movement = new Vector3(x1, 0f, z1);
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnspeed * Time.deltaTime, 0f);
        Vector3 pos = transform.position;
        
        //repeat movement filter
        if(oldZ != z1 || oldX != x1) {
            oldZ = z1;
            oldX = x1;
            UpdateServerRpc(z1, x1, isWalking);
        }

        //rotation remote procedure call
        RotateServerRpc(desiredForward);

        //attack remote procedure call
        AttackServerRpc(attacking);

        RunningServerRpc(isRunning);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    public void OnTriggerEnter(Collider other)
    {
        GameObject tmp = GameObject.Find("HUD");
        HUD var = tmp.GetComponent<HUD>(); 
        if(other.tag == "Item") {
            var.openMessage();
            itemRange = true;
        }
        collided = other.gameObject;
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    public void OnTriggerExit(Collider other)
    {
        GameObject tmp = GameObject.Find("HUD");
        HUD var = tmp.GetComponent<HUD>(); 
        if(other.tag == "Item") {
            var.closeMessage();
            itemRange = false;
        }
    }

    [ServerRpc]
    public void UpdateServerRpc(float z1, float x1, bool isWalking) {
        x.Value = x1;
        z.Value = z1;
        walking = isWalking;
    }

    [ServerRpc]
    public void RotateServerRpc(Vector3 desiredForward) {
        rotation = Quaternion.LookRotation(desiredForward);
    }

    [ServerRpc]
    public void AttackServerRpc(bool attack) {
        serverAttack = attack;
    }

    [ServerRpc]
    public void RunningServerRpc(bool run) {
        running = run; 
    }


    public void separator() { Debug.Log("Code Line separator for 名刺"); }
}