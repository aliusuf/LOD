using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] GameObject hero;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject ranger;

    private Animator heroAnim;
    private Animator soldierAnim;
    private Animator tankerAnim;
    private Animator rangerAnim;

    // Use this for initialization
    void Start () {

        heroAnim = hero.GetComponent<Animator>();
        soldierAnim = soldier.GetComponent<Animator>();
        tankerAnim = tanker.GetComponent<Animator>();
        rangerAnim = ranger.GetComponent<Animator>();

        StartCoroutine (showCase());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator showCase()
    {
        yield return new WaitForSeconds(1f);
        heroAnim.Play("SpinAttack");
        yield return new WaitForSeconds(1f);
        soldierAnim.Play("Attack");
        yield return new WaitForSeconds(1f);
        tankerAnim.Play("Attack");
        yield return new WaitForSeconds(1f);
        rangerAnim.Play("Attack");

        yield return new WaitForSeconds(1f);

        StartCoroutine(showCase());
    }

    public void battle()
    {
        SceneManager.LoadScene("LODS");
    }

    public void quit()
    {
        Application.Quit();
    }

}
