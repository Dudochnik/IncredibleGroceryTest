using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CashierScript : Human
{
    public int totalMoney = 0;
    public Text moneyText;
    public Transform positiveCheckmark;
    public Transform negativeCheckmark;
    public AudioClip moneySound;
    public Storage storage;
    List<GroceryImageLink> selectedGroceries;
    private ClientScript clientScript;


    private void Awake()
    {
    }

    protected override void Start()
    {
        base.Start();
        clientScript = gameManager.client;

        
    }

    
    IEnumerator ShowSelectedGroceries()
    {
        while (storage.IsStorageVisible)
            yield return null;

        ShowBubble();
        selectedGroceries = new List<GroceryImageLink>(storage.GetSelectedGroceries());
        List<Transform> showedItems = ShowGroceries(selectedGroceries);

        yield return new WaitForSeconds(1); 
        int i = 0;
        foreach (GroceryImageLink item in selectedGroceries)
        {
            yield return new WaitForSeconds(0.5f);
            if (clientScript.GetNeededGroceries().Contains(item))
            {
                Instantiate(positiveCheckmark, showedItems[i]);
            }
            else
            {
                Instantiate(negativeCheckmark, showedItems[i]);
            }
            i++;
        }
        yield return new WaitForSeconds(1);


        DestroyGroceries(showedItems);
        HideBubble();
        storage.ClearSelectedGroceries();

        clientScript.BuyGroceries(selectedGroceries);
        
    }


    public void SellGroceries()
    {
        StartCoroutine(storage.CloseStorage());
        StartCoroutine(ShowSelectedGroceries());
    }

    public void AddMoney(int money)
    {
        if (money <= 0)
            return;

        soundManager.PlaySoundOnce(moneySound);
        SetMoney(totalMoney + money);
        gameManager.SaveGameData();
    }


    public void SetMoney(int money)
    {
        totalMoney = money;
        moneyText.text = "$ " + totalMoney.ToString();
    }
}

