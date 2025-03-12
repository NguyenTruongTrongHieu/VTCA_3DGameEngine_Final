using UnityEngine;

public class GunsVFX : MonoBehaviour
{
    // --- Muzzle ---
    public GameObject muzzlePrefab;
    public GameObject muzzlePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireWeapon()
    {
        // --- Muzzle ---
        if (muzzlePrefab != null && muzzlePosition != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, muzzlePosition.transform.position, Quaternion.identity);
            Destroy(muzzleVFX, 10f);
        }
    }
}
