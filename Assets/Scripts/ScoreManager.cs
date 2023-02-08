using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text puntuacionGameplayText, puntuacionFinalText, finalGameState;

    private int scorePerTick = 100;

    private float nextTimeToScore;

    public int totalScore = 0;
    private bool juegoAcabado = false;
    // Use this for initialization
    void Start () {
        //Empezamos teniendo el cuenta en tiempo para que el score no empiece ya en 100
        nextTimeToScore = Time.timeSinceLevelLoad + 1;

    }
	
	// Update is called once per frame
	void Update () {

        if (!juegoAcabado)
        {
            if (Time.timeSinceLevelLoad >= nextTimeToScore)
            {
                AumentarScore();
                nextTimeToScore = Time.timeSinceLevelLoad + 1;
            }
        }
    }

    public void AcabarJuego()
    {
        juegoAcabado = true;

        int maximaPuntuacion = PlayerPrefs.GetInt("Puntuacion", 0);
        int puntuacionTotal = GetTotalScore();
        puntuacionFinalText.text = "Total Score\n " + puntuacionTotal;
     


        if (maximaPuntuacion < puntuacionTotal)
        {
            finalGameState.text = "New Highscore";
            PlayerPrefs.SetInt("Puntuacion", puntuacionTotal);
            PlayerPrefs.Save();
        } else { finalGameState.text = "Game Over"; }
    }


    void AumentarScore()
    {
        totalScore += scorePerTick;
        puntuacionGameplayText.text =  totalScore.ToString();
    }

    //Adds 500 points to the score when the player gets a coin
    public void AumentarScoreCoin()
    {
        totalScore += 500;
        puntuacionGameplayText.text = totalScore.ToString();
    }

    //Returns the total score
    public int GetTotalScore()
    {
        return totalScore;
    }
}
