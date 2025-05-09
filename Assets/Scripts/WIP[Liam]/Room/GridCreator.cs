using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject _floor, _wallLeft, _wallRight, _wallMiddle, _boxPrefab, _shortGrid, _longGridVertical, _longGridHorizontal, _bigGrid;
    [SerializeField]
    private int _yRange, _xRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _floor.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(_xRange / 2, _yRange / 2);

        //this is for the shortgrid
        for (int y = 1; y <= _yRange; y++)
            for (int x = 1; x <= _xRange; x++)
            {
                GameObject box = Instantiate(_boxPrefab, _shortGrid.transform);

                box.transform.localScale = new Vector3(_floor.transform.parent.localScale.x / _xRange, _floor.transform.parent.localScale.y / _yRange, _floor.transform.parent.localScale.y / _yRange);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_yRange / 2f) + .5f), 0, (y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRange / 2f) + .5f));

                box.layer = 7;
            }

        //this is for the longverticalgrid
        for (int y = 2; y <= _yRange; y++)
            for (int x = 1; x <= _xRange; x++)
            {

                GameObject box = Instantiate(_boxPrefab, _longGridVertical.transform);

                box.transform.localScale = new Vector3(_floor.transform.parent.localScale.x / _xRange, _floor.transform.parent.localScale.y / _yRange, _floor.transform.parent.localScale.y / _yRange);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_yRange / 2f) + .5f), 0, (y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRange / 2f) + .5f) - box.transform.localScale.y / 2);
                
                box.layer = 9;
            }

        //this is for the longhorizontalgrid
        for (int y = 1; y <= _yRange; y++)
            for (int x = 2; x <= _xRange; x++)
            {

                GameObject box = Instantiate(_boxPrefab, _longGridHorizontal.transform);

                box.transform.localScale = new Vector3(_floor.transform.parent.localScale.x / _xRange, _floor.transform.parent.localScale.y / _yRange, _floor.transform.parent.localScale.y / _yRange);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_yRange / 2f) + .5f) - box.transform.localScale.y / 2, 0, (y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRange / 2f) + .5f));

                box.layer = 8;
            }

        //this is for the biggrid
        for (int y = 2; y <= _yRange; y++)
            for (int x = 2; x <= _xRange; x++)
            {

                GameObject box = Instantiate(_boxPrefab, _bigGrid.transform);

                box.transform.localScale = new Vector3(_floor.transform.parent.localScale.x / _xRange, _floor.transform.parent.localScale.y / _yRange, _floor.transform.parent.localScale.y / _yRange);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_yRange / 2f) + .5f) - box.transform.localScale.y / 2, 0, ((y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRange / 2f) + .5f) - box.transform.localScale.y / 2));

                box.layer = 6;
            }

    }
}
