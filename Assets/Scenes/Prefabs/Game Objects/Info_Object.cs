using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_Object : MonoBehaviour
{
    public string nameObject;
    public int healthObject;
    public GameObject destroyedVersion;	// Reference to the shattered version of the object

    // Start is called before the first frame update
    public void Damage(int damage)
    {
        healthObject = healthObject - damage;
        if (healthObject < 0) DestroyObject();
    }

    public void DestroyObject()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
        Debug.Log(nameObject + " уничтожен");
    }
}
