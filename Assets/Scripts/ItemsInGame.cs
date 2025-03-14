using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInGame : MonoBehaviour
{
    public static ItemsInGame itemsInstance;
    public List<ItemParameter> items = new List<ItemParameter>();

    private void Awake()
    {
        if (itemsInstance == null)
        {
            itemsInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
    }
}
