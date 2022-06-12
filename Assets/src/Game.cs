using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

#region klase
[System.Serializable]
public class ProvinceData
{
    public BAOC_Color color;
    public string name;
    public string country;
    public int vojska;
    public int homogenous_people;
    public int stanovnici;
    public EthnicData ethnicData;
}
[System.Serializable]
public class CountryData
{
    public string name, president, capital;
    public int vojska;
    public int population;
    public int index;
    public Dictionary<string, int> odnosi_dict;
    public List<ProvinceData> provinces;
    public EthnicData ethnicData;
}
[System.Serializable]
public class EthnicData
{
    public int size;
    public string drzava;
    public List<string> ljudi;
    public List<float> postotci;
    public List<int> stanovnici;
}
[System.Serializable]
public class GameData
{
    public string drzava;
    public int potez;
    public int kinta;
    public int zarada;
    public int vojnici;
    public List<WarData> ratovi;
    public GameSettings settings;

    public bool atWar(string country)
    {
        foreach (WarData war in ratovi)
        {
            if (war.defender == country || war.aggresor == country)
            {
                return true;
            }
        }
        return false;
    }
}
[System.Serializable]
public class GameSettings
{
    public int saturation = 40;
}
[System.Serializable]
public class WarData
{
    public string aggresor;
    public string defender;
}
[System.Serializable]
public struct BAOC_Color
{
    
    public float r, g, b, a;

    public BAOC_Color(float r, float g, float b, float a = 1f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public string ToHex()
    {
        return ColorUtility.ToHtmlStringRGBA(GetColor());
    }
    public Color GetColor()
    {
        return new Color(r, g, b, a);
    }
    
    public BAOC_Color Copy()
    {
        return new BAOC_Color(r, g, b, a);
    }

    public BAOC_Color Invert()
    {
        return new BAOC_Color(1f - r, 1f - g, 1f - b, a);
    }


    public BAOC_Color SetComponent(Channel channel, float value)
    {
        BAOC_Color instance = Copy();
        switch (channel)
        {
            case Channel.RED:
                instance = new BAOC_Color(value, g, b, a);
                break;
            case Channel.GREEN:
                instance = new BAOC_Color(r, value, b, a);
                g = value;
                break;
            case Channel.BLUE:
                instance = new BAOC_Color(r, g, value, a);
                b = value;
                break;
            case Channel.ALPHA:
                instance = new BAOC_Color(r, g, b, value);
                a = value;
                break;
        }
        return instance;
    }
    public static BAOC_Color FromColor(Color color)
    {
        return new BAOC_Color(color.r, color.g, color.b, color.a);
    }
    public static BAOC_Color HSVToRGB(float H, float S, float V, bool hdr = false)
    {
        Color white = Color.white;
        if (S == 0f)
        {
            white.r = V;
            white.g = V;
            white.b = V;
        }
        else if (V == 0f)
        {
            white.r = 0f;
            white.g = 0f;
            white.b = 0f;
        }
        else
        {
            white.r = 0f;
            white.g = 0f;
            white.b = 0f;
            float num = H * 6f;
            int num2 = (int)Mathf.Floor(num);
            float num3 = num - (float)num2;
            float num4 = V * (1f - S);
            float num5 = V * (1f - S * num3);
            float num6 = V * (1f - S * (1f - num3));
            switch (num2)
            {
                case 0:
                    white.r = V;
                    white.g = num6;
                    white.b = num4;
                    break;
                case 1:
                    white.r = num5;
                    white.g = V;
                    white.b = num4;
                    break;
                case 2:
                    white.r = num4;
                    white.g = V;
                    white.b = num6;
                    break;
                case 3:
                    white.r = num4;
                    white.g = num5;
                    white.b = V;
                    break;
                case 4:
                    white.r = num6;
                    white.g = num4;
                    white.b = V;
                    break;
                case 5:
                    white.r = V;
                    white.g = num4;
                    white.b = num5;
                    break;
                case 6:
                    white.r = V;
                    white.g = num6;
                    white.b = num4;
                    break;
                case -1:
                    white.r = V;
                    white.g = num4;
                    white.b = num5;
                    break;
            }

            if (!hdr)
            {
                white.r = Mathf.Clamp(white.r, 0f, 1f);
                white.g = Mathf.Clamp(white.g, 0f, 1f);
                white.b = Mathf.Clamp(white.b, 0f, 1f);
            }
        }

        return FromColor(white);
    }
    public static BAOC_Color FromHex(string hex)
    {
        return FromColor(ColorUtility.TryParseHtmlString(hex, out Color color) ? color : Color.white);
    }
    public static BAOC_Color black = new BAOC_Color(0, 0, 0);
    public static BAOC_Color clear = new BAOC_Color(0, 0, 0, 0);
    public static BAOC_Color white = new BAOC_Color(1, 1, 1);

    public static BAOC_Color operator +(BAOC_Color left, BAOC_Color right)
    {
        return new BAOC_Color(left.r + right.r, left.g + right.g, left.b + right.b, left.a + right.a);
    }
    public static BAOC_Color operator -(BAOC_Color left, BAOC_Color right)
    {
        return new BAOC_Color(left.r - right.r, left.g - right.g, left.b - right.b, left.a - right.a);
    }
    public static bool operator ==(BAOC_Color left, BAOC_Color right)
    {
        return (left.r == right.r) && (left.g == right.g) && (left.b == right.b) && (left.a == right.a);
    }
    public static bool operator !=(BAOC_Color left, BAOC_Color right)
    {
        return !(left == right);
    }
    public override bool Equals(object obj)
    {
        return this == (BAOC_Color)obj;
    }
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + r.GetHashCode();
        hash = hash * 31 + g.GetHashCode();
        hash = hash * 31 + b.GetHashCode();
        hash = hash * 31 + a.GetHashCode();
        return hash;
    }
}
#region enum
public enum ProvinceGFXAction
{
    LIT,
    REMOVED,
    IDLE
}
public enum Channel
{
    RED,
    GREEN,
    BLUE,
    ALPHA
}
#endregion
#endregion
public class Game : MonoBehaviour
{

