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
    public List<ItemInventory> items = new List<ItemInventory>();
    [SerializeField] private Weapon playerAKWeapon;
    [SerializeField] private Weapon playerPítolWeapon;
    [SerializeField] private PlayerStats health;

    [SerializeField] private GameObject firstAidPanel;
    [SerializeField] private Image firstAidImage;
    [SerializeField] private Image coolDownImage;
    private float coolDownTime;//To use first aid
    private bool canUseFirstAid = true;
    [SerializeField] private TextMeshProUGUI firstAidText;
    [SerializeField] private Button pickingButton;
    private bool canPickUpItem;
    //[SerializeField] private PlayerHealth playerHealth;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //firstAidPanel.SetActive(false);
        coolDownImage.enabled = false;
        pickingButton.gameObject.SetActive(false);
        ShowInventory();
    }

    // Update is called once per frame
    void Update()
    {
        //Count down time
        coolDownTime += Time.deltaTime;
        coolDownImage.fillAmount = coolDownTime / 0.5f;
        if (coolDownTime > 0.5f)
        {
            coolDownTime = 0f;
            canUseFirstAid = true;
            coolDownImage.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && canUseFirstAid)
        {
            UseItem("FirstAid");
            canUseFirstAid = false;
            coolDownImage.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            //Activate picking button
            pickingButton.gameObject.SetActive(true);

            var item = other.gameObject.GetComponent<ItemParameter>();

            //  pickingButton.onClick.AddListener(()=> { PickingUpItem(item); });

            if (Input.GetKeyDown(KeyCode.F))
            {
                //Thêm âm thanh khi nhặt đồ
                AudioManager.audioInstance.PlaySFX("PickUpItem");

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
            canPickUpItem = false;
        }
    }

    public void PickingUpItem(ItemParameter item)
    {
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

        if (item.name == "Ammo")
        {
            UseItem("Ammo");
        }

        ShowInventory();
    }

    void UseItem(string nameItem)
    {
        ItemInventory inventory = items.Find(i => i.item.name == nameItem);//Check nếu player đã lấy item này rồi

        if (inventory == null)
        {
            Debug.Log(nameItem +" null");
            return;
        }
        else
        {
            //Use item
            if (inventory.quantity == 0)//Nếu số lượng = 0 thì không dùng được
            {
                //Có thể xoá item đi khi đã dùng hết
                items.Remove(inventory);
                return;
            }

            if (nameItem == "Ammo")
            {
                playerAKWeapon.ammoClip += inventory.item.ammo / 2;
                playerPítolWeapon.ammoClip += inventory.item.ammo / 2;
                ReduceQuantity(nameItem);
            }

            if (health.currentHealth != health.maxHealth && nameItem == "FirstAid")//Nếu health đang max thì không cần dùng item
            {
                health.currentHealth = Mathf.Min(health.maxHealth, health.currentHealth + inventory.item.healing);
                health.healthBar.SetHealth(health.currentHealth);
                ReduceQuantity(nameItem);
            }
            else return;

            ShowInventory();

        }
    }

    void ShowInventory()
    {
        foreach (var item in items)
        {
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

    void ReduceQuantity(string nameItem)
    {
        items.Find(i => i.item.name == nameItem).quantity = Mathf.Max(0, items.Find(i => i.item.name == nameItem).quantity - 1);
        Debug.Log(items.Find(i => i.item.name == nameItem).quantity);
    }
}
