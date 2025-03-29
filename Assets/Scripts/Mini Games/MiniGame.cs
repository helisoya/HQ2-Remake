using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// General class for Mini Games
/// </summary>
public class MiniGame : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected GameObject rulesRoot;
    [SerializeField] protected GameObject gameRoot;
    [SerializeField] protected PauseMenu pauseMenu;
    [SerializeField] protected Fade fade;
    [SerializeField] protected string nextChapter;
    [SerializeField] protected string nextChapterBad;
    [SerializeField] protected string bgmName = "MiniGame";
    protected Coroutine exitingScene;

    public bool exiting { get { return exitingScene != null; } }
    public bool started { get { return !rulesRoot.activeInHierarchy && !pauseMenu.open; } }

    public static MiniGame instance;

    void Awake()
    {
        instance = this;
        OnAwake();
    }

    void Start()
    {
        AudioManager.instance.PlaySong(null);
        fade.ForceAlphaTo(1);
        fade.FadeTo(0);
        gameRoot.SetActive(false);
        rulesRoot.SetActive(true);
        OnStart();
    }

    /// <summary>
    /// Callback on Awake()
    /// </summary>
    protected virtual void OnAwake()
    {

    }

    /// <summary>
    /// Callback on Start()
    /// </summary>
    protected virtual void OnStart()
    {

    }

    /// <summary>
    /// Starts the minigame
    /// </summary>
    public virtual void StartMiniGame()
    {
        AudioManager.instance.PlaySong(bgmName);
        rulesRoot.SetActive(false);
        gameRoot.SetActive(true);
    }

    /// <summary>
    /// Ends the minigame
    /// </summary>
    /// <param name="wonMiniGame">Did the player win ?</param>
    public virtual void EndMiniGame(bool wonMiniGame = true)
    {
        if (exiting) return;
        exitingScene = StartCoroutine(Routine_Exit(wonMiniGame));
    }

    /// <summary>
    /// Routine for exiting the game
    /// </summary>
    /// <param name="wonMiniGame">Did the player win ?</param>
    /// <returns>IEnumerator</returns>
    IEnumerator Routine_Exit(bool wonMiniGame)
    {
        GameManager.instance.SetIsLoadingSave(false);
        GameManager.instance.SetNextChapter(wonMiniGame ? nextChapter : nextChapterBad);
        AudioManager.instance.PlaySong(null);

        fade.FadeTo(1);
        yield return new WaitForEndOfFrame();

        while (fade.fading)
        {
            yield return new WaitForEndOfFrame();
        }

        instance = null;
        SceneManager.LoadScene("VN");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.open) pauseMenu.Close();
            else pauseMenu.Show();
        }

        if (!started || exiting) return;

        MiniGameUpdate();
    }

    /// <summary>
    /// This function is called on Update when the game has started
    /// </summary>
    protected virtual void MiniGameUpdate()
    {

    }
}
