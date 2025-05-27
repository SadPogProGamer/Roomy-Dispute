using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractability : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab, _moneyManager, _playerIndexStartingFrom0;
    private bool _placableHasEnoughMoney;

    // Update is called once per frame
    void Update()
    {

        if (_moneyManager.GetComponent<ButtonSelect>().MoneyManager.GetComponent<MoneyManager>().PlayerMoney[_playerIndexStartingFrom0.GetComponent<PlayerPointer>().PlayerIndex] >= _prefab.GetComponent<ItemStats>().Cost)
        {
            GetComponent<Button>().interactable = true;
            _placableHasEnoughMoney = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
            _placableHasEnoughMoney = false;
        }

        if (_prefab.tag.Contains("Placable") && _placableHasEnoughMoney)
        {
            List<GameObject> listOfTables = GameObject.FindGameObjectsWithTag("Item/Big").Where(table => table.transform.childCount>0).ToList();
            List<GameObject> listOfTvTables = GameObject.FindGameObjectsWithTag("Item/Long").Where(table => table.transform.childCount > 0).ToList();
            List<GameObject> listOfDrawers = GameObject.FindGameObjectsWithTag("Item/Small").Where(table => table.transform.childCount > 0).ToList();

                if (_prefab.tag.Contains("Big"))
                {
                    CheckIfPlacableOnHasEnoughSlots(3, listOfTables, listOfTvTables, listOfDrawers);
                }
                if (_prefab.tag.Contains("Medium"))
                {
                    CheckIfPlacableOnHasEnoughSlots(2, listOfTables, listOfTvTables, listOfDrawers);
                }
                if (_prefab.tag.Contains("Small"))
                {
                    CheckIfPlacableOnHasEnoughSlots(1, listOfTables, listOfTvTables, listOfDrawers);
                }
        } 
    }

    private void CheckIfPlacableOnHasEnoughSlots(int slotCount, List<GameObject> tables, List<GameObject> tvTables, List<GameObject> drawers)
    {
        List<GameObject> tablesAvailable = tables.Where(table => table.transform.childCount <= 10 - slotCount).ToList();
        List<GameObject> tvTablesAvailable = tvTables.Where(tvTable => tvTable.transform.childCount <= 6 - slotCount).ToList();
        List<GameObject> drawersAvailable = drawers.Where(drawer => drawer.transform.childCount <= 4 - slotCount).ToList();

        if (drawersAvailable.Count != 0 || tvTablesAvailable.Count != 0 || tablesAvailable.Count != 0)
        {
            GetComponent<Button>().interactable = true;
            _placableHasEnoughMoney = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
            _placableHasEnoughMoney = false;
        }
    }
}
