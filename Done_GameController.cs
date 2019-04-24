using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject hardBackground;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    //public Image health;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text livesText;
    public Text hardModeText;

    private bool gameOver;
    private bool restart;
    private int score;
    public int lives;
    bool hardMode = false;

    void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        livesText.text = "";
        hardModeText.text = "Press 'Space' to activate Hard Mode";
        score = 0;
        lives = 3;
        UpdateLives();
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && hardMode == false)
            {
                hardMode = true;
                hazardCount = hazardCount * 2;
                spawnWait = spawnWait / 2;
                hardBackground.SetActive(true);
                hardModeText.text = "Press 'Space' to deactivate";
            } else if (Input.GetKeyDown(KeyCode.Space) && hardMode == true)
            {
                hardMode = false;
                hazardCount = hazardCount / 2;
                spawnWait = spawnWait * 2;
                hardBackground.SetActive(false);
                hardModeText.text = "Press 'Space' to activate Hard Mode";
        }
        //health.rectTransform
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();

        if (score >= 250)
        {
            GameOver();
            GameObject.FindGameObjectWithTag("Finish").GetComponent<AudioSource>().Play();
            gameOverText.text = "You Win!";
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateLives()
    {
        livesText.text = "Lives: " + lives;
    }

    public void SubtractLives(int newLivesValue)
    {
        lives -= newLivesValue;
        UpdateLives();
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}