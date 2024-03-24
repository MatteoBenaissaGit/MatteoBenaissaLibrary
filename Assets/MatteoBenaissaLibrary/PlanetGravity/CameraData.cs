using System;
using UnityEngine;

namespace Data.Camera
{
    [Serializable]
    public class CameraInformation
    {
        [field:SerializeField] public string ID { get; private set; }
        [field:SerializeField] public int Priority { get; private set; }
        [field:SerializeField] public float Fov { get; private set; }
        [field:SerializeField] public float SpeedToChange { get; private set; }
        [field:SerializeField] public Vector2 Offset { get; private set; }
        [field:SerializeField] public Vector3 Rotation { get; private set; }
        [field:SerializeField] public Vector2 RotationSpeedMultiplier { get; private set; } = Vector2.one;
    }
    
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Camera", order = 1)]
    public class CameraData : ScriptableObject
    {
        [field: SerializeField] public float CameraXMovementSpeed { get; private set; } = 100f;
        [field: SerializeField] public float CameraYMovementSpeed { get; private set; } = 100f;
        [field: SerializeField] public Vector2 CameraYClamp { get; private set; } = new Vector2(-10,10);
        [field: SerializeField] public CameraInformation BaseCamera { get; private set; }
        [field: SerializeField] public CameraInformation RunCamera { get; private set; }
        [field: SerializeField] public CameraInformation JumpCamera { get; private set; }
        [field: SerializeField] public CameraInformation FallCamera { get; private set; }
    }
}