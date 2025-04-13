using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the "Battle Rap Rickey" MiniGame
/// </summary>
public class BRRMiniGame : MiniGame
{
    [Header("Battle Rap Rickey")]
    [Header("Player")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform prefabYell;
    [SerializeField] private RectTransform yellRoot;
    [SerializeField] private int scoreTarget = 50;
    [SerializeField] private float yellCooldown = 0.5f;
    private int currentScore = 0;
    private float lastYell;


    [Header("Rickey")]
    [SerializeField] private string lyricPrefix = "BRR_";
    [SerializeField] private int lyricsAmount;
    [SerializeField] private LocalizedText lyricText;
    [SerializeField] private float lyricCooldown = 2f;
    [SerializeField] private float typeWritterSpeed = 0.05f;
    private Coroutine routineTypeWritter;
    private float lastLyric;
    private int currentLyric;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        currentLyric = -1;
        RefreshScoreText();
    }

    /// <summary>
    /// Refreshs the score text
    /// </summary>
    private void RefreshScoreText()
    {
        scoreText.text = "Score : " + currentScore + "/" + scoreTarget;
    }

    protected override void MiniGameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastYell >= yellCooldown)
        {
            lastYell = Time.time;
            currentScore++;
            RefreshScoreText();

            RectTransform obj = Instantiate(prefabYell, yellRoot);
            obj.gameObject.SetActive(true);
            obj.anchoredPosition = new Vector2(Random.Range(-350f, 350f), Random.Range(-150f, 150f));
            Destroy(obj.gameObject, 1);

            if (currentScore == scoreTarget) EndMiniGame();
        }

        if (routineTypeWritter == null && Time.time - lastLyric >= lyricCooldown)
        {
            currentLyric = (currentLyric + 1) % lyricsAmount;
            lyricText.SetNewKey(lyricPrefix + currentLyric);
            routineTypeWritter = StartCoroutine(Routine_TypeWritter());
        }
    }

    /// <summary>
    /// Routine for the typewritter on the rap lyrics 
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator Routine_TypeWritter()
    {
        TMP_Text text = lyricText.GetText();
        text.ForceMeshUpdate(false);
        text.maxVisibleCharacters = 0;
        int target = text.textInfo.characterCount;

        while (text.maxVisibleCharacters < target)
        {
            text.maxVisibleCharacters++;
            yield return new WaitForSeconds(typeWritterSpeed);
        }

        lastLyric = Time.time;
        routineTypeWritter = null;
    }
}
