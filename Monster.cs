using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    [SerializeField] private float AttackCooldown;
    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance; 
    [SerializeField] LayerMask PlayerLayer;
    [SerializeField] BoxCollider2D _MonsterBoxCollider;
    [SerializeField] bool isBoss;
    [SerializeField] bool isFirstBoss;
    private float cooldowntimer = Mathf.Infinity;
    private bool _dead;
    private PlayableCharacterScript _PlayerDead;
    public Rigidbody2D _MonsterBody;
    public PolygonCollider2D _Monstercollider;
    public int Health = 100;
    public int WalkSpeed = 10000;
    private EnemyPatrol enemyPatrol;
    [SerializeField] private Behaviour[] components;

    private Animator monsteranimator;

    public void Awake()
    {
        monsteranimator = GetComponent<Animator>();
        _MonsterBody = GetComponent<Rigidbody2D>();
        _Monstercollider = GetComponent<PolygonCollider2D>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        //If the player is spotted, attack
        cooldowntimer += Time.deltaTime;
        if (PlayerInSight())
        {
            if (cooldowntimer >= AttackCooldown)
            {
                cooldowntimer = 0;
                monsteranimator.SetTrigger("MeleeAttack");
            }
        }
        //As long as the Enemy Patrol is active the player is not in sight
        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    // A box is placed and if the player character enters they are detected by the enemy
    private bool PlayerInSight()
    {
        RaycastHit2D hitplayer = Physics2D.BoxCast(_MonsterBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
       new Vector3(_MonsterBoxCollider.bounds.size.x * range, _MonsterBoxCollider.bounds.size.y, _MonsterBoxCollider.bounds.size.z), 
       0, Vector2.left, 0, PlayerLayer);

        if(hitplayer.collider != null)
        {
            _PlayerDead = hitplayer.transform.GetComponent<PlayableCharacterScript>();
        }
        
        return hitplayer.collider != null;
    }

    //Draws a representation of the hitbox
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_MonsterBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(_MonsterBoxCollider.bounds.size.x * range, _MonsterBoxCollider.bounds.size.y, _MonsterBoxCollider.bounds.size.z));
    }

    // Once a Collision is detected, determine if it is fatal
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(ShouldDieFromCollision(collision))
        {
            Die();
        }
    }

    // If the detected collision is from a box or a hazard it should be fatal
    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (_dead)
        return false;

        if (collision.gameObject.tag == "Hazard" || collision.gameObject.tag == "Box")
        {
            return true;
        }

        return false;
    }

    // Disables the characteristics of the enemy and if the Enemy is a boss, changes scene
    void Die()
    {
        _dead = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        _particleSystem.Play();
        Destroy(gameObject, 2);
        Destroy(_Monstercollider);
        Destroy(_MonsterBody);

        foreach(Behaviour component in components)
        {
            component.enabled = false;
        }
        if(isFirstBoss == true)
        {
            SceneManager.LoadScene("Level2");
        }
        if(isBoss == true)
        {
                SceneManager.LoadScene("WinScene");
        }
    }

    //If the player is in sight when this is called, kill the player 
    private void KillPlayer()
    {
        if (PlayerInSight())
        {
            _PlayerDead.playerdeath();
        }
    }
}
