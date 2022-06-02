using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PieChart
{
    public GameObject pieChartTemplate;
    public List<Color> colors;

    public int entries = 0;
    public PieChart(GameObject template, List<Color> prefered_colors)
    {
        pieChartTemplate = template;
        colors = prefered_colors;
    }
    public GameObject AddEntry(string name, float value, bool cw) 
    {
        entries++;
        GameObject modified = GameObject.Instantiate(pieChartTemplate) as GameObject;

        modified.SetActive(true);

        modified.GetComponent<Image>().color = colors[entries-1];
        modified.GetComponent<Image>().fillAmount = value;
        modified.GetComponent<Image>().fillClockwise = cw;

        modified.name = name;
        modified.transform.SetParent(pieChartTemplate.transform.parent);

        return modified;
    }
}
