using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] int startingHealth = 100;
    [SerializeField] float timeSinceLastHit = 2f;
    [SerializeField] Slider slider;

    private float timer;
    private Animator anim;
    private CharacterController characterController;
    private int currentHealth;
    private AudioSource audio;
    private ParticleSystem blood;


    private int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if(value < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(slider);
   }

    // Use this for initialization
    void Start () {

        blood = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        currentHealth = startingHealth;
        audio = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

	}

    void OnTriggerEnter(Collider other)
    {
        if(timer>=timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if(other.tag == "Weapon")
            {
                takeHit();
                timer = 0;
            }
        }
    }

    void takeHit()
    {
        if(currentHealth>=0)
        {
            GameManager.instance.playerHit(currentHealth);
            anim.Play("Hurt");
            currentHealth -= 10;
            slider.value = currentHealth;
            audio.PlayOneShot(audio.clip);
            blood.Play();
        }
        if(currentHealth<= 0)
        {
            killPlayer();
        }
    }

    void killPlayer()
    {
        GameManager.instance.playerHit(currentHealth);
        anim.SetTrigger("HeroDie");
        characterController.enabled = false;
        blood.Play();
        
    }

    public void powerUpHealth()
    {
        if(currentHealth <= 70)
        {
            currentHealth += 30;
        }
        else if(currentHealth<startingHealth)
        {
            currentHealth = startingHealth;
        }

        slider.value = currentHealth;
    }

    

}


