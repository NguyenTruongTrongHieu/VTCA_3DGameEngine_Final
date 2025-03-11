using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory
{
    public ItemParameter item;
    public int quantity;
}

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private List<ItemInventory> items = new List<ItemInventory>();
    [SerializeField] private Weapon playerWeapon;

    [SerializeField] private GameObject firstAidPanel;
    [SerializeField] private Image firstAidImage;
    [SerializeField] private TextMeshProUGUI firstAidText;
    [SerializeField] private Button pickingButton;
    //[SerializeField] private PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstAidPanel.SetActive(false);
        pickingButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //UseItem("Ammo");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            //Activate picking button
            pickingButton.gameObject.SetActive(true);

            var item = other.gameObject.GetComponent<ItemParameter>();

            pickingButton.onClick.AddListener(()=> { PickingUpItem(item); });

            if (Input.GetKeyDown(KeyCode.F))
            {
                PickingUpItem(item);
                Destroy(other.gameObject);
                pickingButton.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            //Deactivate picking button
            pickingButton.gameObject.SetActive(false);
        }
    }

    public void PickingUpItem(ItemParameter item)
    {
        //Thêm âm thanh khi nhặt đồ
        AudioManager.audioInstance.PlaySFX("PickUpItem");

        var playerItem = items.Find(i => i.item.id == item.id);

        if (playerItem != null)
        {
            playerItem.quantity++;
        }
        else
        {
            //null thì phải khởi tạo
            playerItem = new ItemInventory();
            playerItem.quantity = 1;//Nhặt được 1 cái đầu tiên
            playerItem.item = item;
            items.Add(playerItem);
        }

        ShowInventory();
    }

    void UseItem(string nameItem)
    {
        ItemParameter chooseItem = null;
        ItemInventory inventory = null;
        foreach (var item in items)//Check nếu player đã lấy item này rồi
        {
            if (item.item.name == nameItem)
            {
                chooseItem = item.item;
                inventory = item;
            }
        }

        if (chooseItem == null)
        {
            return;
        }
        else
        {
            //Use item
            if (inventory.quantity <= 0)//Nếu số lượng = 0 thì không dùng được
            {
                return;
            }
            inventory.quantity = Mathf.Max(0, inventory.quantity--);
        }
    }

    void ShowInventory()
    {
        foreach (var item in items) {
            Debug.Log($"{item.item.name}; {item.quantity}\n");
        }

        //Show first aid
        if (!firstAidPanel.activeSelf)
        { 
            firstAidPanel.SetActive(true);
        }

        var firstAid = items.Find(i => i.item.name.Equals("FirstAid"));
        if (firstAid != null) {
            firstAidText.text = firstAid.quantity.ToString();
            firstAidImage.sprite = firstAid.item.image;
        }
    }

    void DropItem()
    { 
        
    }
}
