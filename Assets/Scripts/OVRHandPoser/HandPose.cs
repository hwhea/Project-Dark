using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pose", menuName = "OVR/Pose")]
public class HandPose : SerializedScriptableObject
{
    public Transform PoseRootRight;

    [InlineEditor]
    public Dictionary<string, BoneTransformData> BoneDataLeft = new Dictionary<string, BoneTransformData>();

    [InlineEditor]
    public Dictionary<string, BoneTransformData> BoneDataRight = new Dictionary<string, BoneTransformData>();



}

[System.Serializable]
public class BoneTransformData
{
    [SerializeField]
    public Quaternion Rotation { get; set; }

    [SerializeField]

    public Vector3 Position { get; set; }

    [SerializeField]

    public Vector3 Scale { get; set; }
}
