using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bar : MonoBehaviour
{
    public Text numbers;

    private Slider slider;
    public Gradient gradient;
    public Image fill;

    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void UpdateBar(int _max, int _val)
    {
        slider.maxValue = _max;
        slider.value = _val;

        fill.color = gradient.Evaluate(slider.normalizedValue);

        numbers.text = _val.ToString() + "/" + _max.ToString();
    }
}
