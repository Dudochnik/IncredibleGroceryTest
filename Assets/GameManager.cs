using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CashierScript cashier;
    public ClientScript client;
    public List<GroceryImageLink> groceries = new List<GroceryImageLink>();
    public List<Button> buttons = new List<Button>();

    public AudioClip buttonClickSound;
    public Storage storage;
    public SoundManager soundManager;

    [SerializeField]
    int money;



    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            money = (int)bf.Deserialize(file);
            file.Close();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(client.LetNewClientIn());

        cashier.SetMoney(money);

        if (soundManager.MusicOn)
            soundManager.PlayMusic();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(delegate { soundManager.PlaySoundOnce(buttonClickSound); });
        }
    }

    
    public void SaveGameData()
    {
        money = cashier.totalMoney;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, money);
        file.Close();
    }
}