    #region ui
    [Header("UI")]
    public Slider slider;
    public Slider sli_hue;
    public Slider sli_value;
    public TextMeshProUGUI ime;
    public TextMeshProUGUI drzava;
    public TextMeshProUGUI ime_zupanije;
    public TextMeshProUGUI kapital;
    public TextMeshProUGUI gore_levo_zarada;
    public TextMeshProUGUI gore_levo_kinta;
    public TextMeshProUGUI gore_levo_potez;
    public TextMeshProUGUI side_r_drzava;
    public TextMeshProUGUI side_drzava;
    public TextMeshProUGUI side_predsednik;
    public TextMeshProUGUI side_populacija;
    public TextMeshProUGUI side_kapital;
    public TextMeshProUGUI side_vojska;
    public TextMeshProUGUI vojnici_slider_text;
    public TextMeshProUGUI vojnici_cena_text;
    public TextMeshProUGUI odnosi;
    public TextMeshProUGUI odnosi_label;
    public TextMeshProUGUI rat_textinho;
    public TextMeshProUGUI zup_vojska;
    public TextMeshProUGUI zup_stanovnistvo;
    public TextMeshProUGUI zup_text_name;
    public TextMeshProUGUI t_plus;
    public RawImage map_mask;
    public Texture2D colourMap;
    public Texture2D fakeMap;
    public Button plus_button;
    #endregion
    #region boja
    [Header("Boja")]
    public List<BAOC_Color> prefered_BAOC_Colors;
    public Gradient odnosi_gradient;
    public BAOC_Color highlight;
    public BAOC_Color sel_high;
    
