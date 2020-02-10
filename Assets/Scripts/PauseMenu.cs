using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	
	/*[SerializeField]
	string mouseHoverSound = "ButtonHover";

	[SerializeField]
	string buttonPressSound = "ButtonPress";*/

	public static bool GameIsPaused = false;


	public GameObject pauseMenuUI;
	public GameObject gameOverMenuUI;

	void Start()
	{
		GameIsPaused = false;
		/*audioManager = AudioManager.instance;
		if (audioManager == null) {
			Debug.LogError ("NoAudioManager");
		}*/
	}
	// Update is called once per frame
	void Update () {
		if (!gameOverMenuUI.activeInHierarchy) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (GameIsPaused) {
					Resume ();
				} else {
					Pause ();
				}
			}
		}
	}

	public void Resume(){
		GameIsPaused = false;
		Time.timeScale = 1f;	

		pauseMenuUI.SetActive (false);
	}

	void Pause(){
		GameIsPaused = true;
		Time.timeScale = 0f;
		pauseMenuUI.SetActive (true);
	}

	public void LoadMenu(){
		//audioManager.PlaySound (buttonPressSound);

		Time.timeScale = 1f;
		SceneManager.LoadScene (0);
	}

	public void Retry ()
	{
		//audioManager.PlaySound (buttonPressSound);

		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnMouseOver(){
		//audioManager.PlaySound (mouseHoverSound);
	}

}	
