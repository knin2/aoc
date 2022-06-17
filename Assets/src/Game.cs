using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

#region klase
public static class BAOC
{
    public static object that;
    static string GetSorrounded(string start, string end, string s)
    {
        int idx = s.IndexOf(start) + start.Length;
        int end_idx = s.IndexOf(end) - end.Length;
        return s.Substring(idx, (end_idx - idx) + 1);
    }
#nullable enable
    public static void Log(object val, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = "")
    {
        Debug.Log(string.Format("[LOG] BAOC: [{0}] [{1}]: [{3}] [{2}] | {4}",
            DateTime.Now.ToString("dd-MM-yyyy h:mm:ss tt"), that.GetType().Name, caller, lineNumber, val)
        );
        return;

    }
#nullable disable
}
[System.Serializable]
public class ProvinceData
{
    public BAOC_Color Color;
    public string Name;
    public string Country;
    public int Army;
    public int HomogenousPeople;
    public int Population;
    public EthnicData EthnicData;
}
[System.Serializable]
public class CountryData
{
    public string Name, President, Capital;
    public int Army;
    public int Population;
    public int Index;
    public Dictionary<string, int> Relations;
    public List<ProvinceData> Provinces;
    public EthnicData EthnicData;
}
[System.Serializable]
public class EthnicData
{
    public int Size;
    public string Country;
    public List<string> PopulationByNation;
    public List<float> PopulationPercentages;
    public List<int> PopulationByQuantity;
}
[System.Serializable]
public class GameData
{
    public string Country;
    public int Move;
    public int Balance;
    public int Income;
    public int Army;
    public List<WarData> Wars;


}
[System.Serializable]
public class WarData
{
    public string Aggresor;
    public string Defender;
}
[System.Serializable]
public struct BAOC_Color
{
    public float R, G, B, A;
    public BAOC_Color(float r = 0, float g = 0, float b = 0, float a = 1f) : this()
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public string ToHex()
    {
        return ColorUtility.ToHtmlStringRGBA(GetColor());
    }
    public Color GetColor()
    {
        return new Color(R, G, B, A);
    }

    public BAOC_Color Copy()
    {
        return new BAOC_Color(R, G, B, A);
    }

    public BAOC_Color Invert()
    {
        return new BAOC_Color(1f - R, 1f - G, 1f - B, A);
    }


