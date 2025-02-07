using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Items", order = 1)]
public class NewBehaviourScript1 : ScriptableObject
{
    [SerializeField] Texture2D _texture = null;
    public Texture2D GetTexture()
    {
        return _texture;
    }
}
