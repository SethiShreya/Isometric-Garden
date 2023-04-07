using Dossamer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.UI;

[System.Serializable]
public class PlantDetails
{
    public int id;
    public string common_name;
    public string[] scientific_name;
    public string[] other_name;
    public string family;
    public string[] origin;
    public string type;
    public string dimension;
    public string cycle;
    public string watering;
    public string[] attracts;
    public string[] propagation;
    public Hardiness hardiness;
    public string hardiness_location_image;
    public string[] sunlight;
    public string[] soil;
    public string growth_rate;
    public string maintenance;
    public bool drought_tolerant;
    public bool salt_tolerant;
    public bool thorny;
    public bool invasive;
    public bool tropical;
    public bool indoor;
    public string care_level;
    public string pest_susceptibility;
    public string pest_susceptibility_api;
    public bool flowers;
    public string flowering_season;
    public string flower_color;
    public bool cones;
    public bool fruits;
    public bool edible_fruit;
    public string edible_fruit_taste_profile;
    public string fruit_nutritional_value;
    public string[] fruit_color;
    public string fruiting_season;
    public string harvest_season;
    public string harvest_method;
    public bool leaf;
    public string[] leaf_color;
    public bool edible_leaf;
    public string edible_leaf_taste_profile;
    public string leaf_nutritional_value;
    public bool cuisine;
    public string cuisine_list;
    public string medicinal;
    public string medicinal_use;
    public string medicinal_method;
    public string poisonous_to_humans;
    public string poison_effects_to_humans;
    public string poison_to_humans_cure;
    public string poisonous_to_pets;
    public string poison_effects_to_pets;
    public string poison_to_pets_cure;
    public string rare;
    public string rare_level;
    public string endangered;
    public string endangered_level;
    public string description;
    public string problem;
    public DefaultImage default_image;


    

}

[System.Serializable]
public class Hardiness
{
    public string min;
    public string max;
}

[System.Serializable]
public class DefaultImage
{
    public int license;
    public string license_name;
    public string license_url;
    public string original_url;
    public string regular_url;
    public string medium_url;
    public string small_url;
    public string thumbnail;
}


// create class with all the values "RetrieveData"

public class RetrieveData
{
    public string name;
    public string s_Name;
    public string family;
    public string origin;
    public string watering;
    public string sunlight;
    public string soil;
    public bool tropical;
    public bool fruits;
    public string growth_rate;
    public string imageUrl;

    public RetrieveData(string name, string sname, string family, string origin, string watering, string sunlight,string soil,  bool tropical, bool fruits, string growth_rate, string imageUrl)
    {
        this.name = name;
        this.s_Name = sname;
        this.family = family;
        this.origin = origin;
        this.watering = watering;
        this.sunlight = sunlight;
        this.soil = soil;
        this.tropical = tropical;
        this.fruits = fruits;
        this.growth_rate = growth_rate;
        this.imageUrl = imageUrl;
    }
}


public class GetData : MonoBehaviour
{
    private const string api_key = "sk-SALL641c009636ff5303";
    //public string query = "monstera";
    public UnityEngine.UI.RawImage plantImage;
    //public MeshRenderer cube;
    public TextMeshProUGUI[] textMeshes;
    public TMP_InputField input;
    public GameObject SearchCanvas;
    public GameObject PlantDetailCanvas;
    public GameObject NoDataCanvas;
    public AudioSource source;
    public AudioClip errorClip;
    [SerializeField]
    private GardenManager gardenManager;
    public TextMeshProUGUI errorString;



    public void Search()
    {
        StartCoroutine(SearchRoutine());
    }

    public IEnumerator SearchRoutine()
    {
       string url = $"https://perenual.com/api/species-list?key={api_key}&q={input.text}";
        Debug.Log("Input field " + input.text);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            JObject jsonObject = JObject.Parse(json);
            if (jsonObject["data"].Count() > 0)
            {
                long id = (long)jsonObject["data"][0]["id"];
                StartCoroutine(GetDetails(id));
                Debug.Log("String searched "+input.text);
            }
            else
            {
                SearchCanvas.SetActive(false);
                source.PlayOneShot(errorClip);
                errorString.text =input.text;
                NoDataCanvas.gameObject.SetActive(true);
                Debug.Log("Specified Plant: \"" + input.text + " \" not found, try searching for something else");
            }  
        }
    }

    public void PrintData(RetrieveData data)
    {
        SearchCanvas.SetActive(false);
        PlantDetailCanvas.SetActive(true);
        textMeshes[0].text = data.name;
        textMeshes[1].text = data.s_Name;
        textMeshes[2].text = data.family;
        textMeshes[3].text = data.origin;
        textMeshes[4].text = data.watering;
        textMeshes[5].text = data.sunlight;
        textMeshes[6].text = data.soil;
        textMeshes[7].text = data.tropical.ToString();
        textMeshes[8].text = data.fruits.ToString();
        textMeshes[9].text = data.growth_rate;

        StartCoroutine(GetImage(data.imageUrl));
    }

    IEnumerator GetDetails(long id)
    {
        
        string detailsUrl = $"https://perenual.com/api/species/details/{id}?key={api_key}";
        UnityWebRequest www= UnityWebRequest.Get(detailsUrl);

        yield return www.SendWebRequest();
        Debug.Log("get request send");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error downloading detials: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            PlantDetails plants = JsonUtility.FromJson<PlantDetails>(json);
            RetrieveData data=new RetrieveData(plants.common_name ?? "N/A",
               plants.scientific_name.Length > 0 ? plants.scientific_name[0] : "N/A",
               plants.family ?? "N/A",
               plants.origin.Length > 0 ? plants.origin[0] : "N/A",
               plants.watering ?? "N/A",
               plants.sunlight.Length > 0 ? plants.sunlight[0] : "N/A",
               plants.soil.Length>0?plants.soil[0]:"N/A",
               plants.tropical,
               plants.fruits,
               plants.growth_rate ?? "N/A",
               plants.default_image.original_url.ToString()
                );
            PrintData(data);
            gardenManager.GetValuesToStore(data);           
        }
    }

    IEnumerator GetImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error downloading image: " + www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), UnityEngine.Vector2.zero);
            plantImage.texture = texture;
        }
    }
}
