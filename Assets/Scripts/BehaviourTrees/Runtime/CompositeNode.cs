using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    [Serializable]
    public abstract class CompositeNode : Node
    {
        [HideInInspector] [SerializeReference] public List<Node> children = new();
    }
}