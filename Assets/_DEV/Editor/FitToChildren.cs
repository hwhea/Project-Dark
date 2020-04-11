using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
using System.Collections;

public class FitToChildren : MonoBehaviour
{

    [MenuItem("My Tools/Collider/Copy Child Colliders")]
    static void CopyColliders()
    {
        foreach (GameObject rootGameObject in Selection.gameObjects)
        {
            for (int i = 0; i < rootGameObject.transform.childCount; i++)
            {
                Transform child = rootGameObject.transform.GetChild(i);
                for (int x = 0; x < child.gameObject.GetComponents<Collider>().Length; x++)
                {
                    
                }
            }
        }
    }


    [MenuItem("My Tools/Collider/Fit to Children")]
    static void Fit()
    {
        foreach (GameObject rootGameObject in Selection.gameObjects)
        {
            if (!(rootGameObject.GetComponent<Collider>() is BoxCollider))
                continue;

            bool hasBounds = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            for (int i = 0; i < rootGameObject.transform.childCount; ++i)
            {
                Renderer childRenderer = rootGameObject.transform.GetChild(i).GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    if (hasBounds)
                    {
                        bounds.Encapsulate(childRenderer.bounds);
                    }
                    else
                    {
                        bounds = childRenderer.bounds;
                        hasBounds = true;
                    }
                }
            }

            BoxCollider collider = (BoxCollider)rootGameObject.GetComponent<Collider>();
            collider.center = bounds.center - rootGameObject.transform.position;
            collider.size = bounds.size;
        }
    }

}
