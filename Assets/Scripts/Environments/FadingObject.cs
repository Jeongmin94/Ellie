using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environments
{
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
    {
        public List<Renderer> Renderers = new();
        public Vector3 Position;
        public List<Material> Materials = new();

        [HideInInspector] public float InitialAlpha;

        private void Awake()
        {
            Position = transform.position;

            if (Renderers.Count == 0)
            {
                Renderers.AddRange(GetComponentsInChildren<Renderer>());
            }

            foreach (var renderer in Renderers)
            {
                Materials.AddRange(renderer.materials);
            }

            InitialAlpha = Materials[0].color.a;
        }

        public bool Equals(FadingObject other)
        {
            return Position.Equals(other.Position);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}