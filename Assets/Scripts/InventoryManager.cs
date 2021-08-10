using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.UI;
using UnityEngine.GameFoundation.DataPersistence;

public class InventoryManager : MonoBehaviour
{
    public Text points;
    public Text car1;
    public Text car2;
    public Text car3;
    public Text car4;
    public Button car1Button;
    public Button car2Button;
    public Button car3Button;
    public Button car4Button;
    public Button car1Choose;
    public Button car2Choose;
    public Button car3Choose;
    public Button car4Choose;
    public Text car1drive;
    public Text car2drive;
    public Text car3drive;
    public Text car4drive;
    public int car1price;
    public int car2price;
    public int car3price;
    public int car4price;
    public int playerCar;

    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public Text level2Text;
    public Text level3Text;
    public Text level4Text;
    public int level2price;
    public int level3price;
    public int level4price;
    string item;

    InventoryItem coinItem;
    InventoryItem car1Item;
    InventoryItem car2Item;
    InventoryItem car3Item;
    InventoryItem car4Item;
    InventoryItem level2Item;
    InventoryItem level3Item;
    InventoryItem level4Item;
    InventoryItem purchasedItem;

    public GameObject confirmPanel;
    private bool confirmation;

    IDataPersistence localPersistence;
    void Awake()
    {
        // choose what format you want to use
        JsonDataSerializer dataSerializer = new JsonDataSerializer();

        // choose where and how the data is stored
        localPersistence = new LocalPersistence(dataSerializer);

        // tell Game Foundation to initialize using this
        // persistence system. Only call Initialize once per session.
        if (!GameFoundation.IsInitialized)
        {
            GameFoundation.Initialize(localPersistence);
        }
    }

    void Start()
    {
        Load();

        // get the coin item by its ID
        coinItem = Wallet.GetItem("coin");
        car1Item = Inventory.main.GetItem("car1");
        car2Item = Inventory.main.GetItem("car2");
        car3Item = Inventory.main.GetItem("car3");
        car4Item = Inventory.main.GetItem("car4");
        level2Item = Inventory.main.GetItem("level2");
        level3Item = Inventory.main.GetItem("level3");
        level4Item = Inventory.main.GetItem("level4");

        points.text = coinItem.quantity.ToString() + " RN";

        if (car1Item.quantity != 0)
        {
            car1.text = "OWNED";
            car1Button.interactable = false;
        }
        else
        {
            car1.text = "PURCHASE: " + car1price.ToString() + "RN";
            car1Button.interactable = true;
        }

        if (car2Item.quantity != 0)
        {
            car2.text = "OWNED";
            car2Button.interactable = false;
        }
        else
        {
            car2.text = "PURCHASE: " + car2price.ToString() + "RN";
            car2Button.interactable = true;
        }

        if (car3Item.quantity != 0)
        {
            car3.text = "OWNED";
            car3Button.interactable = false;
        }
        else
        {
            car3.text = "PURCHASE: " + car3price.ToString() + "RN";
            car3Button.interactable = true;
        }

        if (car4Item.quantity != 0)
        {
            car4.text = "OWNED";
            car4Button.interactable = false;
        }
        else
        {
            car4.text = "PURCHASE: " + car4price.ToString() + "RN";
            car4Button.interactable = true;
        }

        CheckLevel();

        if (!PlayerPrefs.HasKey("PlayerCar"))
        {
            PlayerPrefs.SetInt("PlayerCar", 0);
        }

        playerCar = PlayerPrefs.GetInt("PlayerCar");

    }

