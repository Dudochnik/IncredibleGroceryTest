using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public float speedAppearence;
    public AudioClip productSelectSound;
    public Button sellButton;
    public GameManager gameManager;
    [HideInInspector]
    public int groceriesCount;

    SoundManager soundManager;
    public bool IsStorageActive { get; private set; }
    public bool IsStorageVisible { get; private set; }
    List<GroceryImageLink> selectedGroceries = new List<GroceryImageLink>();
    RectTransform storagePanel;


    private void Start()
    {
        soundManager = gameManager.soundManager;
        storagePanel = GetComponent<RectTransform>();
    }
    public void ClickGrocery(Image grocery)
    {
        if (!IsStorageActive)
            return;


        foreach (GroceryImageLink item in gameManager.groceries)
        {
            if (item.groceryUIImage == grocery)
            {
                soundManager.PlaySoundOnce(productSelectSound);
                SetGrocery(item);
                break;
            }
        }

    }

    public void SetGrocery(GroceryImageLink grocery)
    {
        Image imageCheckmark = grocery.groceryUIImage.transform.GetChild(0).GetComponent<Image>();
        if (selectedGroceries.Contains(grocery))
        {
            selectedGroceries.Remove(grocery);
            imageCheckmark.enabled = false;
            grocery.groceryUIImage.color = new Color(grocery.groceryUIImage.color.r, grocery.groceryUIImage.color.g, grocery.groceryUIImage.color.b, 1f);
        }
        else
        {
            if (selectedGroceries.Count < groceriesCount)
            {
                selectedGroceries.Add(grocery);
                imageCheckmark.enabled = true;
                grocery.groceryUIImage.color = new Color(grocery.groceryUIImage.color.r, grocery.groceryUIImage.color.g, grocery.groceryUIImage.color.b, 0.3f);
            }
        }

        var colors = sellButton.colors;
        if (selectedGroceries.Count == groceriesCount)
        {
            sellButton.interactable = true;
            colors.normalColor = new Color(1, 1, 1, 1);
        }
        else
        {
            sellButton.interactable = false;
            colors.normalColor = new Color(1, 1, 1, 0.5f);
        }
        sellButton.colors = colors;
    }

    public IEnumerator OpenStorage()
    {
        IsStorageVisible = true;
        if (speedAppearence <= 0)
            speedAppearence = 1f;
        while (storagePanel.anchoredPosition.x >= -storagePanel.rect.width / 2)
        {
            storagePanel.anchoredPosition = new Vector3(storagePanel.anchoredPosition.x - speedAppearence, storagePanel.anchoredPosition.y, 0);
            yield return new WaitForSeconds(0);
        }
        IsStorageActive = true;
    }

    public IEnumerator CloseStorage()
    {
        IsStorageActive = false;
        if (speedAppearence <= 0)
            speedAppearence = 1f;
        while (storagePanel.anchoredPosition.x <= storagePanel.rect.width / 2)
        {
            storagePanel.anchoredPosition = new Vector3(storagePanel.anchoredPosition.x + speedAppearence, storagePanel.anchoredPosition.y, 0);
            yield return new WaitForSeconds(0);
        }
        IsStorageVisible = false;
    }

    public List<GroceryImageLink> GetSelectedGroceries()
    {
        return selectedGroceries;
    }

    public void ClearSelectedGroceries()
    {
        int numberOfIteratons = selectedGroceries.Count;
        for (int j = 0; j < numberOfIteratons; j++)
        {
            SetGrocery(selectedGroceries[0]);
        }
        selectedGroceries.Clear();
    }
}
