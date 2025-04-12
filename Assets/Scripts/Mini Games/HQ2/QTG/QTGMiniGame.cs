using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the "Quick Time Guard" Minigame"
/// </summary>
public class QTGMiniGame : MiniGame
{
    [Header("Quick Time Guard")]
    [SerializeField] private float timeToClick;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Animator animator;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        animator.SetTrigger("Start");
    }

    protected override void MiniGameUpdate()
    {
        timeToClick -= Time.deltaTime;
        if (timeToClick <= 0) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

        targetTransform.LookAt(Camera.main.transform);

        if (Input.GetMouseButtonDown(0))
        {
            // Check for target
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 50f, mask))
            {
                EndMiniGame();
            }
        }
    }
}
