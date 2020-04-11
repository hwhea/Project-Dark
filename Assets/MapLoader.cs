using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [AssetsOnly]
    [Required("Don't forget a map definition file, nothing will work without it!")]
    [InlineEditor]
    public MapDefinition mapDefinition;



    // Start is called before the first frame update
    void Start()
    {

    }

    public RoundSettings GetRoundByNumber(int roundNumber)
    {
        return mapDefinition.rounds[roundNumber - 1];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
