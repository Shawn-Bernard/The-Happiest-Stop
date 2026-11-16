using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float sensitivity = 1;
    public float Sensitivity => sensitivity;


    [SerializeField] private bool useCameraSmoothing;
    public bool UseCameraSmoothing => useCameraSmoothing;


    [Range(10f, 30f)]
    [SerializeField] private float cameraSmoothing = 30;
    public float CameraSmoothing => cameraSmoothing;
}
