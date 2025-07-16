using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenuController : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Перезагрузка сцены");
        PlayerSelection.chosenClass = "Melee";
        SceneManager.LoadScene("GameScene");
    }
    public void ReturnToMainMenu()
    {
        PlayerSelection.chosenClass = "Melee";
        SceneManager.LoadScene("MainMenu");
    }
}