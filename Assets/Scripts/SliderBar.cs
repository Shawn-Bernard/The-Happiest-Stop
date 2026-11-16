using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string sliderString;
    [SerializeField] private TextMeshProUGUI stringText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stringText ??= GetComponentInChildren<TextMeshProUGUI>();
        slider ??= GetComponent<Slider>();
        slider.onValueChanged?.Invoke(slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetText(float value)
    {
        stringText.text = $"{sliderString} {value:F1}";
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(SetText);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(SetText);
    }
}
