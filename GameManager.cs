using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int enemyCount;

    public GameObject victoryMenu;
    public GameObject defeatMenu;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (enemyCount <= 0)
        {
            EndGame(true);
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            defeatMenu.SetActive(true);
        }
    }

    public void OnEnemyKilled()
    {
        enemyCount--;
        if (enemyCount <= 0 && !gameEnded)
        {
            EndGame(true);
        }
    }
    void EndGame(bool isVictory)
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (isVictory)
        {
            if (victoryMenu != null)
                victoryMenu.SetActive(true);
        }
        else
        {
            if (defeatMenu != null)
                defeatMenu.SetActive(true);
        }

        Time.timeScale = 0f;
    }

}