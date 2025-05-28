using System.Drawing;
using System.Linq;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;


public class PlayerPointer : MonoBehaviour
{
    public bool CanMove;
    public GameObject[] Room;
    [SerializeField]
    private float _speed;
    private Vector2 _moveVector;
    public int PlayerIndex;
    public void OnMove(InputValue value)
    {
        _moveVector = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            transform.localPosition += _speed * Time.fixedDeltaTime * new Vector3(_moveVector.x * transform.right.x, _moveVector.y * transform.up.y, _moveVector.y * transform.up.z).normalized;
            CheckOutOfBounds();
        }

    }

    private void CheckOutOfBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPosition.y > Screen.height) 
        {
            screenPosition.y = Screen.height;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.y < 0)
        {
            screenPosition.y = 0;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
        
        if (screenPosition.x > Screen.width)
        {
            screenPosition.x = Screen.width;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x < 0)
        {
            screenPosition.x = 0;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }

    public void StayOnFloor(int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        Vector3[] floorMeshVertices = Room[0].transform.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] leftFloorMeshVertices = floorMeshVertices.Where(vert => vert.x < 0).ToArray();

        Vector3 correctVert0Position = Camera.main.WorldToScreenPoint(new Vector3(leftFloorMeshVertices[0].x, leftFloorMeshVertices[0].z, leftFloorMeshVertices[0].y));
        Vector3 correctVert1Position = Camera.main.WorldToScreenPoint(new Vector3(leftFloorMeshVertices[1].x, leftFloorMeshVertices[1].z, leftFloorMeshVertices[1].y));

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        float verticesDifference = Mathf.Abs(correctVert1Position.y - correctVert0Position.y);
        float pointerDifference = Mathf.Abs(screenPosition.y - correctVert0Position.y) / verticesDifference;

        // left bound
        transform.GetChild(0).position = new Vector3(-Room[0].transform.localScale.x/2, 0,Mathf.Lerp(leftFloorMeshVertices[0].y, leftFloorMeshVertices[1].y, pointerDifference));
        
        // right bound
        transform.GetChild(1).position = new Vector3(Room[0].transform.localScale.x / 2, 0, Mathf.Lerp(leftFloorMeshVertices[0].y, leftFloorMeshVertices[1].y, pointerDifference));

        
        //bounding blub
        if (screenPosition.y > correctVert0Position.y - keptAwayHorizontalBounds)
        {
            screenPosition.y = correctVert0Position.y - keptAwayHorizontalBounds;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.y < correctVert1Position.y + keptAwayHorizontalBounds)
        {
            screenPosition.y = correctVert1Position.y + keptAwayHorizontalBounds;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x > Camera.main.WorldToScreenPoint(transform.GetChild(0).position).x - keptAwayVerticalBounds)
        {
            screenPosition.x = Camera.main.WorldToScreenPoint(transform.GetChild(0).position).x - keptAwayVerticalBounds;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x < Camera.main.WorldToScreenPoint(transform.GetChild(1).position).x + keptAwayVerticalBounds)
        {
            screenPosition.x = Camera.main.WorldToScreenPoint(transform.GetChild(1).position).x + keptAwayVerticalBounds;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }

    public void StayOnWalls(int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        Vector3 topMiddleWallInScreenSpace = Camera.main.WorldToScreenPoint(new Vector3(0, Room[2].transform.position.y + Room[2].transform.localScale.y / 2, Room[2].transform.position.z));
        Vector3 bottomMiddleWallInScreenSpace = Camera.main.WorldToScreenPoint(new Vector3(0, Room[2].transform.position.y - Room[2].transform.localScale.y / 2, Room[2].transform.position.z));
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);


        float sideDifference = Mathf.Abs(topMiddleWallInScreenSpace.y - bottomMiddleWallInScreenSpace.y);
        float pointerDifference = Mathf.Abs(screenPosition.y - bottomMiddleWallInScreenSpace.y) / sideDifference;

        if (screenPosition.y <= bottomMiddleWallInScreenSpace.y)
        {
            pointerDifference = 0;
        }

        // right bound
        transform.GetChild(2).GetChild(0).position = new Vector3(-Room[2].transform.localScale.x / 2, Mathf.Lerp(Room[2].transform.position.y - Room[2].transform.localScale.y / 2, Room[2].transform.position.y + Room[2].transform.localScale.y / 2,pointerDifference), -Room[2].transform.localScale.x / 2);

        // left bound
        transform.GetChild(2).GetChild(1).position = new Vector3(Room[2].transform.localScale.x / 2, Mathf.Lerp(Room[2].transform.position.y - Room[2].transform.localScale.y / 2, Room[2].transform.position.y + Room[2].transform.localScale.y / 2, pointerDifference), -Room[2].transform.localScale.x / 2);


        if (screenPosition.x < Camera.main.WorldToScreenPoint(transform.GetChild(2).GetChild(1).position).x)
        {
            StayOnLeftWall(screenPosition, bottomMiddleWallInScreenSpace, keptAwayHorizontalBounds, keptAwayVerticalBounds);
        }
        else
        if (screenPosition.x > Camera.main.WorldToScreenPoint(transform.GetChild(2).GetChild(0).position).x)
        {
            StayOnRightWall(screenPosition, bottomMiddleWallInScreenSpace, keptAwayHorizontalBounds, keptAwayVerticalBounds);
        }
        else
            //works
            StayOnMiddletWall(screenPosition, topMiddleWallInScreenSpace, bottomMiddleWallInScreenSpace, keptAwayHorizontalBounds, keptAwayVerticalBounds);
    }

    private void StayOnLeftWall(Vector3 screenPosition, Vector3 bottomOfMiddleInScreenSpace, int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        Vector3 topLeftCorner = new Vector3(Room[1].transform.position.x, Room[1].transform.position.y + Room[1].transform.localScale.y / 2, Room[1].transform.position.z + Room[1].transform.localScale.z / 2);
        Vector3 topRightCorner = new Vector3(Room[1].transform.position.x, Room[1].transform.position.y + Room[1].transform.localScale.y / 2, Room[1].transform.position.z - Room[1].transform.localScale.z / 2);
        Vector3 bottomLeftCorner = new Vector3(Room[1].transform.position.x, Room[1].transform.position.y - Room[1].transform.localScale.y / 2, Room[1].transform.position.z + Room[1].transform.localScale.z / 2);
        Vector3 bottomRightCorner = new Vector3(Room[1].transform.position.x, Room[1].transform.position.y - Room[1].transform.localScale.y / 2, Room[1].transform.position.z - Room[1].transform.localScale.z / 2);

        Vector3 topLeftCornerScreen = Camera.main.WorldToScreenPoint(topLeftCorner);
        Vector3 topRightCornerScreen = Camera.main.WorldToScreenPoint(topRightCorner);
        Vector3 bottomLeftCornerScreen = Camera.main.WorldToScreenPoint(bottomLeftCorner);
        Vector3 bottomRightCornerScreen = Camera.main.WorldToScreenPoint(bottomRightCorner);

        //print(topLeftCorner + " " + topRightCorner + " " + bottomLeftCorner + " " + bottomRightCorner);


        float sideDifference = Mathf.Abs(topLeftCornerScreen.x - bottomLeftCornerScreen.x);
        float pointerSideDifference = Mathf.Abs(screenPosition.x - bottomLeftCornerScreen.x) / sideDifference;

        float topDifference = Mathf.Abs(topLeftCornerScreen.y - topRightCornerScreen.y);
        float pointerTopDifference = Mathf.Abs(screenPosition.y - topRightCornerScreen.y) / topDifference;

        float bottomDifference = Mathf.Abs(bottomLeftCornerScreen.y - bottomRightCornerScreen.y);
        float pointerBottomDifference = Mathf.Abs(screenPosition.y - bottomRightCornerScreen.y) / bottomDifference;

        int collisionCorrecter;
        //print(pointerBottomDifference + " " + pointerSideDifference + " " + pointerTopDifference);
        if (screenPosition.y >= bottomOfMiddleInScreenSpace.y)
        {
            pointerBottomDifference = 0;
            collisionCorrecter = 0;
        }
        else
            collisionCorrecter = 1;


        // side
        transform.GetChild(3).GetChild(2).position = Camera.main.ScreenToWorldPoint(Vector3.Lerp(bottomLeftCornerScreen, topLeftCornerScreen, pointerSideDifference));

        // bottom
        transform.GetChild(3).GetChild(1).position = Camera.main.ScreenToWorldPoint(Vector3.Lerp(bottomRightCornerScreen, bottomLeftCornerScreen, pointerBottomDifference));

        // top
        transform.GetChild(3).GetChild(0).position = Camera.main.ScreenToWorldPoint(Vector3.Lerp(topRightCornerScreen, topLeftCornerScreen, pointerTopDifference));

        //bounding blub

        if (screenPosition.y < Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(2).position).y + keptAwayVerticalBounds +60)
        {
            screenPosition.y = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(2).position).y + keptAwayVerticalBounds +60;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x > Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(1).position).x - (keptAwayHorizontalBounds - 20) * collisionCorrecter)
        {
            screenPosition.x = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(1).position).x - (keptAwayHorizontalBounds - 20) * collisionCorrecter;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x < Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(0).position).x + keptAwayHorizontalBounds+45)
        {
            screenPosition.x = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(0).position).x + keptAwayHorizontalBounds+45;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }


    private void StayOnMiddletWall(Vector3 screenPosition, Vector3 topInScreenSpace,Vector3 bottomInScreenSpace, int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        
        if (screenPosition.y > topInScreenSpace.y - keptAwayHorizontalBounds - 45)
        {
            screenPosition.y = topInScreenSpace.y - keptAwayHorizontalBounds - 45;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.y < bottomInScreenSpace.y + keptAwayHorizontalBounds)
        {
            screenPosition.y = bottomInScreenSpace.y + keptAwayHorizontalBounds;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }


    private void StayOnRightWall(Vector3 screenPosition, Vector3 bottomOfMiddleInScreenSpace, int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        Vector3 topLeftCorner = new Vector3(Room[3].transform.position.x, Room[3].transform.position.y + Room[3].transform.localScale.y / 2, Room[3].transform.position.z + Room[3].transform.localScale.z / 2);
        Vector3 topRightCorner = new Vector3(Room[3].transform.position.x, Room[3].transform.position.y + Room[3].transform.localScale.y / 2, Room[3].transform.position.z - Room[3].transform.localScale.z / 2);
        Vector3 bottomLeftCorner = new Vector3(Room[3].transform.position.x, Room[3].transform.position.y - Room[3].transform.localScale.y / 2, Room[3].transform.position.z + Room[3].transform.localScale.z / 2);
        Vector3 bottomRightCorner = new Vector3(Room[3].transform.position.x, Room[3].transform.position.y - Room[3].transform.localScale.y / 2, Room[3].transform.position.z - Room[3].transform.localScale.z / 2);

        Vector3 topLeftCornerScreen = Camera.main.WorldToScreenPoint(topLeftCorner);
        Vector3 topRightCornerScreen = Camera.main.WorldToScreenPoint(topRightCorner);
        Vector3 bottomLeftCornerScreen = Camera.main.WorldToScreenPoint(bottomLeftCorner);
        Vector3 bottomRightCornerScreen = Camera.main.WorldToScreenPoint(bottomRightCorner);

        //print(topLeftCorner + " " + topRightCorner + " " + bottomLeftCorner + " " + bottomRightCorner);


        float sideDifference = Mathf.Abs(topLeftCornerScreen.x - bottomLeftCornerScreen.x);
        float pointerSideDifference = Mathf.Abs(screenPosition.x - bottomLeftCornerScreen.x) / sideDifference;

        float topDifference = Mathf.Abs(topLeftCornerScreen.y - topRightCornerScreen.y);
        float pointerTopDifference = Mathf.Abs(screenPosition.y - topRightCornerScreen.y) / topDifference;

        float bottomDifference = Mathf.Abs(bottomLeftCornerScreen.y - bottomRightCornerScreen.y);
        float pointerBottomDifference = Mathf.Abs(screenPosition.y - bottomRightCornerScreen.y) / bottomDifference;

        int collisionCorrecter;
        //print(pointerBottomDifference + " " + pointerSideDifference + " " + pointerTopDifference);
        if (screenPosition.y >= bottomOfMiddleInScreenSpace.y)
        {
            pointerBottomDifference = 0;
            collisionCorrecter = 0;
        }
        else
            collisionCorrecter = 1;


        // side
        transform.GetChild(3).GetChild(2).position = Camera.main.ScreenToWorldPoint(Vector3.Lerp(bottomLeftCornerScreen, topLeftCornerScreen, pointerSideDifference));

        // bottom
        transform.GetChild(3).GetChild(1).position = Camera.main.ScreenToWorldPoint(Vector3.Lerp(bottomRightCornerScreen, bottomLeftCornerScreen, pointerBottomDifference));

        // top
        transform.GetChild(3).GetChild(0).position = Camera.main.ScreenToWorldPoint(Vector3.Lerp(topRightCornerScreen, topLeftCornerScreen, pointerTopDifference));

        //bounding blub

        if (screenPosition.y < Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(2).position).y + keptAwayVerticalBounds + 60)
        {
            screenPosition.y = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(2).position).y + keptAwayVerticalBounds + 60;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x < Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(1).position).x + (keptAwayHorizontalBounds - 20) * collisionCorrecter)
        {
            screenPosition.x = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(1).position).x + (keptAwayHorizontalBounds - 20) * collisionCorrecter;
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (screenPosition.x > Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(0).position).x - (keptAwayHorizontalBounds + 45))
        {
            screenPosition.x = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(0).position).x - (keptAwayHorizontalBounds + 45);
            transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
        }

    }
    public void OnComfirm()
    {
        if (TryGetComponent(out SabotageTool sabotage) && sabotage.enabled)
        {
            sabotage.OnComfirm(); // Now calls the no-parameter version
        }
    }
}
