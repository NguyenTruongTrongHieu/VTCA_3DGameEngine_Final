using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerHumanScriptable", menuName = "Scriptable Objects/SpawnerHumanScriptable")]
public class SpawnerHumanScriptable : ScriptableObject
{
    public string name;
    public string tag;
    public GameObject humanPrefab;

    public void SpawnEntity()
    { 
        Instantiate(humanPrefab);
    }
}
