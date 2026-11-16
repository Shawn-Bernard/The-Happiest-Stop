using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string volumeString;
    [SerializeField] private TextMeshProUGUI volumeText;

    private SoundManager SoundManager => GameManager.Instance.SoundManger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeText ??= GetComponentInChildren<TextMeshProUGUI>();
        //volumeSlider.onValueChanged.AddListener(SoundManager.SetMasterVolume);
        volumeSlider.onValueChanged?.Invoke(volumeSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetText(float volume)
    {
        volumeText.text = $"{volumeString} Volume {volume:F1}";
    }

    private void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(SetText);
    }

    private void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveListener(SetText);
    }
}
