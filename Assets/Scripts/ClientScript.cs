using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientScript : Human
{
    public float speed;
    public int jumpCounts;
    public float jumpHeight;
    public Transform happyImage;
    public Transform unhappyImage;
    public Transform startPoint;
    public Transform endPoint;

    bool isGoingToCashier = false;
    bool isGoingToEntrance = false;
    bool isClientHappy = false;
    List<GroceryImageLink> neededGroceries = new List<GroceryImageLink>();
    int groceriesCount;
    private List<Vector2> jumpPoints = new List<Vector2>();
    private List<Vector2> peakPoints = new List<Vector2>(); 
    private CashierScript cashierScript;


    protected override void Start()
    {
        base.Start();
        cashierScript = gameManager.cashier;

        CalculatePointsForJumps();
    }

    public IEnumerator LetNewClientIn()
    {
        happyImage.gameObject.SetActive(false);
        unhappyImage.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(false);

        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().enabled = true;

        isGoingToCashier = true;
        transform.DetachChildren();
        transform.localScale = new Vector3Int(1, 1, 1);
        speechBubble.parent = transform;
        transform.position = startPoint.transform.position;
        StartCoroutine(MoveToNextPoint());
    }

    IEnumerator MoveToNextPoint()
    {
        int iteratorIncrementForNextPoint;
        int currentPoint;
        int endPoint;
        int peakPointIncrement;

        if (isGoingToCashier)
        {
            iteratorIncrementForNextPoint = 1;
            currentPoint = 0;
            endPoint = jumpPoints.Count - 1;
            peakPointIncrement = 0;
        }
        else
        {
            iteratorIncrementForNextPoint = -1;
            currentPoint = jumpPoints.Count-1;
            endPoint = 0;
            peakPointIncrement = -1;
        }

        while (currentPoint != endPoint)
        {
            float t = 0;
            do
            {
                t += speed;
                Vector2 bezier = Bezier(t, jumpPoints[currentPoint], peakPoints[currentPoint + peakPointIncrement], jumpPoints[currentPoint + iteratorIncrementForNextPoint]);
                transform.position = bezier;
                yield return new WaitForFixedUpdate();
            } while (Vector3.Distance(transform.position, jumpPoints[currentPoint + iteratorIncrementForNextPoint]) > 0.001f);

            currentPoint = currentPoint + iteratorIncrementForNextPoint;
            transform.position = jumpPoints[currentPoint];
        }

        if (isGoingToCashier)
        {
            isGoingToCashier = false;
            RandomizeNeededGroceries();
            StartCoroutine(ShowNeededGroceries());
        } else if (isGoingToEntrance)
        {
            isGoingToEntrance = false;
            StartCoroutine(LetNewClientIn());
        }

    }


    void RandomizeNeededGroceries()
    {
        groceriesCount = Random.Range(1, 4);
        List<GroceryImageLink> allGroceries = new List<GroceryImageLink>(gameManager.groceries);
        neededGroceries.Clear();
        for (int i = 0; i < groceriesCount; i++)
        {
            int newGroceryIndex = Random.Range(0, allGroceries.Count);
            neededGroceries.Add(allGroceries[newGroceryIndex]);
            allGroceries.RemoveAt(newGroceryIndex);
        }
    }


    IEnumerator ShowNeededGroceries()
    {
        ShowBubble();
        List<Transform> showedItems = ShowGroceries(neededGroceries);

        yield return new WaitForSeconds(5);

        gameManager.storage.groceriesCount = neededGroceries.Count;
        StartCoroutine(cashierScript.storage.OpenStorage());

        while (!gameManager.storage.IsStorageActive)
            yield return null;

        DestroyGroceries(showedItems);
        HideBubble();
    }


    public void BuyGroceries(List<GroceryImageLink> receivedGroceries)
    {
        int payment = 0;
        int correctGroceries = 0;
        foreach(GroceryImageLink grocery in neededGroceries)
        {
            if (receivedGroceries.Contains(grocery))
            {
                correctGroceries++;
                payment += 10;
            }
        }

        if (correctGroceries == groceriesCount)
        {
            payment *= 2;
            isClientHappy = true;
        } else
        {
            isClientHappy = false;
        }

        ShowBubble();
        if (isClientHappy)
        {
            happyImage.gameObject.SetActive(true);
        } else
        {
            unhappyImage.gameObject.SetActive(true);
        }

        cashierScript.AddMoney(payment);

        isGoingToEntrance = true;
        transform.DetachChildren();
        transform.localScale = new Vector3Int(-1, 1, 1);
        speechBubble.parent = transform;
        StartCoroutine(MoveToNextPoint());
    }

    private void CalculatePointsForJumps()
    {
        //Finding all points for jumps on line between startPoint and endPoint
        float t = 0;
        float tIncrement;
        if (jumpCounts <= 0)
        {
            jumpCounts = 1;
        }
        tIncrement = 1f / jumpCounts;

        jumpPoints.Add(startPoint.position);
        for (int i = 0; i < jumpCounts - 1; i++)
        {
            t += tIncrement;
            Vector2 newPoint = Vector2.Lerp(startPoint.position, endPoint.position, t);
            jumpPoints.Add(newPoint);
        }
        jumpPoints.Add(endPoint.position);

        //Finding all peak points for jumps between each pair of jumPoints with jumpHeight
        for (int i = 0; i < jumpPoints.Count - 1; i++)
        {
            Vector2 lineMiddle = Vector2.Lerp(jumpPoints[i], jumpPoints[i + 1], 0.5f);
            Vector2 newPoint = new Vector2(lineMiddle.x, lineMiddle.y + jumpHeight);
            peakPoints.Add(newPoint);
        }
    }

    public Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    public int GetGroceriesCount()
    {
        return groceriesCount;
    }

    public List<GroceryImageLink> GetNeededGroceries()
    {
        return neededGroceries;
    }
}
