using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject _floor, _wallLeft, _wallRight, _wallMiddle, _boxPrefab, _shortGrid, _longGridVertical, _longGridHorizontal, _bigGrid, _shortGridWallLeft, _longGridVerticalWallLeft, _longGridHorizontalWallLeft, _shortGridWallMiddle, _longGridVerticalWallMiddle, _longGridHorizontalWallMiddle, _shortGridWallRight, _longGridVerticalWallRight, _longGridHorizontalWallRight;
    [SerializeField]
    private int _yRange, _xRange, _yRangeWall/*, _xRangeWall*/;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateFloorGrid();
        CreateWallGrid(_wallLeft, _shortGridWallLeft,_longGridVerticalWallLeft,_longGridHorizontalWallLeft);
        CreateWallGrid(_wallRight, _shortGridWallRight, _longGridVerticalWallRight, _longGridHorizontalWallRight);
        CreateWallGrid(_wallMiddle, _shortGridWallMiddle, _longGridVerticalWallMiddle, _longGridHorizontalWallMiddle);

    }
    private void CreateFloorGrid()
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
    private void CreateWallGrid(GameObject wall, GameObject shortGrid, GameObject longVGrid, GameObject longHGrid)
    {

        wall.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(_xRange/*Wall*/ / 2, _yRangeWall / 2);
       
        //this is for the shortgridwall
        for (int y = 1; y <= _yRangeWall; y++)
            for (int x = 1; x <= _xRange/*Wall*/; x++)
            {
                GameObject box = Instantiate(_boxPrefab, shortGrid.transform);

                box.transform.localScale = new Vector3(wall.transform.parent.localScale.x / _xRange, wall.transform.parent.localScale.y / _yRangeWall, .01f);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_yRange / 2f) + .5f), (y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRangeWall/2) + .5f), 0);

                box.layer = 11;
            }
        shortGrid.transform.position = wall.transform.position;
        shortGrid.transform.localRotation = wall.transform.localRotation;


        //this is for the longverticalgridwall
        for (int y = 2; y <= _yRangeWall; y++)
            for (int x = 1; x <= _xRange; x++)
            {

                GameObject box = Instantiate(_boxPrefab, longVGrid.transform);

                box.transform.localScale = new Vector3(wall.transform.parent.localScale.x / _xRange, wall.transform.parent.localScale.y / _yRangeWall, .01f);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_xRange / 2f) + .5f), (y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRangeWall / 2f) + .5f) - box.transform.localScale.y / 2, 0);

                box.layer = 13;
            }
        longVGrid.transform.position = wall.transform.position;
        longVGrid.transform.localRotation = wall.transform.localRotation;


        //this is for the longhorizontalgridwall
        for (int y = 1; y <= _yRangeWall; y++)
            for (int x = 2; x <= _xRange; x++)
            {

                GameObject box = Instantiate(_boxPrefab, longHGrid.transform);

                box.transform.localScale = new Vector3(wall.transform.parent.localScale.x / _xRange, wall.transform.parent.localScale.y / _yRangeWall, .01f);

                box.transform.position = new Vector3((x * box.transform.localScale.x) - box.transform.localScale.x * ((_xRange / 2f) + .5f) - box.transform.localScale.x / 2, (y * box.transform.localScale.y) - box.transform.localScale.y * ((_yRangeWall / 2) + .5f), 0);

                box.layer = 12;
            }
        longHGrid.transform.position = wall.transform.position;
        longHGrid.transform.localRotation = wall.transform.localRotation;
    }

}
