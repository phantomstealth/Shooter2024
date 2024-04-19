using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour
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
}
