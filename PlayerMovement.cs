using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 movement;
    Animator animate;
    Rigidbody mrigidbody;
    public float turnspeed = 10f;
    public float movespeed = 0.05f;
    public bool attacking = false;
    public float attackSpeed = 1f;
    Quaternion m_Rotation = Quaternion.identity;
    // Start is called before the first frame update
    void Start()
    {
        animate = GetComponent<Animator>();
        mrigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            animate.SetFloat("AttackSpeed", attackSpeed);
            animate.SetTrigger("Attack");
        }
    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();

        bool isWalkingHorizontal = !Mathf.Approximately(horizontal, 0f);
        bool isWalkingVertical = !Mathf.Approximately(vertical, 0f);
        bool isWalking = isWalkingHorizontal || isWalkingVertical;
        animate.SetBool("IsWalking", isWalking);
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnspeed * Time.deltaTime, 0f); 
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove() {
        mrigidbody.MoveRotation(m_Rotation);
        mrigidbody.MovePosition(mrigidbody.position + movement * movespeed);
    }
}
