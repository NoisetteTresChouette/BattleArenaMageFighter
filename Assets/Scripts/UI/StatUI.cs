using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{

    [System.Serializable]
    private class DictionnaryAssociation
    {
        public string statType;
        public TextMeshProUGUI textZone;
    }

    [SerializeField]
    [Tooltip("The association between the diffrenent type of stats and their tmpro components")]
    private List<DictionnaryAssociation> _buffTextAssociations;
    

    public void UpdateStats(string type, string value)
    {

        foreach (DictionnaryAssociation da in _buffTextAssociations)
        {
            if (da.statType == type)
            {
                da.textZone.text = value;
                break;
            }
        }
    }
}
