using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reprensents the "Battle against Wizard" MiniGame 
/// </summary>
public class BWMiniGame : MiniGame
{
    [Header("Battle against Wizard")]
    [SerializeField] private float timeToSurvive = 60;
    [SerializeField] private float rockSpawnCooldown;
    [SerializeField] private float playerSpeed;
    [SerializeField] private Rigidbody player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private BWRock prefabRock;
    [SerializeField] private Image timerFill;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private Vector2 arenaSize;
    [SerializeField] private AudioSource damageSource;
    private float lastRockSpawn;
    private float startTime;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        lastRockSpawn = Time.time;
        startTime = Time.time;
    }

    public void TakeDamage()
    {
        startTime = Time.time;
        if (damageSource.isPlaying) damageSource.Stop();
        damageSource.Play();
        damageAnimator.SetTrigger("Blood");
    }

    protected override void MiniGameUpdate()
    {
        float fillAmount = 1f - Mathf.Clamp((Time.time - startTime) / timeToSurvive, 0f, 1f);
        timerFill.fillAmount = fillAmount;

        if (fillAmount == 0f)
        {
            EndMiniGame();
            return;
        }


        if (Time.time - lastRockSpawn >= rockSpawnCooldown)
        {
            lastRockSpawn = Time.time;

            int side = Random.Range(0, 4); // Left - Right - Up - Down
            Vector3 direction = GetDirection(side);

            for (int i = 0; i < 3; i++)
            {
                Instantiate(prefabRock, GetRandomPosition(side), Quaternion.identity, transform).Init(direction);
            }
        }


        Vector3 moveVector = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        player.velocity = moveVector * playerSpeed;

        if (moveVector != Vector3.zero)
        {
            player.transform.forward = moveVector;
        }

        playerAnimator.SetFloat("Speed", moveVector.normalized.magnitude);

    }

    Vector3 GetDirection(int side)
    {
        switch (side)
        {
            case 0:
                return Vector3.right;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.forward;
        }
        return Vector3.zero;
    }


    Vector3 GetRandomPosition(int side)
    {
        switch (side)
        {
            case 0:
                return new Vector3(-55, 1.5f, Random.Range(-arenaSize.y, arenaSize.y));
            case 1:
                return new Vector3(60, 1.5f, Random.Range(-arenaSize.y, arenaSize.y));
            case 2:
                return new Vector3(Random.Range(-arenaSize.y, arenaSize.y), 1.5f, 30);
            case 3:
                return new Vector3(Random.Range(-arenaSize.y, arenaSize.y), 1.5f, -36);
        }
        return Vector3.zero;
    }
}
