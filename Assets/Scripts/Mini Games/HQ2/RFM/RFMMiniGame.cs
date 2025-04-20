using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Represents the "Running From Marcel" Minigame
/// </summary>
public class RFMMiniGame : MiniGame
{
    [Header("Running From Marcel")]
    [Header("Settings")]
    [SerializeField] private float tileSize = 89.94f;
    [SerializeField] private float scrollingSpeed = 4;
    [SerializeField] private int maxTiles = 5;
    [SerializeField] private float barFillSpeed;
    private float barFillAmount = 0;

    [Header("Components")]
    [SerializeField] private Transform tilesRoot;
    [SerializeField] private RectTransform barPlayer;
    [SerializeField] private Image barFill;
    private GameObject[] availableTiles;

    [Header("Hunter")]
    [SerializeField] private Transform scope;
    [SerializeField] private Transform scopeTarget;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float fireLengthCooldown;

    [Header("Sounds")]
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip damageClip;
    private float currentScopeCooldown;
    private int scopeState = 0;


    protected override void OnAwake()
    {
        FillTiles();
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        SetBarFill(0);

        scope.gameObject.SetActive(false);
        currentScopeCooldown = fireCooldown;
    }

    /// <summary>
    /// Fills the initial map
    /// </summary>
    private void FillTiles()
    {
        availableTiles = Resources.LoadAll<GameObject>("MiniGames/RFM/Tiles");
        Instantiate(availableTiles[0], tilesRoot).transform.position = Vector3.zero;

        for (int i = 1; i < maxTiles; i++)
        {
            Instantiate(availableTiles[Random.Range(0, availableTiles.Length)], tilesRoot).transform.position =
                new Vector3(0, 0, tileSize * i);
        }
    }

    /// <summary>
    /// Takes damage
    /// </summary>
    public void TakeDamage()
    {
        SetBarFill(Mathf.Clamp(barFillAmount - 0.1f, 0f, 1f));
        AudioManager.instance.PlaySFX(damageClip);
    }

    /// <summary>
    /// Sets the progress bar fill amount
    /// </summary>
    /// <param name="fillAmount">The new fill amount</param>
    private void SetBarFill(float fillAmount)
    {
        barFillAmount = fillAmount;
        barFill.fillAmount = barFillAmount;
        barPlayer.anchoredPosition = new Vector3(barFillAmount * 600f, 0);
    }

    protected override void MiniGameUpdate()
    {
        base.MiniGameUpdate();

        // Tiles movements

        Vector3 vector = new Vector3(0, 0, -1) * scrollingSpeed * Time.deltaTime;

        foreach (Transform child in tilesRoot)
        {
            child.position += vector;
        }

        // Deletes a tile if off screen


        if (tilesRoot.GetChild(0).position.z <= -tileSize)
        {

            Instantiate(availableTiles[Random.Range(0, availableTiles.Length)], tilesRoot).transform.position =
                new Vector3(0, 0, tilesRoot.GetChild(maxTiles - 1).position.z + tileSize);

            Destroy(tilesRoot.GetChild(0).gameObject);
        }

        if (barFillAmount == 1f)
        {
            EndMiniGame();
        }
        else
        {
            SetBarFill(Mathf.Clamp(barFillAmount + barFillSpeed * Time.deltaTime, 0f, 1f));
        }


        // Scope Handling

        switch (scopeState)
        {
            case 0:
                currentScopeCooldown -= Time.deltaTime;
                if (currentScopeCooldown <= 0f)
                {
                    scopeState = 1;
                    currentScopeCooldown = fireLengthCooldown;
                    scope.gameObject.SetActive(true);
                }
                break;
            case 1:
                currentScopeCooldown -= Time.deltaTime;

                if (currentScopeCooldown >= fireLengthCooldown / 4f)
                {
                    Vector3 direction = (scopeTarget.position - Camera.main.transform.position).normalized;
                    scope.position = Camera.main.transform.position + direction * 6;
                }
                else if (currentScopeCooldown <= 0f)
                {
                    Vector3 direction = (scope.position - Camera.main.transform.position).normalized;
                    Instantiate(bulletPrefab, Camera.main.transform.position, Quaternion.identity).up = direction;
                    AudioManager.instance.PlaySFX(fireClip);

                    scopeState = 0;
                    currentScopeCooldown = fireLengthCooldown;
                    scope.gameObject.SetActive(false);
                }

                break;
        }

    }

}
