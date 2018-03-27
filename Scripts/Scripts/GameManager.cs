using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject ranger;
    [SerializeField] GameObject arrow;
    [SerializeField] Text Leveltext;
    [SerializeField] GameObject[] healthSpawnPoints;
    [SerializeField] GameObject healthPowerUp;
    [SerializeField] GameObject speedPowerUp;
    [SerializeField] Text EndGameText;
    [SerializeField] int FinalLevel = 12;
    [SerializeField] int maxPowerUps = 4;


    private bool gameOver = false;

    private int currentLevel;
    private float generatedSpawnTime = 2f;
    private float currentSpawnTime = 0f;
    private float powerUpSpwanTime = 2f;
    private float currentPowerUpSpawnTime = 0f;
    private GameObject newEnemy;
    private int PowerUps = 0;
    private GameObject newPowerup;

    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private List<EnemyHealth> killedEnemies = new List<EnemyHealth>();

    public void registerPowerUp()
    {
        PowerUps++;
    }

    public GameObject Arrow
    {
        get
        {
            return arrow;
        }
    }

    public void RegisterEnemy (EnemyHealth enemy)
    {
        enemies.Add(enemy);
    }

    public void KilledEnemy(EnemyHealth enemy)
    {
        killedEnemies.Add(enemy);
    }

    public bool GameOver
    {
        get
        {
            return gameOver;
        }
    }

    public GameObject Player
    {
        get
        {
            return player;
        }
    }


     void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
        }
        else
        if(instance!= this)
        {
            Destroy(gameObject);
        }

      //  DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {

        StartCoroutine(spawnEnemy());
        StartCoroutine(powerUpSpawn());
        currentLevel = 1;
        EndGameText.GetComponent<Text>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;

	}

    public void playerHit(int currentHP)
    {
        if(currentHP>0)
        {
            gameOver = false;
        }
        else
        {
            gameOver = true;
            StartCoroutine(EndGameRoutine("DEFEAT"));
        }
    }

    IEnumerator spawnEnemy()
    {
        if(currentSpawnTime>generatedSpawnTime)
        {
            currentSpawnTime = 0;
            if(enemies.Count < currentLevel)
            {
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];

                int randomEnemy = Random.Range(0, 3);
                if (randomNumber == 0)
                {
                    newEnemy = Instantiate(soldier) as GameObject;
                }
                else if(randomNumber == 1)
                {
                    newEnemy = Instantiate(ranger) as GameObject;
                }
                else if(randomNumber == 2)
                {
                    newEnemy = Instantiate(tanker) as GameObject;
                }

                newEnemy.transform.position = spawnLocation.transform.position;
            }

            if(killedEnemies.Count == currentLevel && currentLevel != FinalLevel)
            {
                enemies.Clear();
                killedEnemies.Clear();

                yield return new WaitForSeconds (3f);
                currentLevel++;
                Leveltext.text = "Level: " + currentLevel;
            }
            if (killedEnemies.Count ==  FinalLevel)
            {
                StartCoroutine(EndGameRoutine("VICTORY!!!"));
            }
        }

        yield return null;
        StartCoroutine(spawnEnemy());

    }


    IEnumerator EndGameRoutine(string outcome)
    {
        EndGameText.text = outcome;
        EndGameText.GetComponent<Text>().enabled = true;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("GameMenu");
    }

    IEnumerator powerUpSpawn()
    {
        if(currentPowerUpSpawnTime>powerUpSpwanTime)
        {
            currentPowerUpSpawnTime = 0;

            if (PowerUps < maxPowerUps)
            {
                int randomNumber = Random.Range(0, healthSpawnPoints.Length -1);
                GameObject SpawnLoaction = healthSpawnPoints[randomNumber];
                int randomPowerUp = Random.Range(0, 2);
                if(randomPowerUp == 0)
                {
                    newPowerup = Instantiate(healthPowerUp) as GameObject;
                }
                else if(randomPowerUp == 1)
                 {
                    newPowerup = Instantiate(speedPowerUp) as GameObject;
                 }

                newPowerup.transform.position = SpawnLoaction.transform.position;
            }
        }

        yield return null;
        StartCoroutine(powerUpSpawn());
    }
}
