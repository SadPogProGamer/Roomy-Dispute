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

        //print(leftFloorMeshVertices[1] + " " + leftFloorMeshVertices[0] + " "+pointerDifference);

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
        Vector3[] middleWallMeshVertices = Room[2].transform.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] leftMiddleWallMeshVertices = middleWallMeshVertices.Where(vert => vert.x < 0).ToArray();

        Vector3 correctVert0Position = Camera.main.WorldToScreenPoint(new Vector3(leftMiddleWallMeshVertices[0].x, leftMiddleWallMeshVertices[0].z, leftMiddleWallMeshVertices[0].y));
        Vector3 correctVert1Position = Camera.main.WorldToScreenPoint(new Vector3(leftMiddleWallMeshVertices[1].x, leftMiddleWallMeshVertices[1].z, leftMiddleWallMeshVertices[1].y));

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        float verticesDifference = Mathf.Abs(correctVert1Position.y - correctVert0Position.y);
        float pointerDifference = Mathf.Abs(screenPosition.y - correctVert0Position.y) / verticesDifference;

        print(leftMiddleWallMeshVertices[1] + " " + leftMiddleWallMeshVertices[0] + " "+pointerDifference);

        // right bound
        transform.GetChild(2).GetChild(0).position = new Vector3(-Room[2].transform.localScale.x / 2, Mathf.Lerp(leftMiddleWallMeshVertices[0].y+ Room[2].transform.localScale.x / 2, leftMiddleWallMeshVertices[1].y+ Room[2].transform.localScale.x / 2, pointerDifference), -Room[2].transform.localScale.x / 2);

        // left bound
        transform.GetChild(2).GetChild(1).position = new Vector3(Room[2].transform.localScale.x / 2, Mathf.Lerp(leftMiddleWallMeshVertices[0].y + Room[2].transform.localScale.x / 2, leftMiddleWallMeshVertices[1].y + Room[2].transform.localScale.x / 2, pointerDifference), -Room[2].transform.localScale.x / 2);


        if (screenPosition.x < Camera.main.WorldToScreenPoint(transform.GetChild(2).GetChild(1).position).x)
        {
            StayOnLeftWall(keptAwayHorizontalBounds, keptAwayVerticalBounds);
        }
        else
        if (screenPosition.x > Camera.main.WorldToScreenPoint(transform.GetChild(2).GetChild(0).position).x)
        {
            StayOnRightWall(keptAwayHorizontalBounds, keptAwayVerticalBounds);
        }
        else
            StayOnMiddletWall(screenPosition, correctVert0Position, correctVert1Position, keptAwayHorizontalBounds, keptAwayVerticalBounds);
    }

    private void StayOnLeftWall(int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        print("Left");
    }
    private void StayOnMiddletWall(Vector3 screenPosition, Vector3 correctVert0Position, Vector3 correctVert1Position, int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
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
    }
    private void StayOnRightWall(int keptAwayHorizontalBounds, int keptAwayVerticalBounds)
    {
        print("Right");

    }
    public void OnComfirm()
    {
        if (TryGetComponent(out SabotageTool sabotage) && sabotage.enabled)
        {
            sabotage.OnComfirm(); // Now calls the no-parameter version
        }
    }
}
