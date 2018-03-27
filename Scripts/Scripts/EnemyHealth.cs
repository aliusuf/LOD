using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : MonoBehaviour {

    [SerializeField] private int startingHealth = 20;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float dissapperarSpeed = 2f;

    private AudioSource audio;
    private Animator anim;
    private float timer;
    private NavMeshAgent nav; 
    private bool isAlive;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private bool dissappearEnemy = false;
    private int currentHealth;
    private ParticleSystem blood;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

	// Use this for initialization
	void Start () {

        GameManager.instance.RegisterEnemy(this);
        blood =  GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        isAlive = true;
        currentHealth = startingHealth; 



	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (dissappearEnemy == true)
        {
            transform.Translate(-Vector3.up * dissapperarSpeed * Time.deltaTime);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(timer >= timeSinceLastHit && GameManager.instance.GameOver == false)
        {
            if(other.tag == "PlayerWeapon")
            {
                takeHit();
                timer = 0f;
            }
        }
    }

     void takeHit()
    {
        if(currentHealth>=0) //and game is running
        {
            audio.PlayOneShot(audio.clip);
            anim.Play("Hurt");
           // GetComponent<EnemyAttack>().EnemyEndAttack();  // Stop the enemy being able to hit anything.
            currentHealth -= 10;
            blood.Play();
            
        }
        if(currentHealth<= 0)
        {
            isAlive = false;
            killEnemy();
        }
    }

    void killEnemy()
    {
        //GetComponent<EnemyAttack>().EnemyEndAttack();  // Stop the enemy being able to hit anything.
        anim.SetTrigger("EnemyDie");
        nav.enabled = false;
        capsuleCollider.enabled = false;
        rigidBody.isKinematic = true;
        blood.Play();
        GameManager.instance.KilledEnemy(this);

        StartCoroutine(removeEnemy());
    }

            
    IEnumerator removeEnemy ()
    {
        yield return new WaitForSeconds(4f);
        dissappearEnemy = true;
        yield return new WaitForSeconds(2f);

        Destroy(gameObject); // enemy itself
    }

}
