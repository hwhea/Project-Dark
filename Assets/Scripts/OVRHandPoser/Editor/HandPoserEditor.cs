using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OVRHandPoser))]
public class HandPoserEditor : Editor
{
    static string PREVIEW_HAND_DEMO_PATH = "Assets/Oculus/VR/Prefabs/OVRCustomHandPrefab_{side}.prefab";

    string errorMessage = "";

    bool rightPreview = false;
    bool leftPreview = false;

    SerializedProperty currentPose = null;

    OVRHandPoser poser;

    GUIContent m_toggleContentEnabled;
    GUIContent m_toggleContentDisabled;

    HandPose lastPose = null;


    void OnEnable()
    {
        currentPose = serializedObject.FindProperty("currentPose");

        poser = target as OVRHandPoser;

        if (poser.leftHand == null) poser.leftHand = new OVRHandPoserHand(true);
        if (poser.rightHand == null) poser.rightHand = new OVRHandPoserHand(false);


        if (poser.leftHand.previewAsset == null)
        {
            poser.leftHand.previewAsset = (GameObject)Resources.Load("CustomHandLeftBlack");
        }

        if (poser.rightHand.previewAsset == null)
        {
            poser.rightHand.previewAsset = (GameObject)Resources.Load("CustomHandRightBlack");
        }

        m_toggleContentEnabled = new GUIContent((Texture)Resources.Load("ovrposer_hand_enabled"));
        m_toggleContentDisabled = new GUIContent((Texture)Resources.Load("ovrposer_hand_disabled"));

        leftPreview = poser.leftHand.previewInWorld != null;
        rightPreview = poser.rightHand.previewInWorld != null;

        TryDiscoverHandPreviews(poser.leftHand);
        TryDiscoverHandPreviews(poser.rightHand);

        poser.FindModelManager();

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUI.color = Color.cyan;
        GUILayout.Label("OVR Hand Poser", EditorStyles.boldLabel);
        GUI.color = Color.white;
        if (errorMessage != "")
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            errorMessage = "";
        }

        if (poser.leftHand.previewAsset == null)
        {
            errorMessage = $"Failed to find left hand preview asset at {PREVIEW_HAND_DEMO_PATH}. Do you have OVR installed?";
        }

        if (poser.rightHand.previewAsset == null)
        {
            errorMessage = $"Failed to find right hand preview asset at {PREVIEW_HAND_DEMO_PATH}. Do you have OVR installed?";
        }

        #region PoseSettings
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        EditorGUILayout.ObjectField(currentPose);

        //TODO: IMPROVEMENT: Not sure this is required?
        if (poser.currentPose != lastPose && poser.currentPose != null)
        {
            EditorUtility.SetDirty(poser.currentPose);
        }

