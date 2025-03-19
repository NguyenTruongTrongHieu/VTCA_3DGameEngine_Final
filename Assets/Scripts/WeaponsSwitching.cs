using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo
{
    public GameObject weaponPrefab;//Prefab của súng
}

public class WeaponsSwitching : MonoBehaviour
{
    private GameObject akIcon;
    private GameObject pistolIcon;

    //animation

    //Weapons
    [SerializeField] private GameObject pistolPrefab;//Prefab của súng
    [SerializeField] private GameObject PrimaryGunPrefab;//Prefab của súng
    [SerializeField] private List<WeaponInfo> weapons = new List<WeaponInfo>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        akIcon = GameObject.FindGameObjectWithTag("AKIcon");
        pistolIcon = GameObject.FindGameObjectWithTag("PistolIcon");

        //Set up weapon
        weapons.Add(new WeaponInfo { weaponPrefab = pistolPrefab });
        weapons.Add(new WeaponInfo { weaponPrefab = PrimaryGunPrefab });


        //ActiveWeapons(0);
        ActiveWeapons(1);
        akIcon.gameObject.SetActive(true);
        pistolIcon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {    
        //Đổi súng
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Pistol
        {
            ActiveWeapons(0);
            //gunsAnim = weapons[0].weaponPrefab.GetComponent<Animator>();

            
            akIcon.gameObject.SetActive(false);
            pistolIcon.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Primary Gun
        {
            ActiveWeapons(1);
            //gunsAnim = weapons[1].weaponPrefab.GetComponent<Animator>();

            akIcon.gameObject.SetActive(true);
            pistolIcon.gameObject.SetActive(false);
        }
    }

    void ActiveWeapons(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].weaponPrefab.SetActive(false);
        }

        var weapon = weapons[index];

        weapons[index].weaponPrefab.SetActive(true);
    }


}
