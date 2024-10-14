using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents the "Boyardee Chef" MiniGame
/// </summary>
public class BCMiniGame : MiniGame
{
    [SerializeField] private GameObject prefabPizza;
    [SerializeField] private float sendAnimationTime;
    [SerializeField] private int pizzasToDo;
    [SerializeField] private TextMeshProUGUI textNumberDone;
    [SerializeField] private Transform elementsGUIRoot;
    public BCPizza currentPizza { get; private set; }
    private BCElements currentTarget;
    private int currentPizzaDone;
    public bool canInterract { get; set; }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        currentPizza = Instantiate(prefabPizza).GetComponent<BCPizza>();
        currentPizza.Reset();
        RandomizeTarget();
        canInterract = true;
        textNumberDone.text = "0/" + pizzasToDo;
    }

    /// <summary>
    /// Randomizes the target
    /// </summary>
    private void RandomizeTarget()
    {
        currentTarget = new BCElements
        {
            hasSalad = Random.Range(0, 2) == 0,
            hasCheese = Random.Range(0, 2) == 0,
            hasChocolate = Random.Range(0, 2) == 0,
            hasHam = Random.Range(0, 2) == 0,
            hasTomato = Random.Range(0, 2) == 0,
        };

        elementsGUIRoot.GetChild(0).gameObject.SetActive(currentTarget.hasTomato);
        elementsGUIRoot.GetChild(1).gameObject.SetActive(currentTarget.hasCheese);
        elementsGUIRoot.GetChild(2).gameObject.SetActive(currentTarget.hasSalad);
        elementsGUIRoot.GetChild(3).gameObject.SetActive(currentTarget.hasChocolate);
        elementsGUIRoot.GetChild(4).gameObject.SetActive(currentTarget.hasHam);
    }

    /// <summary>
    /// Sends the pizza
    /// </summary>
    public void SendPizza()
    {
        canInterract = false;
        StartCoroutine(Routine_SendPizza());
    }


    IEnumerator Routine_SendPizza()
    {
        currentPizza.TriggerAnimation("Exit");
        yield return new WaitForSeconds(sendAnimationTime);

        if (currentPizza.IsConform(currentTarget))
        {
            currentPizzaDone++;
            if (currentPizzaDone == pizzasToDo)
            {
                EndMiniGame();
            }
            textNumberDone.text = currentPizzaDone + "/" + pizzasToDo;
        }

        RandomizeTarget();
        currentPizza.Reset();

        yield return new WaitForSeconds(sendAnimationTime);

        canInterract = true;
    }
}

[System.Serializable]
public struct BCElements
{
    public bool hasTomato;
    public bool hasCheese;
    public bool hasSalad;
    public bool hasChocolate;
    public bool hasHam;
}