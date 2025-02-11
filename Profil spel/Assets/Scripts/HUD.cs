using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    Sprite[] allSprites;

    VisualElement rootvis;
    VisualElement AmoCounter;

    private void OnEnable()
    {
        rootvis = GetComponent<UIDocument>().rootVisualElement;
        AmoCounter = rootvis.Q<VisualElement>("Ammo_UI");
        allSprites = Resources.LoadAll<Sprite>("DebuggunMagUI_32x32_SH");
    }

    void Start()
    {
        //SetAmoCount(24);
    }

    public void SetAmoCount(int count)
    {
        AmoCounter.style.backgroundImage = Background.FromSprite(allSprites[30 - count]);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
