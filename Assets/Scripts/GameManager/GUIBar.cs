using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GUIBar : MonoBehaviour
{
    [SerializeField]
    public Image bar;

    [System.NonSerialized]
    public int currentAmount = 100;
    [System.NonSerialized]
    public int maxAmount = 100;



    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = cur / maxAmount;
    }
}
