using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private LayerMask layerMask;

    private CharacterController characterController;
    private Vector3 currentLookTarget = Vector3.zero;
    private Animator anim;
    private BoxCollider[] swordColliders;
    private GameObject fireTrail;
    private ParticleSystem fireTrailPartilce;


	// Use this for initialization
	void Start () {

        fireTrail = GameObject.FindWithTag("Fire");
        fireTrail.SetActive (false);
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        swordColliders = GetComponentsInChildren<BoxCollider>();

	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.GameOver == false)
         {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.SimpleMove(moveDirection * moveSpeed);

            if (moveDirection == Vector3.zero)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("DoubleChop");
            }
            if (Input.GetMouseButtonDown(1))
            {
                anim.Play("SpinAttack");
            }
        }

    }

    private void FixedUpdate()
    {
        if(GameManager.instance.GameOver == false)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(ray.origin, ray.direction * 500, Color.green);

            if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point != currentLookTarget)
                {
                    currentLookTarget = hit.point;
                }
                Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                Quaternion rotaion = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotaion, Time.deltaTime * 10f);

            }
        }
        
    }
    public void SpeedPowerUp()
    {
        StartCoroutine(fireTrailRoutine());
    }

    public void PlayerBeginAttack()
    {
        foreach(var PlayerWeapon in swordColliders)
        {
            PlayerWeapon.enabled = true;
        }
    }

    public void PlayerEndAttack()
    {
        foreach(var PlayerWeapon in swordColliders)
        {
            PlayerWeapon.enabled = false;
        }
    }

    IEnumerator fireTrailRoutine()
    {
        fireTrail.SetActive(true);
        moveSpeed = 12f;

        yield return new WaitForSeconds(7f);
        fireTrailPartilce = fireTrail.GetComponent<ParticleSystem>();
        var emmission = fireTrailPartilce.emission;
        emmission.enabled = false;

        yield return new WaitForSeconds(3f);
        emmission.enabled = true;

        fireTrail.SetActive(false);
        moveSpeed = 7f;
    }
}
