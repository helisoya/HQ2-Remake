using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the player in the horse race Minigame
/// </summary>
public class HRPlayer : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float speedGain = 1f;
    [SerializeField] private float stunTime = 1f;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float endAfterZ = 552f;

    [Header("Components")]
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator bloodAnimator;



    private float currentAdditiveSpeed = 0;
    private KeyCode requiredKey = KeyCode.LeftArrow;

    private float stunStart = 0f;
    private bool waitForStun = false;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundChecker.position, groundChecker.position + Vector3.down * 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!MiniGame.instance.started || MiniGame.instance.exiting) return;

        if (!waitForStun)
        {
            if (Physics.Raycast(groundChecker.position, Vector3.down, 0.25f))
            {
                if (Input.GetKeyDown(requiredKey))
                {
                    if (requiredKey == KeyCode.LeftArrow) requiredKey = KeyCode.RightArrow;
                    else requiredKey = KeyCode.LeftArrow;
                    currentAdditiveSpeed = Mathf.Clamp(currentAdditiveSpeed + speedGain, minSpeed, maxSpeed);
                }
                else
                {
                    currentAdditiveSpeed = Mathf.Clamp(currentAdditiveSpeed - 5f * Time.deltaTime, minSpeed, maxSpeed);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerAnimator.SetTrigger("Jump");
                    player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }

        }
        else if (Time.time - stunStart >= stunTime)
        {
            waitForStun = false;
        }




        player.MovePosition(player.position + player.transform.forward * currentAdditiveSpeed * Time.deltaTime);

        if (player.position.z >= endAfterZ) MiniGame.instance.EndMiniGame();
    }


    /// <summary>
    /// Takes damage
    /// </summary>
    public void TakeDamage()
    {
        playerAnimator.SetTrigger("Damage");
        bloodAnimator.SetTrigger("Blood");
        stunStart = Time.time;
        waitForStun = true;
        currentAdditiveSpeed = Mathf.Clamp(currentAdditiveSpeed * 0.5f, minSpeed, maxSpeed);
    }


    public void OnTriggerEnter(Collider collider)
    {
        if (!waitForStun && collider.tag == "Obstacle")
        {
            TakeDamage();
        }
    }
}
