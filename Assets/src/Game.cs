using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Web;
using TMPro;
using System.IO;
using Newtonsoft.Json;
public class CityData
{
    public string name;
    public int size;
    public ProvinceData parent;
}
public class ProvinceData
{
    public string name;
    public CityData capital;
    public string country;
}
public class Game : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI ime;
    public TextMeshProUGUI drzava;
    public TextMeshProUGUI kapital;
    public RawImage map_mask;
    public Texture2D colourMap;
    public Texture2D fakeMap;

    [Header("Boja")]
    public List<Color> prefered_colors;
    public Color highlight;

    [Header("Vektori")]
    public Vector2 map_size;

    [Header("Klase")]
    [Header("Objekti")]
    public GameObject template;

    // Assign your colour-coded territory map here.
    Texture2D fakeMap_dum;
    Rect map_rect;
    Vector2 side_size;
    float scale_factor;
    bool reset_highlight;
    string last_zup = "";
    Color last_zup_clr;
    Dictionary<Color, CityData> cities;
    Dictionary<Color, List<Vector2Int>> pixels_zup;
    Dictionary<Color, ProvinceData> provinces;
    Dictionary<string, Dictionary<string, float>> ethnic_data;
    // We'll use this to quickly find a territory by its colour
    void reset_fake_dum()
    {
        for (int x = 0; x < fakeMap.width; x++)
        {
            for (int y = 0; y < fakeMap.height; y++)
            {
                fakeMap_dum.SetPixel(x, y, fakeMap.GetPixel(x, y));
            }
        }
    }
    void erase_last_zup()
    {
        List<Vector2Int> pix_data = pixels_zup[last_zup_clr];
        foreach (Vector2Int vector2Int in pix_data)
        {
            fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, Color.clear);
        }
        fakeMap_dum.Apply();
    }
    private void Start()
    {
        #region init
        cities = new Dictionary<Color, CityData>();
        pixels_zup = new Dictionary<Color, List<Vector2Int>>();
        provinces = new Dictionary<Color, ProvinceData>();
        fakeMap_dum = new Texture2D(fakeMap.width, fakeMap.height);
        ethnic_data = new Dictionary<string, Dictionary<string, float>>();
        #endregion
        #region province
        reset_fake_dum();
        fakeMap_dum.Apply();
        // Fetch the pixel data of the texture as a big block we can iterate through quickly.
        scale_factor = 1920 / Screen.width;
        int size_x = Mathf.RoundToInt(
            (
                (Screen.width * scale_factor)
                    -
                (map_size.x * scale_factor)
            )
            / 2);
        int size_y = Mathf.RoundToInt(
                (
                    (Screen.height * scale_factor)
                        -
                    (map_size.y * scale_factor)
                )
            / 2);
        side_size = new Vector2(size_x, size_y);
        map_rect = new Rect(side_size, map_size);

        string json = File.ReadAllText("Assets/provinces/provinces.json");
        dynamic provinces_json = JsonConvert.DeserializeObject(json);
        foreach (dynamic country in provinces_json.podaci)
        {
            foreach (dynamic province in country.podaci)
            {
                Color clr;
                ColorUtility.TryParseHtmlString(Convert.ToString(province.boja), out clr);

                ProvinceData data = new ProvinceData();
                data.name = province.ime;
                data.country = country.ime;

                CityData cityData = new CityData();
                int k = Convert.ToInt32(province.kapital);
                dynamic city_entry = province.gradovi[k];

                cityData.size = city_entry.velicina;
                cityData.name = city_entry.ime;
                cityData.parent = data;

                data.capital = cityData;
                provinces.Add(clr, data);
            }
        }
        foreach (Color p in provinces.Keys)
        {
            List<Vector2Int> pix_c = new List<Vector2Int>();
            for (int x = 0; x < colourMap.width; x++)
            {
                for (int y = 0; y < colourMap.height; y++)
                {
                    if (p == colourMap.GetPixel(x, y))
                    {
                        pix_c.Add(new Vector2Int(x, y));
                    }
                }

            }
            pixels_zup.Add(p, pix_c);
        }
        for (int x = 0; x < colourMap.width; x++)
        {
            for (int y = 0; y < colourMap.height; y++)
            {
                Color c = colourMap.GetPixel(x, y);
                if (cities.ContainsKey(c))
                {
                    CityData city = cities[c];

                }
            }
        }
        #endregion
        #region ethnic
        foreach (dynamic drzava in provinces_json.podaci)
        {
            string drz = Convert.ToString(drzava.ime);
            float sum = 0;
            Dictionary<string, float> TValue = new Dictionary<string, float>();
            bool r = true;
            for (int i = 0; i < Convert.ToInt32(drzava.etnicka_struktura_velicina); i++)
            {
                float f = float.Parse(Convert.ToString(drzava.etnicka_struktura_float[i]));
                TValue[Convert.ToString(drzava.etnicka_struktura_str[i])] = 1f - sum;
                sum += f;
                r = false;
            }
            //TValue["Ostali"]  = 1f - sum;
            ethnic_data.Add(drz, TValue);
        }
        #endregion
    }

    void Update()
    {
        // If no click this frame, abort. Nothing to do.

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse_pos = Input.mousePosition;
            if (map_rect.Contains(mouse_pos))
            {
                //for (int i = 0; i < template.transform.parent.childCount; i++)
                //{
                //    Transform child = template.transform.parent.GetChild(i);
                //    if (child.name != template.name)
                //    {
                //        Destroy(child.gameObject);
                //    }
                //}
                //print(new Vector2((int)mouse_pos.x - (int)side_size.x, (int)mouse_pos.y - (int)side_size.y));
                Color target = colourMap.GetPixel((int)mouse_pos.x - (int)side_size.x, (int)mouse_pos.y - (int)side_size.y);
                //print(target);
                if (provinces.ContainsKey(target))
                {
                    ProvinceData data = provinces[target];

                    ime.text = data.name;
                    if (last_zup != "" && last_zup != ime.text)
                    {
                        erase_last_zup();
                        map_mask.texture = fakeMap;

                    }

                    last_zup = ime.text;
                    last_zup_clr = target;
                    drzava.text = data.country;
                    kapital.text = data.capital.name;

                    List<Vector2Int> pix_data = pixels_zup[target];
                    foreach (Vector2Int vector2Int in pix_data)
                    {
                        fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, highlight);
                    }
                    fakeMap_dum.Apply();
                    map_mask.texture = fakeMap_dum;

                    //PieChart ethnic = new PieChart(template, prefered_colors);
                    //Dictionary<string, float> KV_pair = ethnic_data[data.country];
                    //bool first = true;
                    //foreach (string key in KV_pair.Keys)
                    //{
                    //    float val = KV_pair[key];
                    //    ethnic.AddEntry(key, val, first);
                    //    first = false;
                    //}
                }
            }
        }
    }
}