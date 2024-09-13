using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Outline))]

public class Info_Object : MonoBehaviour
{
    public string nameObject;
    public int healthObject=5;
    public GameObject destroyedVersion; // Reference to the shattered version of the object
    private Outline outline;

    public enum tMatObj
    {
        Default,
        Wood,
        Metall,
        Meat
    }

    public tMatObj materialObject;

    private void OnEnable()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 0;
    }

    public void OnHoverEnter()
    {
        outline.OutlineWidth = 2;
    }

    public void OnHoverExit() 
    {
        outline.OutlineWidth = 0;
    }


    public void Damage(int damage)
    {
        healthObject = healthObject - damage;
        if (healthObject < 0) DestroyObject();
    }

    public void DestroyObject()
    {
        if (destroyedVersion!=null) Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
        Debug.Log(nameObject + " уничтожен");
    }
}
