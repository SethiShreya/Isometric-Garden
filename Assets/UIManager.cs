using Dossamer.PanZoom;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject SearchCanvas;
    public GameObject PlantDetailCanvas;
    public PanZoomBehavior PanZoomBehavior;

    public void EnableSearch()
    {
        PanZoomBehavior.enabled = false;
        SearchCanvas.SetActive(true);
    }
    
    public void DisableSearch()
    {
        PanZoomBehavior.enabled = true;
        SearchCanvas.SetActive(false);
    }

    public void ShowDetails()
    {
        PanZoomBehavior.enabled = false;
        PlantDetailCanvas.SetActive(true);
    }
    
    public void HideDetails()
    {
        PanZoomBehavior.enabled = true;
        PlantDetailCanvas.SetActive(false);
    }
}
