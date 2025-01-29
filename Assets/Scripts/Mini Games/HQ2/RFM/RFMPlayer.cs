using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the player in the "Running From Marcel" Minigame
/// </summary>
public class RFMPlayer : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float stunTime = 1f;
    [SerializeField] private float jumpForce = 20;

    [Header("Components")]
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator bloodAnimator;

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
                player.MovePosition(new Vector3(
                    Mathf.Clamp(player.position.x + Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime, -6f, 6f),
                    player.position.y,
                    player.position.z
                ));

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
        ((RFMMiniGame)MiniGame.instance).TakeDamage();
    }


    public void OnTriggerEnter(Collider collider)
    {
        if (!waitForStun && collider.tag == "Obstacle")
        {
            TakeDamage();
        }
    }
}
