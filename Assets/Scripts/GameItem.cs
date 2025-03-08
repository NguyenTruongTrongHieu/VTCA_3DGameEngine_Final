using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameItem", menuName = "Scriptable Objects/GameItem")]
public class GameItem : ScriptableObject
{
    public string name;
    public int id;
    public int healing;
    public int ammo;
    public GameObject prefab;
    public Sprite image;

    public void SpawnEntities(Vector3 position)
    {
        var itemPrefab = Instantiate(prefab, position, Quaternion.identity);
        var item = itemPrefab.GetComponent<ItemParameter>();
        item.name = name;
        item.id = id;
        item.healing = healing;
        item.ammo = ammo;
        item.image = image;
    }
}
