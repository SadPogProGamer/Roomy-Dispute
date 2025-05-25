using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowActivation : MonoBehaviour
{
    [SerializeField]
    private GameObject _arrow;

    private int _smallMax, _bigMax, _mediumMax;
    private bool _hasCheckedBig, _hasCheckedMedium;



    // Update is called once per frame
    void Update()
    {
        if (CompareTag("Item/Big"))
        {
            _bigMax = 8;
            _mediumMax = 9;
            _smallMax = 10;
        }
        if (CompareTag("Item/Long"))
        {
            _bigMax = 4;
            _mediumMax = 5;
            _smallMax = 6;
        }
        if (CompareTag("Item/Small"))
        {
            _bigMax = 0;
            _mediumMax = 3;
            _smallMax = 4;
        }
        CheckIfArrowNeedsToBeShown("Item/Placable/Big",_bigMax);

        if (_hasCheckedBig)
        {
            CheckIfArrowNeedsToBeShown("Item/Placable/Medium", _mediumMax);
        }
        if (_hasCheckedMedium)
        {
            CheckIfArrowNeedsToBeShown("Item/Placable/Small", _smallMax);
        }

        _hasCheckedBig = false;
        _hasCheckedMedium = false;
    }

    private void CheckIfArrowNeedsToBeShown(string tag, int maxChildCount)
    {
        if (transform.childCount < maxChildCount)
        {
            List<GameObject> listOfPlacables = GameObject.FindGameObjectsWithTag(tag).ToList();

            if (listOfPlacables.Where(placable => placable.transform.parent == null).ToList().Count != 0)
            {
                _arrow.SetActive(true);

            }
            else
            {
                if (tag.Contains("Big")) _hasCheckedBig = true;
                if (tag.Contains("Medium")) _hasCheckedMedium = true;
                _arrow.SetActive(false);
            }
                

        }
        else
        {
            if (tag.Contains("Big")) _hasCheckedBig = true;
            if (tag.Contains("Medium")) _hasCheckedMedium = true;
            _arrow.SetActive(false);
        }


    }
}
