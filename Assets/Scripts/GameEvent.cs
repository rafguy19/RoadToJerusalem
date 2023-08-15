using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    private int goblinsKilled;
    [SerializeField]
    private Animation winAnimation;
    [SerializeField]
    private AudioClip winSound;
    [SerializeField]
    private AudioSource playerAudio;
    [SerializeField]
    private GameObject winButtons;
    [SerializeField]
    private ParticleSystem confetti;
    [SerializeField]
    private TMP_Text objectives;

    [SerializeField]
    private int goblins_to_kill;
    public bool Won;
    void Awake()
    {
        Won = false;
        updateObjectives();
    }
    void FixedUpdate()
    {
 
        //win
        if (goblinsKilled == goblins_to_kill && Won==false)
        {
            Won = true;
            winAnimation.Play();

            playerAudio.PlayOneShot(winSound);
            confetti.Play();
        }
        //enable win screen
        if (Won == true)
        {
            winButtons.SetActive(true);
            
        }
        else
        {
            winButtons.SetActive(false);
        }
    }
    public void killedGolblin()
    {
        goblinsKilled++;
        updateObjectives();
    }
    private void updateObjectives()
    {
        string goblinsLeft = "Goblins left: " + goblinsKilled +"/"+ goblins_to_kill;

        objectives.text = goblinsLeft;
    }
}
