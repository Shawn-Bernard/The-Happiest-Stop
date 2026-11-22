using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Range(0.1f,5)]
    [SerializeField] private float sensitivity = 1;
    public float Sensitivity => sensitivity;

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        SaveSettings();
    }


    [SerializeField] private bool useCameraSmoothing;
    public bool UseCameraSmoothing => useCameraSmoothing;

    public void SetCameraSmoothBool(bool newCameraSmoothBool)
    {
        useCameraSmoothing = newCameraSmoothBool;
        SaveSettings();
    }


    [Range(10f, 30f)]
    [SerializeField] private float smoothingSensitivity = 30;
    public float SmoothingSensitivity => smoothingSensitivity;

    public void SetCameraSmoothing(float newSmoothingSensitivity)
    {
        smoothingSensitivity = newSmoothingSensitivity;
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);

        if (useCameraSmoothing) PlayerPrefs.SetInt("CameraSmoothing", 1);
        else PlayerPrefs.SetInt("CameraSmoothing", 0);

        PlayerPrefs.SetFloat("SmoothingSensitivity", smoothingSensitivity);

        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", sensitivity);

        int cameraSmoothing = PlayerPrefs.GetInt("CameraSmoothing");

        if (PlayerPrefs.HasKey("CameraSmoothing"))
        {
            useCameraSmoothing = cameraSmoothing == 1;
        }

        smoothingSensitivity = PlayerPrefs.GetFloat("SmoothingSensitivity", smoothingSensitivity);
    }
}
