using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the "Shooting the Salesman" MiniGame
/// </summary>
public class STSMiniGame : MiniGame
{
    [Header("Shoot the Salesman")]
    [SerializeField] private Transform targetRoot;
    [SerializeField] private float targetRotationSpeed;
    [SerializeField] private float targetHorizontalSpeed;
    [SerializeField] private float throwCooldown;
    [SerializeField] private GameObject prefabKnife;
    [SerializeField] private int pointTarget = 300;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Animator bloodAnimator;
    private int currentPoints = 0;

    private float side = 1;
    private float rotAngle = 0;
    private float lastThrow;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        AddPoints(0);
    }

    /// <summary>
    /// Add points
    /// </summary>
    /// <param name="amount">The amount of points to add</param>
    public void AddPoints(int amount)
    {
        currentPoints = Mathf.Clamp(currentPoints + amount, 0, pointTarget);
        pointsText.text = "Points : " + currentPoints + "/" + pointTarget;
        if (amount < 0) bloodAnimator.SetTrigger("Blood");
        if (currentPoints == pointTarget)
        {
            EndMiniGame();
        }
    }

    /// <summary>
    /// Parent an object to the target
    /// </summary>
    /// <param name="obj">The object</param>
    public void ParentToTarget(Transform obj)
    {
        obj.SetParent(targetRoot);
    }

    protected override void MiniGameUpdate()
    {
        rotAngle += Time.deltaTime * targetRotationSpeed;
        if (rotAngle >= 360f) rotAngle -= 360f;
        targetRoot.localRotation = Quaternion.Euler(new Vector3(0, rotAngle, 0));

        targetRoot.localPosition += Vector3.forward * side * targetHorizontalSpeed * Time.deltaTime;
        if ((side == 1f && targetRoot.localPosition.z >= 13f) ||
            (side == -1f && targetRoot.localPosition.z <= 7f))
        {
            side *= -1f;
        }

        if (Time.time - lastThrow >= throwCooldown && Input.GetMouseButtonDown(0))
        {
            lastThrow = Time.time;
            GameObject obj = Instantiate(prefabKnife, Camera.main.transform.position, Quaternion.identity);
            obj.transform.up = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y)).direction;
        }
    }
}