        if (poser.currentPose == null)
        {
            SetPreviewActive(poser.leftHand, false);
            SetPreviewActive(poser.rightHand, false);

            if (GUILayout.Button("Create Pose"))
            {
                HandPose pose = ScriptableObject.CreateInstance<HandPose>();

                var path = EditorUtility.SaveFilePanel(
                    "Create new pose",
                    "",
                    "my_pose.asset",
                    "asset");

                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                Debug.Log("Saving at path " + path);

                if (path.Length != 0)
                {

                    AssetDatabase.CreateAsset(pose, path);
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                }

                poser.currentPose = pose;
                lastPose = pose;

                // Default show both.
                SetPreviewActive(poser.leftHand, true);
                SetPreviewActive(poser.rightHand, true);
            }
        }
        else
        {
            string loadString = "Load Pose";
            if (poser.currentPose != lastPose)
            {
                GUI.color = Color.red;
                loadString += " (WARN: Will overwrite unsaved data).";
            }


            if (GUILayout.Button(loadString))
            {
                //Left it here, need to copy these transforms.
                SetPreviewActive(poser.leftHand, true);
                SetPreviewActive(poser.rightHand, true);
                lastPose = poser.currentPose;
            }
            GUI.color = Color.white;

            if (GUILayout.Button("Save Pose" + (poser.currentPose != lastPose ? " *" : "")))
            {
                bool leftPrevActive = poser.leftHand.previewInWorld != null;
                bool rightPrevActive = poser.rightHand.previewInWorld != null;

                //Show both to grab position data.
                SetPreviewActive(poser.leftHand, true);
                SetPreviewActive(poser.rightHand, true);


                poser.currentPose.BoneDataLeft = poser.ReadPose(poser.leftHand);
                poser.currentPose.BoneDataRight = poser.ReadPose(poser.rightHand);

                //Restore previews 
                SetPreviewActive(poser.leftHand, leftPrevActive);
                SetPreviewActive(poser.rightHand, rightPrevActive);

                Debug.Log("Saved!");
                lastPose = poser.currentPose;
            }

        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        #endregion


        GUILayout.Space(20f);
        GUILayout.Label("Poser Tools", EditorStyles.boldLabel);

        if (poser.currentPose == null)
        {
            GUILayout.Label("Please select or create a pose to begin.");
        }
        else
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            if (GUILayout.Button(leftPreview ? m_toggleContentEnabled : m_toggleContentDisabled, GUILayout.Width(64), GUILayout.Height(64)))
            {
                leftPreview = !leftPreview;
                SetPreviewActive(poser.leftHand, leftPreview);
            }

            if (GUILayout.Button("Mirror to right >>"))
            {
                SetPreviewActive(poser.rightHand, false);
                poser.currentPose.BoneDataLeft = poser.ReadPose(poser.leftHand);
                poser.currentPose.BoneDataRight = poser.FlipDictionary(poser.currentPose.BoneDataLeft);
                SetPreviewActive(poser.rightHand, true);
            }

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button(rightPreview ? m_toggleContentEnabled : m_toggleContentDisabled, GUILayout.Width(64), GUILayout.Height(64)))
            {
                rightPreview = !rightPreview;
                SetPreviewActive(poser.rightHand, rightPreview);
            }
            if (GUILayout.Button("<< Mirror to left"))
            {
                SetPreviewActive(poser.rightHand, false);
                poser.currentPose.BoneDataLeft = poser.ReadPose(poser.leftHand);
                poser.currentPose.BoneDataRight = poser.FlipDictionary(poser.currentPose.BoneDataLeft);
                SetPreviewActive(poser.rightHand, true);
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }





        serializedObject.ApplyModifiedProperties();
    }

    private void TryDiscoverHandPreviews(OVRHandPoserHand hand)
    {
        GameObject discovered = poser.transform.Find("OVRPoserTemplate_" + (hand.isLeftHand ? "LEFT" : "RIGHT"))?.gameObject;

        if (discovered != null) hand.previewInWorld = discovered;
    }

    private void LoadPose(OVRHandPoserHand hand)
    {
        if (hand.previewInWorld != null)
        {
            poser.SetPose(hand.previewInWorld.transform, (hand.isLeftHand ? poser.currentPose.BoneDataLeft : poser.currentPose.BoneDataRight));
        }
    }

    private void SetPreviewActive(OVRHandPoserHand hand, bool active)
    {

        //Try discover previews by name.

        if (active)
        {
            if (hand.previewInWorld == null)
            {
                Transform grabHandle = poser.GetComponent<Grabbable>().GrabPoints[0];

                Transform grabPoint = hand.previewAsset.transform.FindChildRecursive("gripTrans");

                hand.previewInWorld = Instantiate(hand.previewAsset, hand.previewAsset.transform.position, hand.previewAsset.transform.rotation, poser.transform);

                if (hand.isLeftHand)
                {
                    hand.previewInWorld.transform.position = grabHandle.transform.position - (poser.ModelManager.LeftGrabber.transform.position - poser.ModelManager.LeftHand.transform.position);
                }
                else
                {
                    hand.previewInWorld.transform.position = grabHandle.transform.position - (poser.ModelManager.RightGrabber.transform.position - poser.ModelManager.RightHand.transform.position);
                }
                //Euler angles is done off of approximation.
                //hand.previewInWorld.transform.localEulerAngles = grabHandle.transform.forward + hand.previewAsset.transform.localEulerAngles;
                LoadPose(hand);
            }
        }
        else
        {
            if (hand.previewInWorld != null)
            {
                DestroyImmediate(hand.previewInWorld);
            }
        }

        rightPreview = (hand.isLeftHand ? rightPreview : active);
        leftPreview = (hand.isLeftHand == false ? leftPreview : active);

    }

}
