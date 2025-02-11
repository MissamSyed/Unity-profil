using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuTest : MonoBehaviour
{
    VisualElement main = null;
    VisualElement settings = null;

    VisualElement root;

    // Start is called before the first frame update
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    void Start()
    {
        Button settingsButton = root.Q<Button>("Settings_button");
        Button mainButton = root.Q<Button>("Exit");

        settings = root.Q<VisualElement>("settingbackground");
        main = root.Q<VisualElement>("Exit");

        mainButton.RegisterCallback<ClickEvent>(x => settings.style.display = DisplayStyle.None);
        settingsButton.RegisterCallback<ClickEvent>(x => settings.style.display = DisplayStyle.Flex);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
