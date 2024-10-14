using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Represents the "Boss Battle" Minigame
/// </summary>
public class BBMiniGame : MiniGame
{
    [Header("Player")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private int maxPlayerHP;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private int averagePlayerDamage;
    [SerializeField] private Image playerBarFill;
    [SerializeField] private AudioClip playerAttackSound;
    private int currentPlayerHP;
    private bool playerGuarding;

    [Header("Ennemy")]
    [SerializeField] private Animator ennemyAnimator;
    [SerializeField] private int maxEnemyHP;
    [SerializeField] private int averageEnnemyDamage;
    [SerializeField] private Transform ennemyCamera;
    [SerializeField] private ParticleSystem cannon;
    [SerializeField] private Image ennemyBarFill;
    [SerializeField] private AudioClip ennemyAttackSound;
    private int currentEnnemyHP;
    private bool playerTurn;
    private bool endTurn;
    private Coroutine routineBattle;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        currentPlayerHP = maxPlayerHP;
        currentEnnemyHP = maxEnemyHP;
        playerTurn = true;
        playerGuarding = false;
        endTurn = false;
        RefreshHealthBars();
        routineBattle = StartCoroutine(Routine_ProcessBattle());
    }

    public override void EndMiniGame()
    {
        StopCoroutine(routineBattle);
        base.EndMiniGame();
    }

    public void Event_Attack()
    {
        StartCoroutine(Routine_Attack());
    }

    public void Event_Guard()
    {
        StartCoroutine(Routine_Guard());
    }

    private int Add(int baseVlue, int add, int min, int max)
    {
        return Mathf.Clamp(baseVlue + add, min, max);
    }

    private void RefreshHealthBars()
    {
        playerBarFill.fillAmount = currentPlayerHP / (float)maxPlayerHP;
        ennemyBarFill.fillAmount = currentEnnemyHP / (float)maxEnemyHP;
    }

    IEnumerator Routine_Guard()
    {
        HUD.SetActive(false);
        playerGuarding = true;
        playerAnimator.CrossFade("Guard", 0.1f);
        currentPlayerHP = Add(currentPlayerHP, 40 + Random.Range(-10, 10), 0, maxPlayerHP);
        RefreshHealthBars();
        yield return new WaitForSeconds(1f);
        endTurn = true;
    }

    IEnumerator Routine_Attack()
    {
        HUD.SetActive(false);
        playerAnimator.CrossFade("Attack", 0.1f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX(playerAttackSound);
        currentEnnemyHP = Add(currentEnnemyHP, -(averagePlayerDamage + Random.Range(-10, 10)), 0, maxEnemyHP);
        RefreshHealthBars();
        yield return new WaitForSeconds(0.5f);
        endTurn = true;
    }


    IEnumerator Routine_ProcessBattle()
    {
        while (currentPlayerHP > 0 && currentEnnemyHP > 0)
        {
            Transform position = playerTurn ? playerCamera.transform : ennemyCamera.transform;
            Camera.main.transform.position = position.position;
            Camera.main.transform.rotation = position.rotation;
            HUD.SetActive(playerTurn);

            if (playerTurn)
            {
                playerGuarding = false;
                playerAnimator.CrossFade("Idle", 0.1f);

                while (!endTurn)
                {
                    yield return new WaitForEndOfFrame();
                }

                endTurn = false;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                // AI
                AudioManager.instance.PlaySFX(ennemyAttackSound);
                if (cannon) cannon.Play();
                ennemyAnimator.CrossFade("Attack", 0.1f);
                currentPlayerHP = Add(currentPlayerHP, -((averageEnnemyDamage + Random.Range(-10, 10)) / (playerGuarding ? 2 : 1)), 0, maxPlayerHP);
                RefreshHealthBars();
                yield return new WaitForSeconds(1f);
            }

            playerTurn = !playerTurn;
        }

        if (currentEnnemyHP == 0)
        {
            EndMiniGame();
        }
        else if (currentPlayerHP == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
