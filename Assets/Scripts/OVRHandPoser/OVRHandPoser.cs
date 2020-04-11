using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class OVRHandPoser : GrabbableEvents
{
    public HandPose currentPose;

    public OVRHandPoserHand leftHand;
    public OVRHandPoserHand rightHand;

    public OVRPoserModelManager ModelManager = null;

    public Grabbable _grabbable;

    public Animator[] _knownAnimators;

    void Awake()
    {
        if (leftHand == null) leftHand = new OVRHandPoserHand(true);
        if (rightHand == null) rightHand = new OVRHandPoserHand(false);

        //Destroy any development objects. 
        if (leftHand.previewAsset != null)
        {
            Destroy(leftHand.previewAsset);
        }

        if (rightHand.previewAsset != null)
        {
            Destroy(rightHand.previewAsset);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FindModelManager();

        if (!ModelManager)
        {
            Debug.LogError($"OVRHandPoser on {gameObject.name} failed to activate because it could not find a valid OVRPoserModelManager.");
            this.enabled = false;
        }

    }



    // Update is called once per frame
    void Update()
    {

    }

    public override void OnGrab(Grabber grabber)
    {
        base.OnGrab(grabber);

        //We want to disable the animator on the hand.
        //TODO: Optimise
        _knownAnimators = grabber.HandsGraphics.GetComponentsInChildren<Animator>();
        SetAnimatorListItemsActive(_knownAnimators, false);

        if (grabber.HandSide == ControllerHand.Left)
        {
            SetPose(ModelManager.LeftHand.transform, currentPose.BoneDataRight);
        }
        else
        {
            SetPose(ModelManager.RightHand.transform, currentPose.BoneDataRight);
        }
        //And set out pose.
    }

    public override void OnRelease()
    {
        SetAnimatorListItemsActive(_knownAnimators, true);
    }

    private void SetAnimatorListItemsActive(Animator[] animators, bool active)
    {
        foreach (Animator animator in animators)
        {
            animator.enabled = active;
        }
    }

    public void FindModelManager()
    {
        if (ModelManager != null)
        {
            return;
        }
        else
        {
            OVRPoserModelManager[] mms = GameObject.FindObjectsOfType<OVRPoserModelManager>();
            if (mms.Length > 1)
            {
                Debug.LogWarning("WARNING: More than one OVRPoser model manager found. Conflict expected.");
            }
            else if (mms.Length == 1)
            {
                ModelManager = mms[0];
            }
            else
            {
                Debug.LogError("No model manager found for OVRHandPoser");
            }
        }
    }


    public Dictionary<string, BoneTransformData> FlipDictionary(Dictionary<string, BoneTransformData> boneDictionary)
    {
        Dictionary<string, BoneTransformData> newDictionary = new Dictionary<string, BoneTransformData>();
        foreach (string dictKey in boneDictionary.Keys)
        {
            BoneTransformData data = null;
            boneDictionary.TryGetValue(dictKey, out data);

            string newDictKey = dictKey.Replace("l_", "r_");

            Vector3 mirroredEulers = data.Rotation.eulerAngles;
            //mirroredEulers.x = -mirroredEulers.x;
            mirroredEulers.y = -mirroredEulers.y;
            mirroredEulers.z = -mirroredEulers.z;

            //data.Rotation = Quaternion.Euler(mirroredEulers);

            data.Position = new Vector3(-data.Position.x, data.Position.y, -data.Position.z);

            newDictionary.Add(newDictKey, data);
        }

        return newDictionary;
    }

    public void SetPose(Transform root, Dictionary<string, BoneTransformData> boneDictionary)
    {
        if (root == null) return;

        foreach (string dictKey in boneDictionary.Keys)
        {
            string searchKey = dictKey;
            if (dictKey == "b_r_wrist")
            {
                Debug.Log("Setting wrist to hand");
                searchKey = "b_r_hand";
            }
            Transform child = root.FindChildRecursive(searchKey);
            if (child != null)
            {
                BoneTransformData transformData = null;
                boneDictionary.TryGetValue(dictKey, out transformData);

                if (child.name.Contains("ring") || child.name.Contains("pinky") || child.name.Contains("index") || child.name.Contains("middle") || child.name.Contains("thumb"))
                {
                    child.transform.localPosition = transformData.Position;

                    child.transform.localRotation = transformData.Rotation;
                }
                //child.transform.localScale = transformData.Scale;
            }
        }
    }


    public Dictionary<string, BoneTransformData> ReadPose(OVRHandPoserHand hand, Transform overrideTransformRoot = null, Dictionary<string, BoneTransformData> currDictionary = null)
    {
        Dictionary<string, BoneTransformData> outDictionary = (currDictionary != null ? currDictionary : new Dictionary<string, BoneTransformData>());
        Transform transformRoot = (overrideTransformRoot ? overrideTransformRoot : hand.previewInWorld.transform);

        for (int i = 0; i < transformRoot.childCount; i++)
        {
            //go into child and add to my point
            BoneTransformData newBoneTransform = new BoneTransformData();
            Transform existingBoneTransform = transformRoot.GetChild(i);
            newBoneTransform.Rotation = existingBoneTransform.localRotation;
            newBoneTransform.Position = existingBoneTransform.localPosition;
            newBoneTransform.Scale = existingBoneTransform.localScale;

            if (!outDictionary.ContainsKey(existingBoneTransform.name))
                outDictionary.Add(existingBoneTransform.name, newBoneTransform);

            ReadPose(hand, transformRoot.GetChild(i), outDictionary);
        };

        return outDictionary;
    }
}

public class OVRHandPoserHand
{

    public bool isLeftHand = false;
    public GameObject previewAsset = null;

    private GameObject _previewInWorld = null;

    public OVRHandPoserHand(bool isLeftHand)
    {
        this.isLeftHand = isLeftHand;
    }

    public GameObject previewInWorld
    {
        get
        {
            return _previewInWorld;
        }
        set
        {
            _previewInWorld = value;
            _previewInWorld.name = "OVRPoserTemplate_" + (previewAsset.name.Contains("_L") || previewAsset.name.ToLower().Contains("left") ? "LEFT" : "RIGHT");
        }
    }


}
