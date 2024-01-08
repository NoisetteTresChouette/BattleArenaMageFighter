using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The different objects that can be drop")]
    private List<GameObject> _lootTable;

    [SerializeField]
    [RangeAttribute(0,1)]
    [Tooltip("At what probability can this entity drop somenthing")]
    private float _probability;

    private bool _hasTried;

    public void TryDrop()
    {
        if (_hasTried) return;
        float rnd = Random.value;

        if (rnd > _probability)
        {
            int rndIndex = Random.Range(0, _lootTable.Count);
            Instantiate(_lootTable[rndIndex], transform.position,Quaternion.identity);
        }

        _hasTried = true;
    }

}