    public BAOC_Color SetComponent(Channel channel, float value)
    {
        BAOC_Color instance = Copy();
        switch (channel)
        {
            case Channel.RED:
                instance = new BAOC_Color(value, G, B, A);
                break;
            case Channel.GREEN:
                instance = new BAOC_Color(R, value, B, A);
                break;
            case Channel.BLUE:
                instance = new BAOC_Color(R, G, value, A);
                break;
            case Channel.ALPHA:
                instance = new BAOC_Color(R, G, B, value);
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
        Color out_clr = Color.clear;
        ColorUtility.TryParseHtmlString(hex, out out_clr);
        BAOC_Color bclr = FromColor(out_clr);
        return bclr;
    }
    public static BAOC_Color black = new BAOC_Color();
    public static BAOC_Color clear = new BAOC_Color(a: 0);
    public static BAOC_Color white = new BAOC_Color(1, 1, 1);
    public static BAOC_Color red = new BAOC_Color(1);
    public static BAOC_Color special0 = BAOC_Color.FromHex("#A64026FF");

    public static BAOC_Color operator +(BAOC_Color left, BAOC_Color right)
    {
        return new BAOC_Color(left.R + right.R, left.G + right.G, left.B + right.B, left.A + right.A);
    }
    public static BAOC_Color operator -(BAOC_Color left, BAOC_Color right)
    {
        return new BAOC_Color(left.R - right.R, left.G - right.G, left.B - right.B, left.A - right.A);
    }
    public static bool operator ==(BAOC_Color left, BAOC_Color right)
    {
        return (left.R == right.R) && (left.G == right.G) && (left.B == right.B) && (left.A == right.A);
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
        hash = hash * 31 + R.GetHashCode();
        hash = hash * 31 + G.GetHashCode();
        hash = hash * 31 + B.GetHashCode();
        hash = hash * 31 + A.GetHashCode();
        return hash;
    }
    public override string ToString()
    {
        return $"BAOC_Color({R}, {G}, {B}, {A})";
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
    #region varijable
    #region ui
    [Header("UI")]
    public Slider S_Slider;
    public Slider S_SliderHue;
    public Slider S_SliderValue;
    public TextMeshProUGUI TXT_ProvinceName;
    public TextMeshProUGUI TXT_Country;
    public TextMeshProUGUI TXT_Capital;
    public TextMeshProUGUI TXT_Income;
    public TextMeshProUGUI TXT_Balance;
    public TextMeshProUGUI TXT_Move;
    public TextMeshProUGUI TXT_SideBarRightProvinceName;
    public TextMeshProUGUI TXT_SideBarRightCountryName;
    public TextMeshProUGUI TXT_SideBarCountry;
    public TextMeshProUGUI TXT_SideBarPresident;
    public TextMeshProUGUI TXT_SideBarPopulation;
    public TextMeshProUGUI TXT_SideBarCapital;
    public TextMeshProUGUI TXT_SideBarArmy;
    public TextMeshProUGUI TXT_SideBarProvinceArmy;
    public TextMeshProUGUI TXT_SideBarProvincePopulation;
    public TextMeshProUGUI TXT_SideBarProvinceName;
    public TextMeshProUGUI TXT_ArmyText;
    public TextMeshProUGUI TXT_ArmyPriceText;
    public TextMeshProUGUI TXT_RelationsValue;
    public TextMeshProUGUI TXT_RelationsLabel;
    public TextMeshProUGUI TXT_Wars;
    public TextMeshProUGUI TXT_RecruitButtonText;
    public Texture2D TEX_ColourMap;
    public Button B_RecruitButton;
    #endregion
    #region boja
    [Header("Boja")]
    public Gradient GR_RelationsGradient;
    public BAOC_Color BCLR_ProvinceHoverHighlight;
    public BAOC_Color BCLR_ProvinceNeighbourHighlight;
    public BAOC_Color BCLR_ProvinceSelectHighlight;
    #endregion
    #region vektori
    [Header("Vektori")]
    public Vector2 V2_MapSize;
    #endregion
    #region objekti
    [Header("Objekti")]
    public Transform TF_CountriesByColor;
    public Transform TF_CountryEthnicStructure;
    public Transform TF_ProvinceEthnicStructure;
    public Transform TF_DeclareWar;
    public Transform TF_ProvinceHolder;
    public Transform TF_ArrowHolder;
    public GameObject GO_ArrowPrefab;
    public Camera CM_MainCamera;
    #endregion
    #region konstante
    [Header("Konstante")]
    public int CInt_SoldierPrice = 5;
    public int CInt_AnglesForNeighbourDetection = 64;
    public int CInt_PixelOriginsForNeighbourDetection = 6;
    public float CFloat_RecruitLossFactor = 1.4f;
    public float CFloat_ArrowAnimationTime = 0.25f;
    #endregion
    #region klase static
    #endregion
    #region globalne varijable
    [HideInInspector]
    public GameData GameData;
    [HideInInspector]

    /// <summary>
    /// završava s [/].
    /// </summary>
    public static string SavePath;

    string LastProvinceByName = "";
    string LastProvinceSelectedByName;
    Rect MapRect;
    Vector2 V2_SideSize;
    float ScaleFactor;

    int Price;

    BAOC_Color LastProvinceColor;
    BAOC_Color LastProvinceSelected;
    BAOC_Color Original;
    BAOC_Color MouseTarget;
    Dictionary<BAOC_Color, ProvinceData> Provinces;
    Dictionary<string, ProvinceData> ProvincesByName;
    Dictionary<string, List<ProvinceData>> ProvincesByCountry;
    Dictionary<string, CountryData> CountriesByName;
    Dictionary<BAOC_Color, List<BAOC_Color>> ProvinceNeighbours;
    Dictionary<string, Dictionary<BAOC_Color, List<BAOC_Color>>> ProvinceNeighboursFiltered;
    Dictionary<string, EthnicData> EthnicDataByCountryName;
    Dictionary<string, EthnicData> EthnicDataByProvinceName;
    Dictionary<string, int> ProvinceHolderChildIndexesByProvinceName;
    Dictionary<BAOC_Color, Vector2Int> ProvincePixels;
    Dictionary<BAOC_Color, Vector2Int> ProvincePixelsNearToSpecial;
#nullable enable
    Dictionary<BAOC_Color, List<Vector2Int>?> PixelPositionsByColor = new Dictionary<BAOC_Color, List<Vector2Int>?>();
#nullable disable
    /// <Summary>
    /// key = lowercase,
    /// value = province.Name
    /// </Summary>
    Dictionary<string, string> LowercaseProvinceNames;
    List<ProvinceData> SelectedProvinces;
    List<ProvinceData> SelectedNeighbouringProvinces;
    ProvinceData CurrentProvince;
    ProvinceData SelectedProvince;


    bool ChangedSliders = false;
    bool OriginalSet = false;
    bool DeclareWarBoolean = false;
    bool SelectedProvinceIsPartOfCountry = false;
    #endregion
    #endregion
    #region glavne metode
    private void Start()
    {
        #region init
        BAOC.that = this;

        SavePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/baoc/";

        GameData = new GameData();
        GameData.Income = 2000000;
        GameData.Balance = 20000000;
        GameData.Move = 0;
        GameData.Country = "Hrvatska";

#nullable enable
        GameData.Wars = new List<WarData>();
        SelectedProvince = new ProvinceData();
        CurrentProvince = new ProvinceData();
        ProvincePixels = new Dictionary<BAOC_Color, Vector2Int>();
        Provinces = new Dictionary<BAOC_Color, ProvinceData>();
        EthnicDataByCountryName = new Dictionary<string, EthnicData>();
        EthnicDataByProvinceName = new Dictionary<string, EthnicData>();
        SelectedProvinces = new List<ProvinceData>();
        SelectedNeighbouringProvinces = new List<ProvinceData>();
        ProvincesByCountry = new Dictionary<string, List<ProvinceData>>();
        ProvincesByName = new Dictionary<string, ProvinceData>();
        CountriesByName = new Dictionary<string, CountryData>();
        ProvinceNeighbours = new Dictionary<BAOC_Color, List<BAOC_Color>>();
        ProvinceNeighboursFiltered = new Dictionary<string, Dictionary<BAOC_Color, List<BAOC_Color>>>();
        PixelPositionsByColor = new Dictionary<BAOC_Color, List<Vector2Int>?>();
        ProvinceHolderChildIndexesByProvinceName = new Dictionary<string, int>();
        LowercaseProvinceNames = new Dictionary<string, string>();
#nullable disable
        UpdateMoveUI();
        #endregion
        #region province
        // Fetch the pixel data of the texture as a big block we can iterate through quickly.
        ScaleFactor = 1920 / Screen.width;
        int size_x = Mathf.RoundToInt(
            (
                (Screen.width * ScaleFactor)
                    -
                (V2_MapSize.x * ScaleFactor)
            )
            / 2);
        int size_y = Mathf.RoundToInt(
                (
                    (Screen.height * ScaleFactor)
                        -
                    (V2_MapSize.y * ScaleFactor)
                )
            / 2);
        V2_SideSize = new Vector2(size_x, size_y);
        MapRect = new Rect(V2_SideSize, V2_MapSize);

        for (int x = 0; x < TEX_ColourMap.width; x++)
        {
            for (int y = 0; y < TEX_ColourMap.height; y++)
            {
                Vector2Int v2int = new Vector2Int(x, y);
                BAOC_Color color = BAOC_Color.FromColor(TEX_ColourMap.GetPixel(x, y));

                if (!PixelPositionsByColor.ContainsKey(color))
                {
                    PixelPositionsByColor[color] = new List<Vector2Int>();
                }
                PixelPositionsByColor[color].Add(v2int);
            }
        }



        string json = File.ReadAllText("Assets/Provinces/Provinces.json");
        dynamic Provinces_json = JsonConvert.DeserializeObject(json);
        foreach (dynamic country in Provinces_json.podaci)
        {
            List<ProvinceData> Provinces__ = new List<ProvinceData>();

            foreach (dynamic province in country.podaci)
            {
                BAOC_Color clr = BAOC_Color.FromHex(Convert.ToString(province.boja));




                ProvinceData data = new ProvinceData();
                data.Name = province.ime;
                data.Country = country.ime;
                data.Population = Convert.ToInt32(province.demografija.populacija);


                EthnicData eth_data = new EthnicData();
                eth_data.Country = data.Country;
                dynamic demo = province.demografija;
                eth_data.Size = Convert.ToInt32(demo.etnicka_struktura_velicina);
                eth_data.PopulationByQuantity = new List<int>();
                eth_data.PopulationPercentages = new List<float>();
                eth_data.PopulationByNation = new List<string>();

                int zup_populacija = Convert.ToInt32(demo.populacija);
                for (int i = 0; i < eth_data.Size; i++)
                {
                    float f = float.Parse(Convert.ToString(demo.etnicka_struktura_float[i]));
                    string s = Convert.ToString(demo.etnicka_struktura_str[i]);
                    eth_data.PopulationPercentages.Add(f);
                    eth_data.PopulationByNation.Add(s);
                    eth_data.PopulationByQuantity.Add(Mathf.RoundToInt(f * zup_populacija));
                }

                data.HomogenousPeople = eth_data.PopulationByQuantity[0];
                data.Color = clr;

                data.EthnicData = eth_data;

                Provinces.Add(clr, data);
                ProvincesByName.Add(data.Name, data);

                EthnicDataByProvinceName.Add(data.Name, eth_data);

                Provinces__.Add(data);

                string key__ = ProvinceLowercase(data.Name);
                LowercaseProvinceNames.Add(key__, data.Name);
            }
            string drzava_ = Convert.ToString(country.ime);
            ProvincesByCountry[drzava_] = Provinces__;
        }
        #endregion
        #region ethnic (2nd provinceData pass)
        ProvincePixelsNearToSpecial = GetProvinceColorsNearToSpecialColor();
        int index = 0;
        foreach (dynamic drzava in Provinces_json.podaci)
        {
            string drz = Convert.ToString(drzava.ime);
            int pop = Convert.ToInt32(drzava.demografija.populacija);
            int vojske = Convert.ToInt32(drzava.Army);

            CountryData countryData = new CountryData();
            countryData.Provinces = ProvincesByCountry[drz];
            countryData.Population = pop;
            countryData.President = drzava.prezident;
            countryData.Relations = new Dictionary<string, int>();
            countryData.Index = index;
            countryData.Army = vojske;

            EthnicData data = new EthnicData();
            data.PopulationPercentages = new List<float>();
            data.PopulationByNation = new List<string>();
            data.PopulationByQuantity = new List<int>();

            for (int i = 0; i < Convert.ToInt32(drzava.demografija.etnicka_struktura_velicina); i++)
            {
                float f = float.Parse(Convert.ToString(drzava.demografija.etnicka_struktura_float[i]));
                string s = Convert.ToString(drzava.demografija.etnicka_struktura_str[i]);
                data.PopulationPercentages.Add(f);
                data.PopulationByNation.Add(s);
                data.PopulationByQuantity.Add(Mathf.RoundToInt(f * pop));
            }
            for (int i = 0; i < Convert.ToInt32(drzava.odnosi_velicina); i++)
            {
                int j = i * 2;
                string o_drz = drzava.odnosi[j];
                int o_data = drzava.odnosi[j + 1];
                countryData.Relations[o_drz] = o_data;
            }
            data.Size = data.PopulationPercentages.Count;
            data.Country = drz;

            countryData.Capital = drzava.kapital;
            countryData.Name = drzava.ime;
            countryData.EthnicData = data;

            CountriesByName[Convert.ToString(drzava.ime)] = countryData;
            EthnicDataByCountryName.Add(drz, data);



            #region detekcija suseda
            foreach (dynamic province in drzava.podaci)
            {
                List<BAOC_Color> __nei = new List<BAOC_Color>();
                List<BAOC_Color> __f_nei = new List<BAOC_Color>();


                string hexCode = Convert.ToString(province.boja);
                BAOC_Color __key_clr = BAOC_Color.FromHex(hexCode);

                ProvincePixels[__key_clr] = ProvincePixelsNearToSpecial[__key_clr];

                foreach (dynamic neighbour in province.susjedi)
                {
                    string name = Convert.ToString(neighbour);
                    BAOC_Color __clr = ProvincesByName[name].Color;

                    ProvinceData __data = Provinces[__clr];

                    if (__data.Country == drz)
                    {
                        __f_nei.Add(__clr);
                    }

                    __nei.Add(__clr);

                }

                ProvinceNeighbours[__key_clr] = __nei;

                Dictionary<BAOC_Color, List<BAOC_Color>> __val = new Dictionary<BAOC_Color, List<BAOC_Color>>();
                __val[__key_clr] = __f_nei;

                ProvinceNeighboursFiltered[drz] = __val;
            }
            #endregion
            index++;
        }
        #endregion
        #region province highlight setup
        List<Transform> children = GetChildren(TF_ProvinceHolder);
        foreach (Transform child in children)
        {
            RawImage img = child.GetComponent<RawImage>();
            ProvinceHolderChildIndexesByProvinceName[LowercaseProvinceNames[img.texture.name]] = children.IndexOf(child);
        }
        #endregion
        SelectedProvince = Provinces[BAOC_Color.red];

        LightUpProvince(SelectedProvince.Color, BCLR_ProvinceSelectHighlight);
        LightUpProvinces(ProvinceNeighbours[SelectedProvince.Color], BCLR_ProvinceNeighbourHighlight);

        CurrentProvince = SelectedProvince;
        LastProvinceSelected = SelectedProvince.Color;
        LastProvinceSelectedByName = SelectedProvince.Name;
        GameData.Army = CountriesByName[GameData.Country].Army;

        UpdateMoveUI();
        UpdateProvinceUI();
        UpdateWarText();
        UpdateSideBarUI();
        #region RPC
        InitializeRichPresence();
        #endregion
    }
    void Update()
    {
        #region UI
        if (S_Slider.gameObject.activeSelf)
        {
            ShowRecruitGUI();
        }
        if (S_SliderHue.transform.parent.gameObject.activeSelf)
        {
            ShowColorPick();
        }

        #endregion
        #region miš
        Vector2Int mouse_pos = new Vector2Int((int)Input.mousePosition.x, (int)Input.mousePosition.y);
        if (MapRect.Contains(mouse_pos))
        {
            MouseTarget = GetMouseColor(mouse_pos);

            if (MouseTarget == BAOC_Color.special0)
            {
                mouse_pos.x++;
                MouseTarget = GetMouseColor(mouse_pos);
            }

            if (Provinces.ContainsKey(MouseTarget))
            {
                ClearAllProvincesExcept(new BAOC_Color[] { SelectedProvince.Color, MouseTarget }.Concat(ProvinceNeighbours[SelectedProvince.Color]).ToArray());

                #region nadi provinciju
                CurrentProvince = Provinces[MouseTarget];
                ProvinceData data = CurrentProvince;
                #endregion


                if (Input.GetMouseButtonDown(0))
                {
                    ClearAllProvinces();

                    SelectedProvince = CurrentProvince;


                    LightUpProvince(SelectedProvince.Color, BCLR_ProvinceSelectHighlight);

                    SelectedProvinces.Add(SelectedProvince);
                    //SelectedNeighbouringProvinces = ProvincesByBAOCColorsToProvincesByProvinceData(ProvinceNeighbours[SelectedProvince.Color]);
                    List<Transform> _children = GetChildren(TF_ArrowHolder);
                    foreach (Transform c in _children)
                    {
                        Destroy(c.gameObject);
                    }

                    foreach (BAOC_Color neighbourColor in ProvinceNeighbours[SelectedProvince.Color])
                    {
                        ProvinceData neighbour = Provinces[neighbourColor];

                        //draw arrows if neighbour is a part of the current country OR the current country is at war with the neighbour

                        if (SelectedProvince.Country != GameData.Country) continue;

                        if (SelectedProvince.Army <= 0) continue;

                        if (!AtWar(neighbour.Country, GameData.Country))
                        {
                            if (neighbour.Country != GameData.Country) continue;
                        }

                        Vector2Int shit = ProvincePixels[neighbourColor];
                        Vector2Int originShit = ProvincePixels[SelectedProvince.Color];
                        float atangent = AngleBetweenVector2(originShit, shit);

                        GameObject newArrowText = Instantiate(GO_ArrowPrefab, TF_ArrowHolder.transform);
                        Vector3 pos = TexturePointToWorld(originShit/* - new Vector2Int((int)R.rect.width, (int)R.rect.height)*/);
                        pos.z = 90;
                        newArrowText.transform.position = pos;
                        newArrowText.transform.rotation = Quaternion.Euler(new Vector3(0, 0, atangent));

                        LightUpProvince(neighbour.Color, BCLR_ProvinceNeighbourHighlight);

                    }

                    #region alpha promjene
                    SelectedProvinceIsPartOfCountry = SelectedProvince.Country == GameData.Country;
                    B_RecruitButton.interactable = SelectedProvinceIsPartOfCountry;
                    if (SelectedProvinceIsPartOfCountry)
                    {
                        BAOC_Color v_cpy = BAOC_Color.FromColor(TXT_SideBarArmy.color);
                        v_cpy.A = 1f;

                        TXT_SideBarArmy.color = v_cpy.GetColor();

                        v_cpy = BAOC_Color.FromColor(TXT_RecruitButtonText.color);
                        v_cpy.A = 1f;

                        TXT_RecruitButtonText.color = v_cpy.GetColor();
                    }
                    else
                    {
                        BAOC_Color v_cpy = BAOC_Color.FromColor(TXT_SideBarArmy.color);
                        v_cpy.A = 0.5f;

                        TXT_SideBarArmy.color = v_cpy.GetColor();

                        v_cpy = BAOC_Color.FromColor(TXT_RecruitButtonText.color);
                        v_cpy.A = 0.5f;

                        TXT_RecruitButtonText.color = v_cpy.GetColor();


                    }
                    #endregion

                    LastProvinceSelectedByName = SelectedProvince.Name;
                    LastProvinceSelected = SelectedProvince.Color;

                    UpdateSideBarUI();

                    LastProvinceByName = TXT_ProvinceName.text;
                    LastProvinceColor = MouseTarget;

                    DeclareWarBoolean = GameData.Country == SelectedProvince.Country;
                    SetDeclareWarButtonInteractable(!DeclareWarBoolean);

                    #region boja UI elemenata
                    SetDeclareWarTextAlpha(DeclareWarBoolean ? 0.5f : 1f);

                    TXT_RelationsLabel.color = DeclareWarBoolean ? BAOC_Color.FromColor(TXT_RelationsLabel.color).SetComponent(Channel.ALPHA, 0.5f).GetColor() : BAOC_Color.FromColor(TXT_RelationsLabel.color).SetComponent(Channel.ALPHA, 1f).GetColor();
                    TXT_RelationsValue.color = DeclareWarBoolean ? BAOC_Color.FromColor(TXT_RelationsLabel.color).SetComponent(Channel.ALPHA, 0.5f).GetColor() : BAOC_Color.FromColor(TXT_RelationsLabel.color).SetComponent(Channel.ALPHA, 1f).GetColor();
                    #endregion

                    UpdateWarText();
                    UpdateProvinceUI();
                    UpdateSideBarUI();
                    UpdateRichPresence();
                }
                LastProvinceByName = data.Name;
                LastProvinceColor = data.Color;

                #region provincija hover
                //ako miš nije iznad SelectedProvince
                if (MouseTarget != SelectedProvince.Color && MouseTarget != BAOC_Color.black)
                {
                    LightUpProvince(MouseTarget, BCLR_ProvinceHoverHighlight);
                }
                #endregion
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
    public void NextMove()
    {
        GameData.Move++;
        GameData.Balance += GameData.Income;
        UpdateMoveUI();
        UpdateRichPresence();
    }
    #endregion
    #region util metode


#nullable enable
    public List<ProvinceData> ProvincesByBAOCColorsToProvincesByProvinceData(List<BAOC_Color> BAOC_Colors)
    {
        List<ProvinceData> __Provinces = new List<ProvinceData>();
        foreach (BAOC_Color item in BAOC_Colors)
        {
            __Provinces.Add(Provinces[item]);
        }
        return __Provinces;
    }
    public Dictionary<BAOC_Color, Vector2Int> GetProvinceColorsNearToSpecialColor()
    {
        Dictionary<BAOC_Color, Vector2Int> ret = new Dictionary<BAOC_Color, Vector2Int>();

        List<Vector2Int>? __positions = PixelPositionsByColor[BAOC_Color.special0];
        foreach (Vector2Int position in __positions!)
        {
            ret[BAOC_Color.FromColor(TEX_ColourMap.GetPixel(position.x + 1, position.y))] = position;
        }

        return ret;
    }
    public BAOC_Color GetMouseColor(Vector2Int mouse_pos)
    {
        return BAOC_Color.FromColor(TEX_ColourMap.GetPixel((int)mouse_pos.x - (int)V2_SideSize.x, (int)mouse_pos.y - (int)V2_SideSize.y));
    }
    public Vector3 TexturePointToWorld(Vector2Int point)
    {
        //return CM_MainCamera.ScreenToWorldPoint(new Vector3(point.x, point.y));
        return CM_MainCamera.ScreenToWorldPoint(new Vector3(point.x + V2_SideSize.x, point.y + V2_SideSize.y, 0));
    }
    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
    public static string ProvinceLowercase(string name)
    {
        string key__ = "";
        if (name.Contains("-"))
        {
            key__ = name.Split('-')[0].ToLower();
        }
        else if (name.Contains(" "))
        {
            key__ = name.Split(' ')[0].ToLower();
        }
        else
        {
            key__ = name.ToLower();
        }
        return key__;
    }
    public static List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }
        return children;
    }
    public static string ToJSON(object? val, bool format = true)
    {
        return JsonConvert.SerializeObject(val, format ? Formatting.Indented : Formatting.None);
    }

    ///<summary>
    ///path = SavePath + path to file without starting / and filetype
    ///</summary>

    public void SaveJSON(object? val, string path)
    {
        File.WriteAllText($"{SavePath}{path}.json", ToJSON(val));
    }
#nullable disable
    public void LightUpProvince(BAOC_Color province, BAOC_Color with_BAOC_Color)
    {
        if (Provinces.ContainsKey(province))
        {
            string name = Provinces[province].Name;
            int index = ProvinceHolderChildIndexesByProvinceName[name];
            TF_ProvinceHolder.GetChild(index).GetComponent<RawImage>().color = with_BAOC_Color.GetColor();
        }
    }
    public void LightUpProvinces(IEnumerable<BAOC_Color> provinces, BAOC_Color with)
    {
        foreach (BAOC_Color province in provinces)
        {
            string name = Provinces[province].Name;


            LightUpProvince(province, with);
        }
    }
    public void ClearProvince(BAOC_Color province)
    {
        LightUpProvince(province, BAOC_Color.clear);
    }
    public void ClearAllProvinces()
    {
        foreach (BAOC_Color p in Provinces.Keys)
        {
            ClearProvince(p);
        }
    }
    public void ClearAllProvincesExcept(BAOC_Color[] exceptions)
    {
        foreach (BAOC_Color p in Provinces.Keys)
        {
            if (!exceptions.Contains(p))
            {
                ClearProvince(p);
            }
        }
    }
    public string BAOCColorToRGBString(BAOC_Color c)
    {
        return String.Format("r: {0}, g: {1}, b: {2}", (int)(c.R * 255), (int)(c.G * 255), (int)(c.B * 255));
    }
    /// <summary>
    /// vra?a epoch time u sekundama
    /// </summary>
    /// <returns>epoch time u sekundama</returns>
    public static ulong GetEpoch()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        int secondsSinceEpoch = (int)t.TotalSeconds;
        return (ulong)secondsSinceEpoch;
    }
    public RPCData GetRPCData()
    {
        RPCData activity = new RPCData();
        activity.state = $"U igri ({GameData.Country})";
        activity.details = $"{CountriesByName[GameData.Country].Provinces.Count} " +
                           $"pokrajine | Potez {GameData.Move}";

        RPC_Timestamps timestamps = new RPC_Timestamps();
        timestamps.start = GetEpoch();
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
    public void InitializeRichPresence()
    {
        RPC.GameInstance = this;

        RPC.Init();
        UpdateRichPresence();
    }
    public void UpdateRichPresence()
    {
        RPCData activity = GetRPCData();
        RPC.SetActivity(activity);
    }
    public Vector3 Vector2IntToVector3(Vector2Int v2int)
    {
        return new Vector3(v2int.x, v2int.y);
    }
    public Vector2 Vector2IntToVector2(Vector2Int v2int)
    {
        return new Vector2(v2int.x, v2int.y);
    }
    public string FormatNumber(int num)
    {
        return string.Format("{0:N0}", num);
    }
    public void ChangeProvinceOwner(ProvinceData data, string owner)
    {
        Provinces[data.Color].Country = owner;
        UpdateProvinces();
    }
    public void SetDeclareWarButtonInteractable(bool interactable)
    {
        TF_DeclareWar.GetComponent<Button>().interactable = interactable;
    }
    public void SetDeclareWarTextAlpha(float a)
    {
        BAOC_Color clr = BAOC_Color.FromColor(TF_DeclareWar.GetChild(0).GetComponent<TextMeshProUGUI>().color);
        TF_DeclareWar.GetChild(0).GetComponent<TextMeshProUGUI>().color = new BAOC_Color(clr.R, clr.G, clr.B, a).GetColor();

    }
    public bool AtWar(string country, string with = "NONE")
    {
        foreach (WarData war in GameData.Wars)
        {
            if (war.Defender == country || war.Aggresor == country)
            {
                return true;
            }
            if ((war.Defender == country || war.Aggresor == with) & (war.Defender == with || war.Aggresor == country))
            {
                return true;
            }
        }
        return false;
    }
    public void UpdateMoveUI()
    {
        TXT_Balance.text = FormatNumber(GameData.Balance);
        TXT_Move.text = "Potez " + FormatNumber(GameData.Move);
        TXT_Income.text = GameData.Income > 0 ? String.Format("+{0:N0}", GameData.Income) : FormatNumber(GameData.Income);
        TXT_SideBarArmy.text = FormatNumber(GameData.Army);
    }
    public void UpdateProvinceUI()
    {
        TXT_ProvinceName.text = SelectedProvince.Name;
        TXT_SideBarProvincePopulation.text = $"{FormatNumber(SelectedProvince.Population)} ljudi";
        TXT_SideBarProvinceArmy.text = $"{FormatNumber(SelectedProvince.Army)} vojnika";
    }
    public void EraseLastProvince()
    {
        ClearProvince(LastProvinceColor);
    }
    public void ShowRecruitGUI()
    {
        S_Slider.maxValue = SelectedProvince.HomogenousPeople;
        S_Slider.minValue = 0;
        TXT_ArmyText.text = FormatNumber(Mathf.RoundToInt(S_Slider.value));
        Price = Mathf.RoundToInt(S_Slider.value) * CInt_SoldierPrice;

        TXT_ArmyPriceText.text = String.Format("{0:N0} $", Price);
        TXT_ArmyPriceText.color = Price > GameData.Balance ? GR_RelationsGradient.Evaluate(1f) : GR_RelationsGradient.Evaluate(0f);
    }
    public void ShowColorPick()
    {
        print(ChangedSliders);
        if (!OriginalSet)
        {
            BAOC_Color c = BAOC_Color.FromColor(TF_CountriesByColor.GetChild(CountriesByName[CurrentProvince.Country].Index).GetComponent<RawImage>().color);
            Original = c;
            OriginalSet = true;
        }
        if (ChangedSliders)
        {
            BAOC_Color _c = BAOC_Color.HSVToRGB(S_SliderHue.value, 0.4f, S_SliderValue.value);
            TF_CountriesByColor.GetChild(CountriesByName[CurrentProvince.Country].Index).GetComponent<RawImage>().color = _c.GetColor();
        }
    }
    public void UpdateProvinces()
    {
        SelectedProvince = Provinces[SelectedProvince.Color];
        CurrentProvince = Provinces[CurrentProvince.Color];
    }
    public void EndRecruit()
    {
        S_Slider.gameObject.SetActive(false);
        GameData.Army += (int)S_Slider.value;
        Provinces[SelectedProvince.Color].Army += (int)S_Slider.value;
        GameData.Balance -= Price;
        Provinces[SelectedProvince.Color].HomogenousPeople -= (int)(S_Slider.value / CFloat_RecruitLossFactor);
        UpdateProvinces();
        UpdateProvinceUI();
        UpdateMoveUI();
    }
    public void EndColorPick()
    {
        S_SliderHue.transform.parent.gameObject.SetActive(false);
        print(TF_CountriesByColor.GetChild(CountriesByName[CurrentProvince.Country].Index).name);
    }
    public void AbortColorPick()
    {
        S_SliderHue.transform.parent.gameObject.SetActive(false);
        print(TF_CountriesByColor.GetChild(CountriesByName[CurrentProvince.Country].Index).name);
        TF_CountriesByColor.GetChild(CountriesByName[CurrentProvince.Country].Index).GetComponent<RawImage>().color = Original.GetColor();
    }
    public void SetSlidersChanged(bool to)
    {
        ChangedSliders = to;
    }
    void UpdateWarText()
    {

        TXT_Wars.text = "<sprite=0>";
        foreach (WarData dom in GameData.Wars)
        {
            if (dom.Aggresor != SelectedProvince.Country)
            {
                TXT_Wars.text += $"<sprite={CountriesByName[dom.Aggresor].Index + 1}>";
            }
            else
            {
                TXT_Wars.text += $"<sprite={CountriesByName[dom.Defender].Index + 1}>";
            }
        }
    }
    public void DeclareWar()
    {
        if (!AtWar(GameData.Country, SelectedProvince.Country))
        {
            WarData wdata = new WarData();

            wdata.Aggresor = GameData.Country;
            wdata.Defender = SelectedProvince.Country;

            GameData.Wars.Add(wdata);

            UpdateWarText();
        }
    }
    void UpdateSideBarUI()
    {
        EthnicData data_eth = EthnicDataByCountryName[SelectedProvince.Country];
        EthnicData data_eth_zup = EthnicDataByProvinceName[SelectedProvince.Name];

        for (int idxi = 0; idxi < data_eth.Size; idxi++)
        {
            TF_CountryEthnicStructure.GetChild(idxi).GetComponent<TextMeshProUGUI>().text = String.Format("{0}: {1}%, {2:N0}", data_eth.PopulationByNation[idxi], data_eth.PopulationPercentages[idxi] * 100f, data_eth.PopulationByQuantity[idxi]);
        }
        for (int idxi = 0; idxi < data_eth_zup.Size; idxi++)
        {
            TF_ProvinceEthnicStructure.GetChild(idxi).GetComponent<TextMeshProUGUI>().text = String.Format("{0}: {1}%, {2:N0}", data_eth_zup.PopulationByNation[idxi], data_eth_zup.PopulationPercentages[idxi] * 100f, data_eth_zup.PopulationByQuantity[idxi]);
        }

        ProvinceData data = SelectedProvince;
        CountryData countryData = CountriesByName[data.Country];
        TXT_ProvinceName.text = data.Name;
        TXT_Country.text = data.Country;
        TXT_SideBarRightCountryName.text = data.Country;
        TXT_SideBarRightProvinceName.text = data.Name;
        if (SelectedProvince.Country == GameData.Country)
        {
            TXT_SideBarArmy.text = FormatNumber(GameData.Army);
        }
        else
        {
            TXT_SideBarArmy.text = FormatNumber(countryData.Army);
        }
        TXT_SideBarPresident.text = countryData.President;
        TXT_SideBarCountry.text = countryData.Name;
        TXT_SideBarCapital.text = countryData.Capital;
        TXT_SideBarPopulation.text = FormatNumber(countryData.Population);
        if (countryData.Relations.ContainsKey(GameData.Country))
        {
            int o_d = countryData.Relations[GameData.Country];
            TXT_RelationsValue.text = o_d > 0 ? $"+{o_d}" : $"{o_d}";
            float o_d_grad = 1f - ((o_d + 100f) / 200f);
            TXT_RelationsValue.color = GR_RelationsGradient.Evaluate(o_d_grad);
        }
        else
        {
            TXT_RelationsValue.text = "";
            TXT_RelationsValue.color = Color.white;
        }
        TXT_ProvinceName.text = data.Name;
    }
    public void ResetSlider()
    {
        S_Slider.value = 0;
    }
    public List<Vector2Int> GetCirclePoints(int radius)
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
    public BAOC_Color GetFirstNonBlackPixelAtAngle(Texture2D tex, Vector2Int origin, float angle, BAOC_Color original)
    {
        List<Vector2Int> points = GetCirclePoints(128);
        Vector2Int p2 = points[(int)((angle / 360f) * points.Count)] + origin;

        Vector2 t = origin;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - origin.x, 2) + Mathf.Pow(p2.y - origin.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
        {
            t = Vector2.Lerp(origin, p2, ctr);
            ctr += frac;
            BAOC_Color col = BAOC_Color.FromColor(tex.GetPixel((int)t.x, (int)t.y));

            if (col != BAOC_Color.black && col != original && Provinces.ContainsKey(col))
            {
                return col;
            }
        }
        return BAOC_Color.black;
    }
    #endregion
}