    public Gradient vojska_gradient;
    #endregion
    #region vektori
    [Header("Vektori")]
    public Vector2 map_size;
    #endregion
    #region objekti
    [Header("Objekti")]
    public Transform drzave_holder;
    public Transform ime_holder;
    public Transform ime_holder_zup;
    public Transform zup_vojska_text_holder;
    public Transform objavi_rat_btn_transform;
    public GameObject zup_text_vojska_template;
    public Camera main_cam;
    #endregion
    #region konstante
    [Header("Konstante")]
    public int cena_vojnika = 5;
    public int broj_kutova = 64;
    public float loss_factor_recruit = 1.4f;
    #endregion
    #region globalne varijable
    [HideInInspector]
    public GameData gameData;
    [HideInInspector]
    public static string save_path;
    Texture2D fakeMap_dum;
    Rect map_rect;
    Vector2 side_size;
    float scale_factor;
    string last_zup = "";
    string last_zup_selected;

    int cena;

    BAOC_Color last_zup_clr;
    BAOC_Color last_zup_clr_selected;
    BAOC_Color original;
    BAOC_Color target;

    Dictionary<BAOC_Color, List<Vector2Int>> pixels_zup;
    Dictionary<BAOC_Color, ProvinceData> provinces;
    Dictionary<string, List<ProvinceData>> provinces_by_country;
    Dictionary<BAOC_Color, Vector2Int> pozicije_misa_u_provinciji; //koordinate na ekranu, ne na teksturi
    Dictionary<string, CountryData> countries;
    Dictionary<BAOC_Color, List<BAOC_Color>> neighboring_provinces;
    Dictionary<string, EthnicData> ethnic_data;
    Dictionary<string, EthnicData> ethnic_data_zup;
    List<ProvinceData> selected_provinces;
    List<ProvinceData> selected_neighbouring_provinces;
    ProvinceData current_province;
    ProvinceData selected_province;


    bool changed_sliders = false;
    bool og_set = false;
    bool objavi_rat_bool = false;
    bool selected_province_is_part_of_country = false;
    #endregion
    #region glavne metode
    private void Start()
    {
        #region init
        save_path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/baoc/";
        gameData = new GameData();
        gameData.zarada = 2000000;
        gameData.kinta = 20000000;
        gameData.potez = 0;
        gameData.drzava = "Hrvatska";

        gameData.ratovi = new List<WarData>();
        gameData.settings = new GameSettings();
        selected_province = new ProvinceData();
        current_province = new ProvinceData();
        pixels_zup = new Dictionary<BAOC_Color, List<Vector2Int>>();
        provinces = new Dictionary<BAOC_Color, ProvinceData>();
        fakeMap_dum = new Texture2D(fakeMap.width, fakeMap.height);
        ethnic_data = new Dictionary<string, EthnicData>();
        ethnic_data_zup = new Dictionary<string, EthnicData>();
        selected_provinces = new List<ProvinceData>();
        selected_neighbouring_provinces = new List<ProvinceData>();
        provinces_by_country = new Dictionary<string, List<ProvinceData>>();
        countries = new Dictionary<string, CountryData>();
        pozicije_misa_u_provinciji = new Dictionary<BAOC_Color, Vector2Int>();
        neighboring_provinces = new Dictionary<BAOC_Color, List<BAOC_Color>>();

        update_potez_ui();
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
            List<ProvinceData> provinces__ = new List<ProvinceData>();

            foreach (dynamic province in country.podaci)
            {
                BAOC_Color clr = BAOC_Color.FromHex(Convert.ToString(province.boja));




                ProvinceData data = new ProvinceData();
                data.name = province.ime;
                data.country = country.ime;
                data.stanovnici = Convert.ToInt32(province.demografija.populacija);


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

                data.homogenous_people = eth_data.stanovnici[0];
                data.color = clr;

                data.ethnicData = eth_data;
                provinces.Add(clr, data);
                ethnic_data_zup.Add(data.name, eth_data);

                provinces__.Add(data);


            }
            string drzava_ = Convert.ToString(country.ime);
            provinces_by_country[drzava_] = provinces__;
        }
        foreach (BAOC_Color p in provinces.Keys)
        {
            List<Vector2Int> pix_c = new List<Vector2Int>();
            for (int x = 0; x < colourMap.width; x++)
            {
                for (int y = 0; y < colourMap.height; y++)
                {
                    if (p.GetColor() == colourMap.GetPixel(x, y))
                    {
                        pix_c.Add(new Vector2Int(x, y));
                    }
                }

            }
            List<BAOC_Color> nei_p = new List<BAOC_Color>();
            for (int i = 0; i < broj_kutova; i++)
            {
                BAOC_Color neighboring = get_first_non_black_pixel_at_angle(colourMap, pix_c[pix_c.Count / 2], i * (360f / broj_kutova), p);
                if (!nei_p.Contains(neighboring) && provinces.ContainsKey(neighboring))
                {
                    nei_p.Add(neighboring);
                }
            }
            neighboring_provinces[p] = nei_p;
            pixels_zup.Add(p, pix_c);
        }

