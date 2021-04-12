using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{

    public Transform speechBubble;
    public Transform imagesContainer;
    public Transform groceryPlaceholder;
    public AudioClip bubbleAppearedSound;
    public AudioClip bubbleDisappearedSound;
    public GameManager gameManager;
    protected SoundManager soundManager;
    // Start is called before the first frame update
    protected virtual void Start()
    { 
        soundManager = gameManager.soundManager; 
    }

    protected List<Transform> ShowGroceries(List<GroceryImageLink> groceries)
    {
        List<Transform> showedItems = new List<Transform>();
        float itemWidth = groceries[0].groceryTransform.GetComponent<SpriteRenderer>().bounds.size.x + 0.3f;
        float xPoint = -(itemWidth / 2) * (groceries.Count - 1);
        foreach (GroceryImageLink grocery in groceries)
        {
            Transform newObject = Instantiate(groceryPlaceholder, new Vector3(imagesContainer.position.x + xPoint, imagesContainer.position.y, imagesContainer.position.z), Quaternion.identity, imagesContainer);
            newObject.GetComponent<SpriteRenderer>().sprite = grocery.groceryTransform.GetComponent<SpriteRenderer>().sprite;
            showedItems.Add(newObject);
            xPoint += itemWidth;
        }

        return showedItems;
    }

    protected void DestroyGroceries(List<Transform> groceries)
    {
        foreach (Transform item in groceries)
        {
            Destroy(item.gameObject);
        }
        groceries.Clear();
    }

    protected void ShowBubble()
    {
        speechBubble.gameObject.SetActive(true);
        soundManager.PlaySoundOnce(bubbleAppearedSound);
    }

    protected void HideBubble()
    {
        speechBubble.gameObject.SetActive(false);
        soundManager.PlaySoundOnce(bubbleDisappearedSound);
    }
}
