using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EmenyMove : MonoBehaviour {


    private Transform player;
    private Animator anim;
    private NavMeshAgent nav;
    private EnemyHealth enemyHealth;


   

    // Use this for initialization
    void Start () {

        player = GameManager.instance.Player.transform;
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

	}

    // Update is called once per frame
    void Update() {

        if (GameManager.instance.GameOver == false && enemyHealth.IsAlive)
        {
            nav.SetDestination(player.position);
        }
        else
            if((GameManager.instance.GameOver == false || GameManager.instance.GameOver == true) && enemyHealth.IsAlive==false)
        {
            nav.enabled = false;
            anim.Play("Die");
        }
        else
        {
            nav.enabled = false;
            anim.Play("Idle");
        }

	}
}