        #endregion
        #region ethnic
        int index = 0;
        foreach (dynamic drzava in provinces_json.podaci)
        {
            string drz = Convert.ToString(drzava.ime);
            int pop = Convert.ToInt32(drzava.demografija.populacija);
            int vojske = Convert.ToInt32(drzava.vojska);

            CountryData countryData = new CountryData();
            countryData.provinces = provinces_by_country[drz];
            countryData.population = pop;
            countryData.president = drzava.prezident;
            countryData.odnosi_dict = new Dictionary<string, int>();
            countryData.index = index;
            countryData.vojska = vojske;

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

            index++;
        }
        #endregion
        selected_province = provinces[provinces.Keys.ToArray()[0]];
        current_province = selected_province;
        gameData.vojnici = countries[gameData.drzava].vojska;

        update_potez_ui();
        update_province_ui();
        update_rat_text();
        update_side_ui();
        Dictionary<string, ProvinceData> neigh_form2 = new Dictionary<string, ProvinceData>();
        foreach (BAOC_Color c in neighboring_provinces.Keys)
        {
            foreach (BAOC_Color province_c in neighboring_provinces[c])
            {
                neigh_form2[provinces[c].name] = provinces[province_c];
            }
        }


        save_json(neigh_form2, "data");
        #region RPC
        init_rich_presence();
        #endregion
    }
    void Update()
    {
        #region UI
        if (slider.gameObject.activeSelf)
        {
            show_recruit();
        }
        if (sli_hue.transform.parent.gameObject.activeSelf)
        {
            show_BAOC_Color_pick();
        }

        #endregion
        #region miš
        Vector2 mouse_pos = Input.mousePosition;
        if (map_rect.Contains(mouse_pos))
        {
            target = BAOC_Color.FromColor(colourMap.GetPixel((int)mouse_pos.x - (int)side_size.x, (int)mouse_pos.y - (int)side_size.y));
            if (provinces.ContainsKey(target))
            {
                #region nadi provinciju
                current_province = provinces[target];
                ProvinceData data = current_province;
                CountryData countryData = countries[data.country];
                if (data.name != last_zup && last_zup != "")
                {
                    erase_last_zup();
                }
                last_zup = data.name;
                last_zup_clr = target;
                #endregion


                if (Input.GetMouseButtonDown(0))
                {
                    #region pozicija miša na ekranu (provincija dictionary), ne teksturi
                    Vector2Int mouse_pos_int = new Vector2Int((int)mouse_pos.x, (int)mouse_pos.y);

                    mouse_pos_int.x += (int)side_size.x;
                    mouse_pos_int.y += (int)side_size.y;

                    #endregion

                    selected_province = current_province;
                    selected_provinces.Add(selected_province);
                    selected_neighbouring_provinces = province_BAOC_Colors_to_province_data_collection(neighboring_provinces[selected_province.color]);

                    print(mouse_pos_int);

                    pozicije_misa_u_provinciji[selected_province.color] = mouse_pos_int;

                    #region alpha promjene
                    selected_province_is_part_of_country = selected_province.country == gameData.drzava;
                    plus_button.interactable = selected_province_is_part_of_country;
                    if (selected_province_is_part_of_country)
                    {
                        BAOC_Color v_cpy = BAOC_Color.FromColor(side_vojska.color);
                        v_cpy.a = 1f;

                        side_vojska.color = v_cpy.GetColor();

                        v_cpy = BAOC_Color.FromColor(t_plus.color);
                        v_cpy.a = 1f;

                        t_plus.color = v_cpy.GetColor();
                    }
                    else
                    {
                        BAOC_Color v_cpy = BAOC_Color.FromColor(side_vojska.color);
                        v_cpy.a = 0.5f;

                        side_vojska.color = v_cpy.GetColor();

                        v_cpy = BAOC_Color.FromColor(t_plus.color);
                        v_cpy.a = 0.5f;

                        t_plus.color = v_cpy.GetColor();


                    }
                    #endregion
                    #region province hud ui
                    for (int i = 0; i < zup_vojska_text_holder.childCount; i++)
                    {
                        GameObject child_GO = zup_vojska_text_holder.GetChild(i).gameObject;
                        if (child_GO.activeSelf)
                        {
                            Destroy(child_GO);
                        }
                    }

                    //foreach (BAOC_Color key in pozicije_misa_u_provinciji.Keys)
                    //{
                    //    if (provinces[key].vojska != 0)
                    //    {
                    //        Vector3 pos = v2int_to_v3(pozicije_misa_u_provinciji[key]);
                    //        pos.y -= 128;
                    //        zup_text_vojska_template.transform.position = pos;
                    //        zup_text_vojska_template.GetComponent<TextMeshProUGUI>().text = format_number(provinces[key].vojska);
                    //        zup_text_vojska_template.SetActive(true);

                    //        Instantiate(zup_text_vojska_template, zup_text_vojska_template.transform.parent);

                    //        zup_text_vojska_template.SetActive(false);
                    //    }
                    //}
                    #endregion

                    last_zup_selected = selected_province.name;
                    last_zup_clr_selected = selected_province.color;

                    update_side_ui();

                    last_zup = ime.text;
                    last_zup_clr = target;

                    objavi_rat_bool = gameData.drzava == selected_province.country;
                    set_objavi_rat_interactable(!objavi_rat_bool);

                    #region boja UI elemenata
                    set_objavi_rat_text_alpha(objavi_rat_bool ? 0.5f : 1f);

                    odnosi_label.color = objavi_rat_bool ? BAOC_Color.FromColor(odnosi_label.color).SetComponent(Channel.ALPHA, 0.5f).GetColor() : BAOC_Color.FromColor(odnosi_label.color).SetComponent(Channel.ALPHA, 1f).GetColor();
                    odnosi.color = objavi_rat_bool ? BAOC_Color.FromColor(odnosi_label.color).SetComponent(Channel.ALPHA, 0.5f).GetColor() : BAOC_Color.FromColor(odnosi_label.color).SetComponent(Channel.ALPHA, 1f).GetColor();
                    #endregion

                    EthnicData data_eth = ethnic_data[data.country];
                    EthnicData data_eth_zup = ethnic_data_zup[data.name];

                    #region etnicki sastav UI
                    for (int idxi = 0; idxi < data_eth.size; idxi++)
                    {
                        ime_holder.GetChild(idxi).GetComponent<TextMeshProUGUI>().text = String.Format("{0}: {1}%, {2:N0}", data_eth.ljudi[idxi], data_eth.postotci[idxi] * 100f, data_eth.stanovnici[idxi]);
                    }
                    for (int idxi = 0; idxi < data_eth_zup.size; idxi++)
                    {
                        ime_holder_zup.GetChild(idxi).GetComponent<TextMeshProUGUI>().text = String.Format("{0}: {1}%, {2:N0}", data_eth_zup.ljudi[idxi], data_eth_zup.postotci[idxi] * 100f, data_eth_zup.stanovnici[idxi]);
                    }
                    #endregion

                    update_rat_text();
                    update_province_ui();

                    update_rich_presence();
                }
                #region provincija hover
                light_up_province(target, highlight);
                #endregion
            }
        }
        #endregion
        #region GFX provincije
        foreach (ProvinceData data in selected_provinces.ToArray())
        {
            if (data.name != selected_province.name)
            {
                light_up_province(data.color, BAOC_Color.clear);
                selected_provinces.Remove(data);
                continue;
            }
            else
            {
                light_up_province(data.color, sel_high);
                continue;
            }
        }

        //susedi
        foreach (ProvinceData data in selected_neighbouring_provinces.ToArray())
        {
            if (!neighboring_provinces[selected_province.color].Contains(data.color) || data.country != selected_province.country)
            {
                light_up_province(data.color, BAOC_Color.clear);
                selected_neighbouring_provinces.Remove(data);
                print($"Removed {data.name} from neighbouring provinces");
                continue;
            }
            else
            {
                light_up_province(data.color, highlight);
                continue;
            }
        }
        #endregion
    }
    private void OnApplicationQuit()
    {
        RPC.Abort();
    }
    #endregion
    #region gameplay metode
    public void next_potez()
    {
        gameData.potez++;
        gameData.kinta += gameData.zarada;
        update_potez_ui();
        update_rich_presence();
    }
    #endregion
    #region util metode
    List<ProvinceData> province_BAOC_Colors_to_province_data_collection(List<BAOC_Color> BAOC_Colors)
    {
        List<ProvinceData> __provinces = new List<ProvinceData>();
        foreach (BAOC_Color item in BAOC_Colors)
        {
            __provinces.Add(provinces[item]);
        }
        return __provinces;
    }
    #nullable enable
    string jsonify(object? val)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings();
        // This tells your serializer that multiple references are okay.
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        return JsonConvert.SerializeObject(val, settings);
    }
    ///<summary>path = save_path + path to file without starting / and filetype</summary>
    void save_json(object? val, string path)
    {
        File.WriteAllText($"{save_path}{path}.json", jsonify(val));
    }
    void set_province_BAOC_Color(BAOC_Color province, BAOC_Color with)
    {
        List<Vector2Int> pix_data = pixels_zup[province];

        foreach (Vector2Int vector2Int in pix_data)
        {
            fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, with.GetColor());
        }
    }
    void light_up_province(BAOC_Color province, BAOC_Color with_BAOC_Color)
    {
        set_province_BAOC_Color(province, with_BAOC_Color);
        fakeMap_dum.Apply();
        map_mask.texture = fakeMap_dum;
    }
    string BAOC_Color_to_rgb255_string(BAOC_Color c)
    {
        return String.Format("r: {0}, g: {1}, b: {2}", (int)(c.r * 255), (int)(c.g * 255), (int)(c.b * 255));
    }
    ulong get_epoch()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int secondsSinceEpoch = (int)t.TotalSeconds;
        return (ulong)secondsSinceEpoch;
    }
    RPCData get_rpc_data()
    {
        RPCData activity = new RPCData();
        activity.state = "U igri";
        activity.details = $"{gameData.drzava} | {countries[gameData.drzava].provinces.Count} provincije | {gameData.potez}. potez";

        RPC_Timestamps timestamps = new RPC_Timestamps();
        timestamps.start = get_epoch();
        activity.timestamps = timestamps;


        Dictionary<string, string> assets = new Dictionary<string, string>();
        assets.Add("large_image", "logo");
        assets.Add("large_text", "Balkanski AOC");
        assets.Add("small_image", "logo");
        assets.Add("small_text", "Balkanski AOC");

        activity.assets = assets;
        activity.instance = true;

        return activity;
    }
    void init_rich_presence()
    {
        RPC.Init();
        update_rich_presence();
    }
    void update_rich_presence()
    {
        RPCData activity = get_rpc_data();
        RPC.SetActivity(activity);
    }
    Vector3 v2int_to_v3(Vector2Int v2int)
    {
        return new Vector3(v2int.x, v2int.y);
    }
    string format_number(int num)
    {
        return string.Format("{0:N0}", num);
    }
    void change_province_owner(ProvinceData data, string owner)
    {
        provinces[data.color].country = owner;
        update_provinces();
    }
    void set_objavi_rat_interactable(bool interactable)
    {
        objavi_rat_btn_transform.GetComponent<Button>().interactable = interactable;
    }
    void set_objavi_rat_text_alpha(float a)
    {
        BAOC_Color clr = BAOC_Color.FromColor(objavi_rat_btn_transform.GetChild(0).GetComponent<TextMeshProUGUI>().color);
        objavi_rat_btn_transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new BAOC_Color(clr.r, clr.g, clr.b, a).GetColor();
    }
    void update_potez_ui()
    {
        gore_levo_kinta.text = format_number(gameData.kinta);
        gore_levo_potez.text = "Potez " + format_number(gameData.potez);
        gore_levo_zarada.text = gameData.zarada > 0 ? String.Format("+{0:N0}", gameData.zarada) : format_number(gameData.zarada);
        side_vojska.text = format_number(gameData.vojnici);
    }
    void update_province_ui()
    {
        zup_text_name.text = selected_province.name;
        zup_stanovnistvo.text = $"{format_number(selected_province.stanovnici)} ljudi";
        zup_vojska.text = $"{format_number(selected_province.vojska)} vojnika";
    }
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
        set_province_BAOC_Color(last_zup_clr, BAOC_Color.clear);
        fakeMap_dum.Apply();
    }
    void show_recruit()
    {
        slider.maxValue = selected_province.homogenous_people;
        slider.minValue = 0;
        vojnici_slider_text.text = format_number(Mathf.RoundToInt(slider.value));
        cena = Mathf.RoundToInt(slider.value) * cena_vojnika;

        vojnici_cena_text.text = String.Format("{0:N0} $", cena);
        vojnici_cena_text.color = cena > gameData.kinta ? odnosi_gradient.Evaluate(1f) : odnosi_gradient.Evaluate(0f);
    }
    void show_BAOC_Color_pick()
    {
        if (!og_set)
        {
            BAOC_Color c = BAOC_Color.FromColor(drzave_holder.GetChild(countries[current_province.country].index).GetComponent<RawImage>().color);
            original = c;
            og_set = true;
        }
        if (changed_sliders)
        {
            BAOC_Color _c = BAOC_Color.HSVToRGB(sli_hue.value, gameData.settings.saturation / 100f, sli_value.value);
            drzave_holder.GetChild(countries[current_province.country].index).GetComponent<RawImage>().color = _c.GetColor();
        }
    }
    void update_provinces()
    {
        selected_province = provinces[selected_province.color];
        current_province = provinces[current_province.color];
    }
    public void end_recruit()
    {
        slider.gameObject.SetActive(false);
        gameData.vojnici += (int)slider.value;
        provinces[selected_province.color].vojska += (int)slider.value;
        gameData.kinta -= cena;
        provinces[selected_province.color].homogenous_people -= (int)(slider.value / loss_factor_recruit);
        update_provinces();
        update_province_ui();
        update_potez_ui();
    }
    public void end_BAOC_Color_pick()
    {
        sli_hue.transform.parent.gameObject.SetActive(false);
    }
    public void abort_BAOC_Color_pick()
    {
        sli_hue.transform.parent.gameObject.SetActive(false);
        drzave_holder.GetChild(countries[current_province.country].index).GetComponent<RawImage>().color = original.GetColor();
    }
    public void set_sliders_changed(bool to)
    {
        changed_sliders = to;
    }
    void update_rat_text()
    {
        rat_textinho.text = "<sprite=0>";
        foreach (WarData dom in gameData.ratovi)
        {
            if (dom.aggresor != selected_province.country)
            {
                rat_textinho.text += $"<sprite={countries[dom.aggresor].index + 1}>";
            }
            else
            {
                rat_textinho.text += $"<sprite={countries[dom.defender].index + 1}>";
            }
        }
    }
    public void objavi_ratno_stanje()
    {
        if (!gameData.atWar(gameData.drzava))
        {
            WarData wdata = new WarData();

            wdata.aggresor = gameData.drzava;
            wdata.defender = selected_province.country;

            gameData.ratovi.Add(wdata);

            update_rat_text();
        }
    }
    void update_side_ui()
    {
        ProvinceData data = current_province;
        CountryData countryData = countries[data.country];
        ime.text = data.name;
        drzava.text = data.country;
        side_r_drzava.text = data.country;
        if (selected_province.country == gameData.drzava)
        {
            side_vojska.text = format_number(gameData.vojnici);
        }
        else
        {
            side_vojska.text = format_number(countryData.vojska);
        }
        side_predsednik.text = countryData.president;
        side_drzava.text = countryData.name;
        side_kapital.text = countryData.capital;
        side_populacija.text = format_number(countryData.population);
        if (countryData.odnosi_dict.ContainsKey(gameData.drzava))
        {
            int o_d = countryData.odnosi_dict[gameData.drzava];
            odnosi.text = o_d > 0 ? $"+{o_d}" : $"{o_d}";
            float o_d_grad = 1f - ((o_d + 100f) / 200f);
            odnosi.color = odnosi_gradient.Evaluate(o_d_grad);
        }
        else
        {
            odnosi.text = "";
            odnosi.color = Color.white;
        }
        ime_zupanije.text = data.name;
    }
    public void resetiraj_slider()
    {
        slider.value = 0;
    }
    List<Vector2Int> get_circle_points(int radius)
    {
        List<Vector2Int> points = new List<Vector2Int>();


        int x = radius;
        int y = 0;
        int err = 0;

        int x0 = 0;
        int y0 = 0;

        while (x >= y)
        {
            points.Add(new Vector2Int(x0 + x, y0 + y));
            points.Add(new Vector2Int(x0 + y, y0 + x));
            points.Add(new Vector2Int(x0 - y, y0 + x));
            points.Add(new Vector2Int(x0 - x, y0 + y));
            points.Add(new Vector2Int(x0 - x, y0 - y));
            points.Add(new Vector2Int(x0 - y, y0 - x));
            points.Add(new Vector2Int(x0 + y, y0 - x));
            points.Add(new Vector2Int(x0 + x, y0 - y));

            if (err <= 0)
            {
                y += 1;
                err += 2 * y + 1;
            }

            if (err > 0)
            {
                x -= 1;
                err -= 2 * x + 1;
            }
        }

        return points;
    }
    BAOC_Color get_first_non_black_pixel_at_angle(Texture2D tex, Vector2Int origin, float angle, BAOC_Color original)
    {
        List<Vector2Int> points = get_circle_points(128);
        Vector2Int p2 = points[(int)((angle / 360f) * points.Count)] + origin;

        Vector2 t = origin;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - origin.x, 2) + Mathf.Pow(p2.y - origin.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
        {
            t = Vector2.Lerp(origin, p2, ctr);
            ctr += frac;
            BAOC_Color col = BAOC_Color.FromColor(tex.GetPixel((int)t.x, (int)t.y));

            if (col != BAOC_Color.black && col != original && provinces.ContainsKey(col))
            {
                return col;
            }
        }
        fakeMap_dum.Apply();
        return BAOC_Color.black;
    }
    #endregion
}