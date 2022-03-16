using System.Collections;
using System.Collections.Generic;
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
    private bool walking = false;

    private void Start() {
        animate = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        Camera.main.GetComponent<CameraMovement>().setTarget(gameObject.transform);
        rigidbody.position = new Vector3(Random.Range(defaultPos.x, defaultPos.y), 0, Random.Range(defaultPos.x, defaultPos.y));
    }

    public void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraMovement>().setTarget(gameObject.transform);
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
        rigidbody.position = new Vector3(transform.position.x + x.Value, transform.position.y, transform.position.z + z.Value);
        rigidbody.MoveRotation(rotation);
    }

    public void UpdateClient() {
        bool isWalking = false;
        float z1 = 0;
        float x1 = 0;

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
        if(Input.GetKey(KeyCode.Z) && !attacking) 
        {
            animate.SetFloat("AttackSpeed", attackspeed);
            animate.SetTrigger("Attack");
            attacking = true;
            attacktimer = attackcd;
        }
        if(attacking) {
            if(attacktimer > 0) {
                attacktimer -= Time.deltaTime;
            } else {
                attacking = false;
            } 
        }

        if(z1 != 0 || x1 != 0) {
            isWalking = true;
        } else {
            isWalking = false;
        }

        var movement = new Vector3(x1, 0f, z1);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnspeed * Time.deltaTime, 0f);

        if(oldZ != z1 || oldX != x1) {
            oldZ = z1;
            oldX = x1;
            UpdateServerRpc(z1, x1, isWalking);
        }

        RotateServerRpc(desiredForward);

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
}
