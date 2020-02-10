using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public GameObject levelPassed;
    public GameManagerr gameManagerr;
    int levelToUnlock;

    DoorOpening door;
    private void Start()
    {
        door = GameObject.Find("Door").GetComponent<DoorOpening>();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (door.doorOpened)
        {            
            WinLevel();
            levelPassed.SetActive(true);
            gameManagerr.DisableMovement();            
        }            
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void WinLevel()
    {
        levelToUnlock = int.Parse(SceneManager.GetActiveScene().name) + 1;
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
