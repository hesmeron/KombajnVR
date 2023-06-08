using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "VRPlugin/InteractionConfig", fileName = "InteractionConfig#")]
public class InteractionConfig : ScriptableObject
{
    private byte _interactionMask;
    [SerializeField]
    private Interaction[] _interactions = new Interaction[32];

    void Test()
    {
        _interactionMask |= 1 << 1;
    }
}
