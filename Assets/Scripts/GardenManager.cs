using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GardenManager : MonoBehaviour
{
    public GridLayout gridLayout;
    private Grid grid;
    public Tilemap tilemap;
    public TileBase plantTile;
    public GameObject plantPrefab;
    public LayerMask layerMask;
    public UIManager uiManager;
    private Vector3 currentPos;


    private void Awake()
    {
     grid= gridLayout.gameObject.GetComponent<Grid>();   
    }

    


    private void GetMousePosition()
    {
        //Debug.Log("Get mouse positon funciton called");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            SnapCoordinateToGrid(hit.point);
        }
    }

    public void GetValuesToStore(RetrieveData data)
    {
        // store the value to the dictionary with current pos
    }
    

    private void SnapCoordinateToGrid(Vector3 position)
    {
        //Debug.Log("Snap to coordinate function called");
        Vector3Int cellpos = tilemap.WorldToCell(position);
        Debug.Log(cellpos);
        cellpos.z = 0;
        Debug.Log("Cell" + cellpos);
        //Debug.Log(tilemap.GetTileFlags(cellpos));

        if (tilemap.HasTile(cellpos) && tilemap.GetTileFlags(cellpos)==TileFlags.LockColor)
        {
            position= grid.GetCellCenterWorld(cellpos);
            Debug.Log(position);
            Instantiate(plantPrefab, position, Quaternion.identity);
            tilemap.AddTileFlags(cellpos, TileFlags.LockAll);
            uiManager.EnableSearch();
            // stores the values got from getdata

        }
        else if (tilemap.HasTile(cellpos) && tilemap.GetTileFlags(cellpos) == TileFlags.LockAll)
        {
            // clicks the already instantiated object so retrieve the values
        }
        else
        {
            Debug.Log("Either no tile or not empty");
        }

    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse clicked");
            GetMousePosition();
        }
    }
    }
