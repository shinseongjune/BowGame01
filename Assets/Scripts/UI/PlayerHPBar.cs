using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] Stats playerStats;
    
    Slider slider;
    TextMeshProUGUI text;

    void Start()
    {
        slider = GetComponent<Slider>();
        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        slider.value = playerStats.hp / playerStats.stats[(int)Stat.Type.MaxHP].Value;
        text.text = string.Format("{0} / {1}", Mathf.CeilToInt(playerStats.hp), Mathf.CeilToInt(playerStats.stats[(int)Stat.Type.MaxHP].Value));
    }
}
