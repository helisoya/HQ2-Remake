using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the "Just Dance Harold" MiniGame
/// </summary>
public class JDHMiniGame : MiniGame
{
    [Header("Just Dance Harold")]
    [Header("Game")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int targetScore = 100;
    [SerializeField] private Animator haroldAnimator;
    [SerializeField] private RectTransform prefabArrow;
    [SerializeField] private Transform arrowsParent;
    [SerializeField] private float arrowSpeed = 50f;
    [SerializeField] private float arrowSpawnCooldown = 5f;
    private float lastSpawn;
    private List<ArrowData> datas;
    private int currentScore = 0;

    [Header("Music")]
    [SerializeField] private double loopStartTime;
    [SerializeField] private double loopEndTime;
    [SerializeField] AudioSource musicSource;
    private int loopStartSamples;
    private int loopEndSamples;
    private int loopLengthSamples;


    public override void StartMiniGame()
    {
        base.StartMiniGame();

        RefreshScoreText();
        datas = new List<ArrowData>();

        loopStartSamples = (int)(loopStartTime * musicSource.clip.frequency);
        loopEndSamples = (int)(loopEndTime * musicSource.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;
        musicSource.Play();
    }

    public override void EndMiniGame(bool wonMiniGame = true)
    {
        musicSource.Stop();
        base.EndMiniGame(wonMiniGame);
    }

    /// <summary>
    /// Add score to the player
    /// </summary>
    /// <param name="amount">The amount of score to add</param>
    public void AddScore(int amount)
    {
        currentScore = Mathf.Clamp(currentScore + amount, 0, targetScore);
        RefreshScoreText();
        if (currentScore == targetScore) EndMiniGame();
    }

    /// <summary>
    /// Refreshs the score's text
    /// </summary>
    public void RefreshScoreText()
    {
        scoreText.text = "Score : " + currentScore + "/" + targetScore;
    }

    protected override void MiniGameUpdate()
    {
        if (musicSource.timeSamples >= loopEndSamples) { musicSource.timeSamples -= loopLengthSamples; }

        if (Time.time - lastSpawn >= arrowSpawnCooldown)
        {
            lastSpawn = Time.time;
            RectTransform rect = Instantiate(prefabArrow, arrowsParent);
            rect.anchoredPosition = new Vector2(450, 0);

            int selected = Random.Range(0, 4);
            KeyCode keyCode = KeyCode.LeftArrow;
            if (selected == 0) keyCode = KeyCode.LeftArrow;
            else if (selected == 1) keyCode = KeyCode.RightArrow;
            else if (selected == 2) keyCode = KeyCode.UpArrow;
            else if (selected == 3) keyCode = KeyCode.DownArrow;
            datas.Add(new ArrowData(keyCode, rect));
        }

        int i = 0;
        while (i < datas.Count)
        {
            bool isDestroyed = false;

            if (datas[i].transform.anchoredPosition.x <= 50 && Input.GetKeyDown(datas[i].associatedKeycode))
            {
                isDestroyed = true;
                datas[i].animator.SetTrigger("Good");
                AddScore(5);
            }

            if (!isDestroyed)
            {
                if (datas[i].transform.anchoredPosition.x > 0)
                {
                    datas[i].transform.anchoredPosition += Vector2.left * Time.deltaTime * arrowSpeed;
                }
                else
                {
                    datas[i].lifeTimeLeft -= Time.deltaTime;
                    if (datas[i].lifeTimeLeft <= 0)
                    {
                        isDestroyed = true;
                        datas[i].animator.SetTrigger("Bad");
                        AddScore(-5);
                    }
                }
            }


            if (!isDestroyed) i++;
            else
            {
                string triggerName = "";
                if (datas[i].associatedKeycode == KeyCode.LeftArrow) triggerName = "Left";
                else if (datas[i].associatedKeycode == KeyCode.RightArrow) triggerName = "Right";
                else if (datas[i].associatedKeycode == KeyCode.UpArrow) triggerName = "Up";
                else if (datas[i].associatedKeycode == KeyCode.DownArrow) triggerName = "Down";

                haroldAnimator.SetTrigger(triggerName);
                Destroy(datas[i].transform.gameObject, 2);
                datas.RemoveAt(i);
            }
        }
    }


    private class ArrowData
    {
        public Animator animator;
        public KeyCode associatedKeycode;
        public RectTransform transform;
        public float lifeTimeLeft;

        public ArrowData(KeyCode code, RectTransform transform)
        {
            lifeTimeLeft = 1f;
            associatedKeycode = code;
            this.transform = transform;
            animator = transform.GetComponent<Animator>();

            float rotation = 0;
            if (code == KeyCode.LeftArrow) rotation = 180;
            if (code == KeyCode.UpArrow) rotation = 90;
            if (code == KeyCode.DownArrow) rotation = -90;

            transform.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
