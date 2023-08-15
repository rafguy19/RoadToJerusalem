using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //sounds
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem blood;
    private HealthSystem health;
    [SerializeField]
    private int Starting_Health;
    [SerializeField]
    private GameEvent gameEvent;
    [SerializeField]
    private CharacterController player;

    void Awake()
    {
        health = new HealthSystem(Starting_Health);
    }


    public void takeDamage(int dmg)
    {
        //reduce its health 
        health.Damage(dmg);
        //play dead animation when health is 0
        if (health.GetHealth() == 0)
        {
            animator.SetBool("isDead", true);
            gameEvent.killedGolblin();
            this.GetComponent<CapsuleCollider2D>().enabled = false;
        }
        audioSource.PlayOneShot(hitSound);
        //play hurt animation
        animator.SetTrigger("Hurt");
        blood.Play();


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.healthSystem.Damage(10);
    }

}
