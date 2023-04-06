using Dossamer.PanZoom;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject SearchCanvas;
    public GameObject PlantDetailCanvas;
    public PanZoomBehavior PanZoomBehavior;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip buttonAudio;
    [SerializeField]
    private GameObject noDataFound;
    public void AudioClick()
    {
        source.PlayOneShot(buttonAudio);
    }


    public void BackToSearch()
    {
        PanZoomBehavior.enabled = false;
        noDataFound.SetActive(false);
        SearchCanvas.SetActive(true);
    }

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
