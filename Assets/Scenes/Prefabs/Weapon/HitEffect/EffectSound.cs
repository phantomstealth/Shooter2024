using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSound : MonoBehaviour
{
    public AudioClip soundWoodImpact;
    public AudioClip soundMetallImpact;
    public Info_Object.tMatObj materialObject;
    private AudioSource source;
    public float destroyAfter = 2f;
    void Start()
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }


    void PlayAudio(AudioClip clip, bool loop)
    {
        source.loop = loop;
        source.clip = clip;
        source.Play();
    }

    public void AudioByMaterial()
    {
        if (materialObject == Info_Object.tMatObj.Wood)
        {
            PlayAudio(soundWoodImpact, false);
            Debug.Log("Звук попадания в дерево");
        }
        else if (materialObject == Info_Object.tMatObj.Metall)
        {
            PlayAudio(soundMetallImpact, false);
        }
    }
    // Update is called once per frame
}
