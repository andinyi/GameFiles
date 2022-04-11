using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class treeBehavior : NetworkBehaviour {
    GameObject thisTree;
    public int treeHp = 5;
    private bool chopped = false;
    public GameObject log;
    public bool applyFall = false;
    public Rigidbody rb;

    private void Start() {
        thisTree = transform.gameObject;
        rb = GetComponent<Rigidbody>();
    }
    
    /*private void Update() {
        if(IsServer) {
            UpdateServer();
        }
        if(IsClient && IsOwner) {
            UpdateClient();
        }
    }*/

    /*private void UpdateServer() {
        if(treeHp <= 0 && chopped == false) {
            Rigidbody rb = thisTree.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            thisTree.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
            StartCoroutine(unload());
        }
    }*/

    private void Update() {
        if(treeHp <= 0 && chopped == false) {
            rb.isKinematic = false;
            rb.useGravity = true;
            thisTree.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
            itemDrop();
            StartCoroutine(unload());
            chopped = true;
        }
    }
    private IEnumerator unload() {
        yield return new WaitForSeconds(5);
        Destroy(thisTree);
    }

    private void itemDrop() {
        int rnd = Random.Range(3, 6);
        for(int i = 0; i <= rnd; i++) {
            Random.InitState(i);
            Vector3 offset = new Vector3(Random.Range(-3, 3), -0.4f, Random.Range(-3, 3));
            GameObject spawnedItem = Instantiate(log, transform.position + offset, Quaternion.identity);
        }
    }
}
