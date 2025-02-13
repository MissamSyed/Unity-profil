using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]

public class ButtonEvent
{
    [SerializeField] string _buttonName = "";
    [SerializeField] UnityEvent _unityEvent;
    Button _button;


    public void Activate(UIDocument document)
    {
        if (_button == null)
        {
            _button = document.rootVisualElement.Q<Button>(_buttonName);

        }

        _button.clicked += _unityEvent.Invoke;

    }


    public void Inactivate(UIDocument document)
    {
        _button.clicked -= _unityEvent.Invoke;
    }
}
    public class SoundSelect : MonoBehaviour
    {
        [SerializeField] UIDocument _document;
        [SerializeField] List<ButtonEvent> _buttonEvents;
        private void OnEnable()
        {
            _buttonEvents.ForEach(Button => Button.Activate(_document));
        }

        private void Ondisable()
        {
            _buttonEvents.ForEach(Button => Button.Inactivate(_document));
        }
    }
