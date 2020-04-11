using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRPoserModelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftHand;

    [SerializeField]
    private GameObject _rightHand;

    [SerializeField]
    private GameObject _rightGrabber;

    [SerializeField]
    private GameObject _leftGrabber;

    public GameObject LeftHand { get { return _leftHand; } }
    public GameObject RightHand { get { return _rightHand; } }

    public GameObject LeftGrabber { get { return _leftGrabber; } }
    public GameObject RightGrabber { get { return _rightGrabber; } }


    // Start is called before the first frame update
    void Start()
    {
        if (_leftHand == null)
        {
            Debug.LogError($"OVRPoserModelManager on {gameObject.name} does not have leftHand property set.");
        }
        if (_rightHand == null)
        {
            Debug.LogError($"OVRPoserModelManager on {gameObject.name} does not have rightHand property set.");
        }
        if (_rightGrabber == null)
        {
            Debug.LogError($"OVRPoserModelManager on {gameObject.name} does not have rightGrabber property set.");
        }
        if (_leftGrabber == null)
        {
            Debug.LogError($"OVRPoserModelManager on {gameObject.name} does not have leftGrabber property set.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
