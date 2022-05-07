using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseActivation : MonoBehaviour
{
    public int attack = 100;
    public float speed = 20f;
    public Rigidbody2D PB;
    // Start is called before the first frame update
    void Start()
    {
        PB.velocity = transform.right * speed;
    }

    // Checks what it has collided with, if it is an enemy the enemy takes damage and the pulse is destroyed
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Monster enemy = hitInfo.GetComponent<Monster>();

        if (enemy != null)
        {
            enemy.TakeDamage(attack);
            Destroy(gameObject);
        }
    }
}
