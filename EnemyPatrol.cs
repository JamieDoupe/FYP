using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol points")]
    [SerializeField] Transform LeftEdge;
    [SerializeField] Transform RightEdge;

    [Header("Monster")]
    [SerializeField] private Transform Monster;
    [SerializeField] private float MonsterSpeed;
    private Vector3 initScale;
    private bool movingleft;
    [SerializeField] private Animator PatrolAnim;
    [SerializeField] private float IdleDuration;
    private float IdleTimer;

    private void Awake()
    {
        initScale = Monster.localScale;
    }

    private void Update()
    {
        // The Enemy keeps moving left until they reach the left edge which causes them to change right
        if (movingleft)
        {
            if (Monster.position.x >= LeftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                ChangeDirection();
            }
        }

        // The Enemy keeps moving right until they reach the right edge which causes them to change left
        else
        {
            if (Monster.position.x <= RightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ChangeDirection();
            }
        }
    }

    // Called when an Enemy reaches an edge, they wait for several seconds idly before flipping directions
    private void ChangeDirection()
    {
        PatrolAnim.SetBool("Moving", false);

        IdleTimer += Time.deltaTime; 

        if(IdleTimer > IdleDuration)
        movingleft = !movingleft;
    }

    private void OnDisable()
    {
        PatrolAnim.SetBool("Moving", false);
    }

    //Moves the Enemy in the selected direction
    private void MoveInDirection(int direction)
    {
        IdleTimer = 0;
        PatrolAnim.SetBool("Moving", true);

        Monster.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, 
        initScale.y, initScale.z);

        Monster.position = new Vector3(Monster.position.x + Time.deltaTime * direction * MonsterSpeed,
        Monster.position.y, Monster.position.z);
    }
}
