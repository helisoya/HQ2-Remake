using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the "Tir sur Archer" Minigame
/// </summary>
public class TAMiniGame : MiniGame
{
    [Header("Tir sur Archer")]
    [SerializeField] private int maxHp = 10;
    [SerializeField] private float stunTime = 1;
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private Image archerHpFill;
    [SerializeField] private Rigidbody player;
    [SerializeField] private AudioClip sfxDamage;
    [SerializeField] private AudioClip sfxFire;

    [Header("Archer")]
    [SerializeField] private float archerWaitTime = 5f;
    [SerializeField] private Transform archerTransform;
    [SerializeField] private Renderer[] archerRenderers;
    [SerializeField] private LayerMask maskArcher;
    [SerializeField] private Transform spotsRoot;


    private int currentHp;
    private Vector2 cursorDirection;
    private bool stuned;
    private float stunStart;
    private bool archerIsHiding;
    private float archerCurrentAlpha = 0f;
    private float archerStartWaitTime;
    private int currentSpot;


    public override void StartMiniGame()
    {
        base.StartMiniGame();
        currentHp = maxHp;
        cursorDirection = Vector2.zero;
        stuned = false;
        archerIsHiding = true;
        archerCurrentAlpha = 0f;
        UpdateRenderers(0f);
        currentSpot = -1;
    }

    /// <summary>
    /// Deals damage to the archer
    /// </summary>
    public void DamageArcher()
    {
        currentHp = Mathf.Clamp(currentHp - 1, 0, maxHp);
        archerHpFill.fillAmount = currentHp / (float)maxHp;

        if (currentHp == 0)
        {
            EndMiniGame();
        }
        else
        {
            archerIsHiding = true;
        }
    }

    /// <summary>
    /// Deals damage to the player
    /// </summary>
    /// 
    /// <param name="showBlood">Should blood be shown ?</param>
    public void DamagePlayer(bool showBlood = true)
    {
        stuned = true;
        stunStart = Time.time;

        if (showBlood)
        {
            AudioManager.instance.PlaySFX(sfxDamage);
            damageAnimator.SetTrigger("Blood");
        }
    }

    /// <summary>
    /// Updates the renderers alpha value
    /// </summary>
    /// <param name="alpha">The new alpha value</param>
    private void UpdateRenderers(float alpha)
    {
        foreach (Renderer renderer in archerRenderers)
        {
            renderer.material.SetFloat("_Alpha", alpha);
        }
    }

    protected override void MiniGameUpdate()
    {
        // Update archer

        if (archerIsHiding)
        {
            archerCurrentAlpha = Mathf.Clamp(archerCurrentAlpha - Time.deltaTime * 5, 0f, 1f);
            UpdateRenderers(archerCurrentAlpha);
            if (archerCurrentAlpha == 0f)
            {
                archerIsHiding = false;
                archerStartWaitTime = Time.time;
                int candidate = currentSpot;
                while (candidate == currentSpot)
                {
                    candidate = Random.Range(0, spotsRoot.childCount);
                }
                currentSpot = candidate;
                archerTransform.position = spotsRoot.GetChild(currentSpot).position;
            }
        }
        else
        {
            if (archerCurrentAlpha < 1f)
            {
                archerCurrentAlpha = Mathf.Clamp(archerCurrentAlpha + Time.deltaTime * 5, 0f, 1f);
                UpdateRenderers(archerCurrentAlpha);
            }

            if (Time.time - archerStartWaitTime >= archerWaitTime)
            {
                archerIsHiding = true;
                DamagePlayer();
            }
        }


        if (stuned)
        {
            if (Time.time - stunStart >= stunTime)
            {
                stuned = false;
            }
        }
        else
        {
            // Find cursorDirection
            Vector2 mousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            cursorDirection.x = mousePosition.x <= 0.25f ? -1 : (mousePosition.x >= 0.75f ? 1 : 0);
            cursorDirection.y = mousePosition.y <= 0.25f ? -1 : (mousePosition.y >= 0.75f ? 1 : 0);

            //Shoot if possible
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(player.transform.position, player.transform.forward, 100, maskArcher) && !archerIsHiding)
                {
                    DamageArcher();
                }

                cursorDirection = Vector2.zero;
                AudioManager.instance.PlaySFX(sfxFire);
                DamagePlayer(false);
            }
        }

        player.velocity = cursorDirection * playerSpeed;
    }
}
