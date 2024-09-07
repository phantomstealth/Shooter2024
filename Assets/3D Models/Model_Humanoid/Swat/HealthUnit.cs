using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUnit : MonoBehaviour
{
    private Text healthCharacterTXT;
    public int healthCharacter=100;
    public int armorCharacter=100;
    // Start is called before the first frame update
    void Start()
    {
        healthCharacterTXT= GameObject.Find("numHealth").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthCharacterTXT.text = healthCharacter.ToString()+"%";
    }
}
