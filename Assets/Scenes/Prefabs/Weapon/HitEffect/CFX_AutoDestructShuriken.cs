using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShurikenNET : MonoBehaviour
{
	public bool OnlyDeactivate;
	public float destroyAfter = 2f;

    void DestroySelf()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    void FixedUpdate()
    {
		CheckAliveNew();
    }


    void CheckAliveNew()
	{
		if (!GetComponent<ParticleSystem>().IsAlive(true))
		{
			if (OnlyDeactivate)
			{
				gameObject.SetActive(false);
			}
			else
				DestroySelf();
		}
    }
}
