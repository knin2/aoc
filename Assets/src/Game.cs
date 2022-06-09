using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Web;
using TMPro;
using System.IO;
using Newtonsoft.Json;

#region klase
[System.Serializable]
public class ProvinceData
{
    public Color color;
    public string name;
    public string country;
    public int vojska;
    public int homogenous_people;
    public int stanovnici;
    public EthnicData ethnicData;
}
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
public class EthnicData
{
    public int size;
    public string drzava;
    public List<string> ljudi;
    public List<float> postotci;
    public List<int> stanovnici;
}
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
public class GameSettings
{
    public int saturation = 40;
}
public class WarData
{
    public string aggresor;
    public string defender;
}
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
    public List<Color> prefered_colors;
    public Gradient odnosi_gradient;
    public Color highlight;
    public Color sel_high;
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
    public GameObject template;
    #endregion
    #region konstante
    [Header("Konstante")]
    public int cena_vojnika = 5;
    public float loss_factor_recruit = 1.4f;
    #endregion
    #region globalne varijable
    [HideInInspector]
    public GameData gameData;
    Texture2D fakeMap_dum;
    Rect map_rect;
    Vector2 side_size;
    float scale_factor;
    string last_zup = "";
    string last_zup_selected;

    int cena;

    Color last_zup_clr;
    Color last_zup_clr_selected;
    Color original;
    Color target;

    Dictionary<Color, List<Vector2Int>> pixels_zup;
    Dictionary<Color, ProvinceData> provinces;
    Dictionary<Color, Vector2Int> pozicije_misa_u_provinciji; //koordinate na ekranu, ne na teksturi
    Dictionary<string, CountryData> countries;
    Dictionary<string, EthnicData> ethnic_data;
    Dictionary<string, EthnicData> ethnic_data_zup;
    List<ProvinceData> selected_provinces;
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
        gameData = new GameData();
        gameData.zarada = 2000000;
        gameData.kinta = 20000000;
        gameData.potez = 0;
        gameData.drzava = "Hrvatska";
        gameData.ratovi = new List<WarData>();
        gameData.settings = new GameSettings();
        update_potez_ui();
        selected_province = new ProvinceData();
        selected_province.name = "";
        selected_province.stanovnici = 0;
        current_province = new ProvinceData();
        pixels_zup = new Dictionary<Color, List<Vector2Int>>();
        provinces = new Dictionary<Color, ProvinceData>();
        fakeMap_dum = new Texture2D(fakeMap.width, fakeMap.height);
        ethnic_data = new Dictionary<string, EthnicData>();
        ethnic_data_zup = new Dictionary<string, EthnicData>();
        selected_provinces = new List<ProvinceData>();
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
        #endregion
        #region ethnic
        int index = 0;
        foreach (dynamic drzava in provinces_json.podaci)
        {
            string drz = Convert.ToString(drzava.ime);
            int pop = Convert.ToInt32(drzava.demografija.populacija);
            int vojske = Convert.ToInt32(drzava.vojska);

            CountryData countryData = new CountryData();
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
            show_color_pick();
        }

        #endregion
        #region miš
        Vector2 mouse_pos = Input.mousePosition;
        if (map_rect.Contains(mouse_pos))
        {
            target = colourMap.GetPixel((int)mouse_pos.x - (int)side_size.x, (int)mouse_pos.y - (int)side_size.y);
            if (provinces.ContainsKey(target))
            {
                #region na?i provinciju
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
                    Vector2Int mouse_pos_int = new Vector2Int( (int) mouse_pos.x, (int) mouse_pos.y);

                    mouse_pos_int.x += (int) side_size.x;
                    mouse_pos_int.y += (int) side_size.y;

                    #endregion

                    selected_province = current_province;
                    selected_provinces.Add(selected_province);

                    pozicije_misa_u_provinciji[selected_province.color] = mouse_pos_int;

                    #region alpha promjene
                    selected_province_is_part_of_country = selected_province.country == gameData.drzava;
                    plus_button.interactable = selected_province_is_part_of_country;
                    if (selected_province_is_part_of_country)
                    {
                        Color v_cpy = copy_color(side_vojska.color);
                        v_cpy.a = 1f;

                        side_vojska.color = v_cpy;

                        v_cpy = copy_color(t_plus.color);
                        v_cpy.a = 1f;

                        t_plus.color = v_cpy;
                    }
                    else
                    {
                        Color v_cpy = copy_color(side_vojska.color);
                        v_cpy.a = 0.5f;

                        side_vojska.color = v_cpy;

                        v_cpy = copy_color(t_plus.color);
                        v_cpy.a = 0.5f;

                        t_plus.color = v_cpy;

                        
                    }
                    #endregion

                    for (int i = 0; i < zup_vojska_text_holder.childCount; i++)
                    {
                        zup_vojska_text_holder.
                    }

                    foreach (Color key in pozicije_misa_u_provinciji.Keys)
                    {
                        
                    }

                    last_zup_selected = selected_province.name;
                    last_zup_clr_selected = selected_province.color;

                    update_side_ui();

                    last_zup = ime.text;
                    last_zup_clr = target;

                    objavi_rat_bool = gameData.drzava == selected_province.country;
                    set_objavi_rat_interactable(!objavi_rat_bool);

                    #region boja UI elemenata
                    set_objavi_rat_text_alpha(objavi_rat_bool ? 0.5f : 1f);

                    odnosi_label.color = objavi_rat_bool ? new Color(odnosi_label.color.r, odnosi_label.color.g, odnosi_label.color.b, 0.5f) : new Color(odnosi_label.color.r, odnosi_label.color.g, odnosi_label.color.b, 1);
                    odnosi.color = objavi_rat_bool ? new Color(odnosi.color.r, odnosi.color.g, odnosi.color.b, 0.5f) : new Color(odnosi.color.r, odnosi.color.g, odnosi.color.b, 1);
                    #endregion
                    
                    EthnicData data_eth = ethnic_data[data.country];
                    EthnicData data_eth_zup = ethnic_data_zup[data.name];

                    #region etni?ki sastav UI
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
                }
                List<Vector2Int> pix_data = pixels_zup[target];

                foreach (Vector2Int vector2Int in pix_data)
                {
                    fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, highlight);
                }
                fakeMap_dum.Apply();
                map_mask.texture = fakeMap_dum;
            }
        }
        #endregion
        #region GFX provincije
        foreach (ProvinceData data in selected_provinces.ToArray())
        {
            if (data.name != selected_province.name)
            {
                List<Vector2Int> pix_data_ = pixels_zup[data.color];
                foreach (Vector2Int vector2Int in pix_data_)
                {
                    fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, Color.clear);
                }
                fakeMap_dum.Apply();
                map_mask.texture = fakeMap_dum;
                selected_provinces.Remove(data);
                continue;
            }
            else
            {
                List<Vector2Int> pix_data_ = pixels_zup[data.color];
                foreach (Vector2Int vector2Int in pix_data_)
                {
                    fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, sel_high);
                }
                fakeMap_dum.Apply();
                map_mask.texture = fakeMap_dum;
                continue;
            }
        }
        #endregion
    }
    #endregion
    #region util metode
    Color copy_color(Color og)
    {
        return new Color(og.r, og.g, og.b, og.a);
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
        Color clr = objavi_rat_btn_transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
        objavi_rat_btn_transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(clr.r, clr.g, clr.b, a);
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
    public void next_potez()
    {
        gameData.potez++;
        gameData.kinta += gameData.zarada;
        update_potez_ui();
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
        List<Vector2Int> pix_data = pixels_zup[last_zup_clr];
        foreach (Vector2Int vector2Int in pix_data)
        {
            fakeMap_dum.SetPixel(vector2Int.x, vector2Int.y, Color.clear);
        }
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
    void show_color_pick()
    {
        if (!og_set)
        {
            Color c = drzave_holder.GetChild(countries[current_province.country].index).GetComponent<RawImage>().color;
            original = c;
            og_set = true;
        }
        if (changed_sliders)
        {
            Color _c = Color.HSVToRGB(sli_hue.value, gameData.settings.saturation / 100f, sli_value.value);
            drzave_holder.GetChild(countries[current_province.country].index).GetComponent<RawImage>().color = _c;
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
    public void end_color_pick()
    {
        sli_hue.transform.parent.gameObject.SetActive(false);
    }
    public void abort_color_pick()
    {
        sli_hue.transform.parent.gameObject.SetActive(false);
        drzave_holder.GetChild(countries[current_province.country].index).GetComponent<RawImage>().color = original;
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
    #endregion
}