using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [Range(0.1f,5)]
    [SerializeField] private float sensitivity = 1;
    public float Sensitivity => sensitivity;

    public void SetSensitivity(float newSensitivity) => sensitivity = newSensitivity;


    [SerializeField] private bool useCameraSmoothing;
    public bool UseCameraSmoothing => useCameraSmoothing;

    public void SetCameraSmoothBool(bool newCameraSmoothBool) => useCameraSmoothing = newCameraSmoothBool;


    [Range(10f, 30f)]
    [SerializeField] private float smoothingSensitivity = 30;
    public float SmoothingSensitivity => smoothingSensitivity;

    public void SetCameraSmoothing(float newSmoothingSensitivity) => smoothingSensitivity = newSmoothingSensitivity;
}
