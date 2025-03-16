using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the Waldo Quiz Minigame
/// </summary>
public class WQMinigame : MiniGame
{
    [System.Serializable]
    public class Question
    {
        public string questionId;
        public string[] anwsersIDs;
        public int correctIdx;
    }

    [Header("Waldo Quiz")]
    [SerializeField] private int maxQuestions;
    [SerializeField] private Question[] questions;
    [SerializeField] private LocalizedText questionText;
    [SerializeField] private LocalizedText[] anwsersTexts;
    [SerializeField] private Animator wrongAnimator;
    private List<Question> currentPool;
    private Question currentQuestion;

    protected override void OnStart()
    {
        currentPool = new List<Question>(questions);
        PoolNewQuestion();
    }

    /// <summary>
    /// Pools a new question
    /// </summary>
    public void PoolNewQuestion()
    {
        if (maxQuestions == 0)
        {
            EndMiniGame();
        }
        else
        {
            maxQuestions--;
            int idx = Random.Range(0, currentPool.Count);
            currentQuestion = currentPool[idx];
            currentPool.RemoveAt(idx);

            questionText.SetNewKey(currentQuestion.questionId);
            for (int i = 0; i < anwsersTexts.Length; i++)
            {
                anwsersTexts[i].SetNewKey(currentQuestion.anwsersIDs[i]);
            }
        }
    }

    /// <summary>
    /// Select an anwser
    /// </summary>
    /// <param name="id">The anwser's id</param>
    public void SelectAnwser(int id)
    {
        if (currentQuestion != null && currentQuestion.correctIdx == id)
        {
            PoolNewQuestion();
        }
        else
        {
            wrongAnimator.SetTrigger("Blood");
        }
    }
}
