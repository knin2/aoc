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
public class CountryData
{
    public string name, president, capital;
    public int population;
    public Dictionary<string, int> odnosi_dict;
    public List<ProvinceData> provinces;
    public EthnicData ethnicData;
}
public class EthnicData
{
    public int size;
    public string drzava;
    public List<string> ljudi;
    public List<float> postotci;
    public List<int> stanovnici;
}
public class Game : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI ime;
    public TextMeshProUGUI drzava;
    public TextMeshProUGUI ime_zupanije;
    public TextMeshProUGUI kapital;

    public TextMeshProUGUI side_r_drzava;
    public TextMeshProUGUI side_drzava;
    public TextMeshProUGUI side_predsednik;
    public TextMeshProUGUI side_populacija;
    public TextMeshProUGUI side_kapital;
    public TextMeshProUGUI odnosi;
    public RawImage map_mask;
    public Texture2D colourMap;
    public Texture2D fakeMap;

    [Header("Boja")]
    public List<Color> prefered_colors;
    public Gradient odnosi_gradient;
    public Color highlight;

    [Header("Vektori")]
    public Vector2 map_size;

    [Header("Objekti")]
    public Transform ime_holder;
    public Transform ime_holder_zup;
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
    Dictionary<string, CountryData> countries;
    Dictionary<string, EthnicData> ethnic_data;
    Dictionary<string, EthnicData> ethnic_data_zup;
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
        ethnic_data = new Dictionary<string, EthnicData>();
        ethnic_data_zup = new Dictionary<string, EthnicData>();
        countries = new Dictionary<string, CountryData>();
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

                EthnicData eth_data = new EthnicData();
                eth_data.drzava = data.country;
                dynamic demo = province.demografija;
                eth_data.size = Convert.ToInt32(demo.etnicka_struktura_velicina);
                eth_data.stanovnici = new List<int>();
                eth_data.postotci = new List<float>();
                eth_data.ljudi = new List<string>();

                int zup_populacija = Convert.ToInt32(demo.populacija);
                for (int i = 0; i < eth_data.size; i++)
                {
                    float f = float.Parse(Convert.ToString(demo.etnicka_struktura_float[i]));
                    string s = Convert.ToString(demo.etnicka_struktura_str[i]);
                    eth_data.postotci.Add(f);
                    eth_data.ljudi.Add(s);
                    eth_data.stanovnici.Add(Mathf.RoundToInt(f * zup_populacija));
                }

                ethnic_data_zup.Add(data.name, eth_data);
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
            int pop = Convert.ToInt32(drzava.demografija.populacija);
            print(pop);

            CountryData countryData = new CountryData();
            countryData.population = pop;
            countryData.president = drzava.prezident;
            countryData.odnosi_dict = new Dictionary<string, int>();

            EthnicData data = new EthnicData();
            data.postotci = new List<float>();
            data.ljudi = new List<string>();
            data.stanovnici = new List<int>();

            for (int i = 0; i < Convert.ToInt32(drzava.demografija.etnicka_struktura_velicina); i++)
            {
                float f = float.Parse(Convert.ToString(drzava.demografija.etnicka_struktura_float[i]));
                string s = Convert.ToString(drzava.demografija.etnicka_struktura_str[i]);
                data.postotci.Add(f);
                data.ljudi.Add(s);
                data.stanovnici.Add(Mathf.RoundToInt(f * pop));
            }
            for (int i = 0; i < Convert.ToInt32(drzava.odnosi_velicina); i++)
            {
                int j = i * 2;
                string o_drz = drzava.odnosi[j];
                int o_data = drzava.odnosi[j + 1];
                countryData.odnosi_dict[o_drz] = o_data;
            }
            //TValue["Ostali"]  = 1f - sum;
            data.size = data.postotci.Count;
            data.drzava = drz;

            countryData.capital = drzava.kapital;
            countryData.name = drzava.ime;
            countryData.ethnicData = data;

            countries[Convert.ToString(drzava.ime)] = countryData;
            ethnic_data.Add(drz, data);
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
                if (target == Color.black)
                {
                    mouse_pos += new Vector2(10, 10);
                }
                //print(target);
                if (provinces.ContainsKey(target))
                {
                    ProvinceData data = provinces[target];
                    print(data.country);
                    CountryData countryData = countries[data.country];
                    ime.text = data.name;
                    if (last_zup != "" && last_zup != ime.text)
                    {
                        erase_last_zup();
                        map_mask.texture = fakeMap;

                    }

                    last_zup = ime.text;
                    last_zup_clr = target;
                    drzava.text = data.country;
                    side_r_drzava.text = data.country;
                    side_predsednik.text = countryData.president;
                    side_drzava.text = countryData.name;
                    side_kapital.text = countryData.capital;
                    side_populacija.text = string.Format("{0:N0}", countryData.population);
                    int o_d = countryData.odnosi_dict["Hrvatska"];
                    odnosi.text = o_d > 0 ? "+" + o_d : "-" + o_d;
                    float o_d_grad = (o_d + 100) / 200;
                    odnosi.color = odnosi_gradient.Evaluate(o_d_grad);
                    kapital.text = data.capital.name;
                    ime_zupanije.text = data.name;

                    List<Vector2Int> pix_data = pixels_zup[target];
                    foreach (Vector2Int vector2Int in pix_data)
                    {
                        fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, highlight);
                    }
                    fakeMap_dum.Apply();
                    map_mask.texture = fakeMap_dum;
                    EthnicData data_eth = ethnic_data[data.country];
                    EthnicData data_eth_zup = ethnic_data_zup[data.name];
                    for (int idxi = 0; idxi < data_eth.size; idxi++)
                    {
                        ime_holder.GetChild(idxi).GetComponent<TextMeshProUGUI>().text = String.Format("{0}: {1}%, {2:N0}", data_eth.ljudi[idxi], data_eth.postotci[idxi] * 100f, data_eth.stanovnici[idxi]);
                        ime_holder_zup.GetChild(idxi).GetComponent<TextMeshProUGUI>().text = String.Format("{0}: {1}%, {2:N0}", data_eth_zup.ljudi[idxi], data_eth_zup.postotci[idxi] * 100f, data_eth_zup.stanovnici[idxi]);
                    }
                }
            }
        }
    }
}