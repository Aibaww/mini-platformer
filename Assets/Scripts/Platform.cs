using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform ptA;
    public Transform ptB;
    public float speed = 2f;

    private Vector3 targetPos;

    void Start() {
        targetPos = ptB.position;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 0.1f) {
            targetPos = (targetPos == ptA.position)? ptB.position : ptA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.SetParent(null);
        }
    }
}
