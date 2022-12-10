using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FillBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Awake()
    {
       // _slider = GetComponent<Slider>();
    }

    public void OnValueChanged(float value, float maxValue)
    {
        _slider.value = value / maxValue;
    }
}
