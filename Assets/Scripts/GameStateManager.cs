using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

    public GameObject gameplayUI, endUI, menuUI;
    public Text currentLivesText, currentScoreText;
    public AudioSource runningAudio;

    public Personaje personajePrincipal;
    public ScoreManager scoreManager;

    private bool juegoTerminado = false;

    void Start()
    {
        //Changes the time scale back to 1 in case the player came from the in-game pause menu
        Time.timeScale = 1;
        
        //Activates on the gameplay interface
        gameplayUI.SetActive(true);
        endUI.SetActive(false);
        menuUI.SetActive(false);
    }

	void Update () {
        if (!juegoTerminado)
        {
            if (personajePrincipal.EstaMuerto()) AcabarJuego();

        }

        //Opens the in-game pause menu if the player presses ESCAPE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Sets time scale to 0 to stop the game
            Time.timeScale = 0;

            //Hides the gameplay interface and shows the pause menu
            menuUI.SetActive(true);
            gameplayUI.SetActive(false);
            //Shows the current stats of the run in the menu
            currentLivesText.text = "Current Lives\n" + personajePrincipal.vidaActual;
            currentScoreText.text = "Current Score\n" + scoreManager.totalScore;
            //Pauses the running audio
            runningAudio.Pause();
        }
    }

    //Continues the current run
    public void ContinueGame()
    {
        //Hides the pause menu and shows the gameplay interface
        menuUI.SetActive(false);
        gameplayUI.SetActive(true);
        //Sets the time scale back to 1 to continue the game
        Time.timeScale = 1;
        //Starts playing the running audio again
        runningAudio.Play();
    }


    void AcabarJuego()
    {
        juegoTerminado = true;
        personajePrincipal.DetenerPersonaje();
        //Stops the running audio
        runningAudio.Stop();
        //Updates the total coin amount by adding the coin amount the player got during the run
        PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + personajePrincipal.currentCoinAmount);
        scoreManager.AcabarJuego();


        StartCoroutine(DelayEnseñarInterfaz());
    }

    [ContextMenu("Reiniciar el juego")]
    public void ReiniciarJuego()
    {
        //Updates the total coin amount by adding the coin amount the player got during the run
        PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + personajePrincipal.currentCoinAmount);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        //Updates the total coin amount by adding the coin amount the player got during the run
        PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + personajePrincipal.currentCoinAmount);
        SceneManager.LoadScene("Menu");
    }


    IEnumerator DelayEnseñarInterfaz()
    {
        yield return new WaitForSeconds(1.5f);
     
        gameplayUI.SetActive(false);
        endUI.SetActive(true);
    }
}
