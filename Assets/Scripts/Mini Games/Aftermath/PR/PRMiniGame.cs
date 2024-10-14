using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the "Punch Rickey" MiniGame
/// </summary>
public class PRMiniGame : MiniGame
{
    [Header("Punch Rickey")]
    [SerializeField] private Animator animatorRickey;
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Animator bloodAnimator;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private int maxRickeyHealth;
    [SerializeField] private float punchAnimationLength;
    [SerializeField] private float rickeyCooldownTime;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip punchClip;
    [SerializeField] private AudioClip damageClip;

    private int currentRickeyHealth;
    private float lastPlayerAttack;
    private bool blocking;
    private float rickeyLastMove;
    private bool rickeyBlocking;
    private bool checkForRickeyAttack;


    public override void StartMiniGame()
    {
        base.StartMiniGame();
        currentRickeyHealth = maxRickeyHealth;
        RefreshHealthBar();
        blocking = false;
        rickeyBlocking = false;
        checkForRickeyAttack = false;
        rickeyLastMove = Time.time;
        playerRenderer.material.SetFloat("_Alpha", 0.5f);
    }

    /// <summary>
    /// Refreshs Rickey's health bar
    /// </summary>
    private void RefreshHealthBar()
    {
        healthBarFill.fillAmount = (float)currentRickeyHealth / maxRickeyHealth;
    }

    /// <summary>
    /// Adds health to Rickey
    /// </summary>
    /// <param name="amount">The amount to add</param>
    private void AddHealth(int amount)
    {
        currentRickeyHealth = Mathf.Clamp(currentRickeyHealth + amount, 0, maxRickeyHealth);
        RefreshHealthBar();

        if (currentRickeyHealth == 0)
        {
            EndMiniGame();
        }
    }

    protected override void MiniGameUpdate()
    {
        base.MiniGameUpdate();

        // Player Input
        if (Time.time - lastPlayerAttack >= punchAnimationLength)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Punch
                blocking = false;
                animatorPlayer.SetBool("Block", false);

                animatorPlayer.SetTrigger("Attack");
                lastPlayerAttack = Time.time;

                audioSource.PlayOneShot(punchClip);

                if (!rickeyBlocking)
                {
                    AddHealth(-1);
                }
            }
            else
            {
                blocking = Input.GetKey(KeyCode.DownArrow);
                animatorPlayer.SetBool("Block", blocking);
            }
        }

        // Rickey AI
        if (Time.time - rickeyLastMove >= rickeyCooldownTime)
        {
            rickeyLastMove = Time.time;
            rickeyBlocking = false;

            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0: // Attack
                    audioSource.PlayOneShot(punchClip);
                    animatorRickey.SetTrigger("Attack");
                    checkForRickeyAttack = true;
                    break;
                case 1: // Block
                    rickeyBlocking = true;
                    break;
                case 2: // Nothing
                    break;
            }

            animatorRickey.SetBool("Block", rickeyBlocking);
        }
        else if (Time.time - rickeyLastMove >= 0.5f && checkForRickeyAttack)
        {
            checkForRickeyAttack = false;
            if (!blocking)
            {
                audioSource.PlayOneShot(damageClip);
                bloodAnimator.SetTrigger("Blood");
                AddHealth(1);
            }
        }

    }
}