    private void Update()
    {
        points.text = coinItem.quantity.ToString() + " RN";

        if (car1Item.quantity != 0)
        {
            car1.text = "OWNED";
            car1Button.interactable = false;
        }
        else
        {
            car1.text = "PURCHASE:\n" + car1price.ToString() + "RN";
            car1Button.interactable = true;
        }

        if (car2Item.quantity != 0)
        {
            car2.text = "OWNED";
            car2Button.interactable = false;
        }
        else
        {
            car2.text = "PURCHASE:\n" + car2price.ToString() + "RN";
            car2Button.interactable = true;
        }

        if (car3Item.quantity != 0)
        {
            car3.text = "OWNED";
            car3Button.interactable = false;
        }
        else
        {
            car3.text = "PURCHASE:\n" + car3price.ToString() + "RN";
            car3Button.interactable = true;
        }

        if (car4Item.quantity != 0)
        {
            car4.text = "OWNED";
            car4Button.interactable = false;
        }
        else
        {
            car4.text = "PURCHASE:\n" + car4price.ToString() + "RN";
            car4Button.interactable = true;
        }

        CheckLevel();

        car1Choose.interactable = true;
        car2Choose.interactable = true;
        car3Choose.interactable = true;
        car4Choose.interactable = true;
        car1Choose.enabled = true;
        car2Choose.enabled = true;
        car3Choose.enabled = true;
        car4Choose.enabled = true;
        car2drive.text = "DRIVE";
        car3drive.text = "DRIVE";
        car4drive.text = "DRIVE";

        if (PlayerPrefs.GetInt("PlayerCar") == 0)
        {
            car1Choose.interactable = false;
        }
        if(car2Item.quantity == 0)
        {
            car2drive.text = "";
            car2Choose.enabled = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("PlayerCar") == 1)
            {
                car2Choose.interactable = false;
            }
        }
        if (car3Item.quantity == 0)
        {
            car3drive.text = "";
            car3Choose.enabled = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("PlayerCar") == 2)
            {
                car3Choose.interactable = false;
            }
        }
        if (car4Item.quantity == 0)
        {
            car4drive.text = "";
            car4Choose.enabled = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("PlayerCar") == 3)
            {
                car4Choose.interactable = false;
            }
        }

    }

    public void Save()
    {
        GameFoundation.Save(localPersistence);
    }

    public void Load()
    {
        GameFoundation.Load(localPersistence);
    }

    public void PurchaseItem()
    {
        InventoryItem purchasedItem = Inventory.main.GetItem(item);
        int price = 0;
        if (item == "car1") price = car1price;
        else if (item == "car2") price = car2price;
        else if (item == "car3") price = car3price;
        else if (item == "car4") price = car4price;
        else if (item == "level2") price = level2price;
        else if (item == "level3") price = level3price;
        else if (item == "level4") price = level4price;


        if (coinItem.quantity >= price)
        {
            coinItem.quantity -= price;
            purchasedItem.quantity = 1;
            confirmPanel.transform.localPosition = new Vector3(0, 1000, 0);
        }
    }

    public void ChooseCar(int item)
    {
        GameObject.Find("Level Select Manager").GetComponent<LevelManager>().car = item;
        PlayerPrefs.SetInt("PlayerCar", item);
    }

    private void CheckLevel()
    {
        if (level2Item.quantity != 0)
        {
            level2.SetActive(false);
        }
        else
        {
            level2.SetActive(true);
            level2Text.text = "UNLOCK LVL2:\n" + level2price + "RN";
        }

        if (level3Item.quantity != 0)
        {
            level3.SetActive(false);            
        }
        else
        {
            level3.SetActive(true);
            level3Text.text = "UNLOCK LVL3:\n" + level3price + "RN";
        }

        if (level4Item.quantity != 0)
        {
            level4.SetActive(false);
        }
        else
        {
            level4.SetActive(true);
            level4Text.text = "UNLOCK LVL4:\n" + level4price + "RN";
        }
    }

    public void Confirmation(string itemPicked)
    {
        confirmPanel.transform.localPosition = new Vector3(0, 0, 0);
        item = itemPicked;
    }

    public void Cancel()
    {
        confirmPanel.transform.localPosition = new Vector3(0, 1000, 0);
    }

    public void Buypoints()
    {
        IAPManager.Instance.Buy5000RN();
    }

    public void GetPoints()
    {
        coinItem.quantity += 5000;
    }
}
