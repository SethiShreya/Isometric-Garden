using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

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
    private Dictionary<Vector3, RetrieveData> dataCorrespondingToPos= new Dictionary<Vector3, RetrieveData>();
    [SerializeField]
    private GetData getData;
    public AudioSource source;
    public AudioClip plantingClip;
    [SerializeField]
    private float yPos=0.6f;

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
        dataCorrespondingToPos.Add(currentPos, data);
        Debug.Log("Data stored in dict");
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
            position.y = yPos;
            source.PlayOneShot(plantingClip);
            Instantiate(plantPrefab, position, Quaternion.identity);
            tilemap.AddTileFlags(cellpos, TileFlags.LockAll);
            uiManager.EnableSearch();
            // stores the values got from getdata
            currentPos = cellpos;
        }
        else if (tilemap.HasTile(cellpos) && tilemap.GetTileFlags(cellpos) == TileFlags.LockAll)
        {
            // clicks the already instantiated object so retrieve the values
            if (dataCorrespondingToPos.ContainsKey(cellpos))
            {
                Debug.Log("plant present at the point");
                getData.PrintData(dataCorrespondingToPos[cellpos]);
            }
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
