using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponInfo
{
    public GameObject weaponPrefab;//Prefab của súng
}

public class WeaponsSwitching : MonoBehaviour
{
    //animation
     public Animator gunsAnim;

    //Weapons
    [SerializeField] private GameObject pistolPrefab;//Prefab của súng
    [SerializeField] private GameObject PrimaryGunPrefab;//Prefab của súng
    [SerializeField] private List<WeaponInfo> weapons = new List<WeaponInfo>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunsAnim = GetComponentInChildren<Animator>();
        gunsAnim.SetInteger("WeaponType", 0);

        //Set up weapon
        weapons.Add(new WeaponInfo { weaponPrefab = pistolPrefab });
        weapons.Add(new WeaponInfo { weaponPrefab = PrimaryGunPrefab });


        //ActiveWeapons(0);
        ActiveWeapons(1);
    }

    // Update is called once per frame
    void Update()
    {
        gunsAnim = GetComponentInChildren<Animator>();
        
        //Đổi súng
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunsAnim.SetInteger("WeaponType", 1);
            gunsAnim.SetTrigger("DrawWeapons");
            ActiveWeapons(0);
            //gunsAnim = weapons[0].weaponPrefab.GetComponent<Animator>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunsAnim.SetInteger("WeaponType", 0);
            gunsAnim.SetTrigger("DrawWeapons");
            ActiveWeapons(1);
            //gunsAnim = weapons[1].weaponPrefab.GetComponent<Animator>();
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
