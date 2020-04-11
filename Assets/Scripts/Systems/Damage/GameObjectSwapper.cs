using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwapper : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _objectsToHide = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _objectsToShow = new List<GameObject>();

    [SerializeField]
    bool destroyObjects = true;

    public void DoSwap()
    {
        foreach (GameObject obj in _objectsToHide)
        {
            if (destroyObjects)
            {
                Destroy(obj);
            }
            else
            {
                obj.SetActive(false);
            }
        }

        foreach (GameObject obj in _objectsToShow)
        {
            obj.SetActive(true);
        }
    }
}
