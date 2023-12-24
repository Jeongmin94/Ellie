using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Environments
{
    public class FadeObjectBlockingObject : MonoBehaviour
    {
        [SerializeField] private LayerMask LayerMask;

        [SerializeField] private Transform Target;

        [SerializeField] private Camera Camera;

        [SerializeField] [Range(0, 1f)] private float FadedAlpha = 0.33f;

        [SerializeField] private bool RetainShadows = true;

        [SerializeField] private Vector3 TargetPositionOffset = Vector3.up;

        [SerializeField] private float FadeSpeed = 1;

        [Header("Read Only Data")] [SerializeField]
        private List<FadingObject> ObjectsBlockingView = new();

        private readonly RaycastHit[] Hits = new RaycastHit[10];
        private readonly Dictionary<FadingObject, Coroutine> RunningCoroutines = new();

        private void OnEnable()
        {
            StartCoroutine(CheckForObjects());
        }

        private void OnDrawGizmos()
        {
            var rayColor = Color.red;
            Gizmos.color = rayColor;

            Gizmos.DrawRay(Camera.transform.position, (Target.position - Camera.transform.position) * 10f);
        }

        private IEnumerator CheckForObjects()
        {
            while (true)
            {
                var hits = Physics.RaycastNonAlloc(
                    Camera.transform.position,
                    (Target.transform.position + TargetPositionOffset - Camera.transform.position).normalized,
                    Hits,
                    Vector3.Distance(Camera.transform.position, Target.transform.position + TargetPositionOffset),
                    LayerMask
                );

                if (hits > 0)
                {
                    for (var i = 0; i < hits; i++)
                    {
                        var fadingObject = GetFadingObjectFromHit(Hits[i]);

                        if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                        {
                            if (RunningCoroutines.ContainsKey(fadingObject))
                            {
                                if (RunningCoroutines[fadingObject] != null)
                                {
                                    StopCoroutine(RunningCoroutines[fadingObject]);
                                }

                                RunningCoroutines.Remove(fadingObject);
                            }

                            RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                            ObjectsBlockingView.Add(fadingObject);
                        }
                    }
                }

                FadeObjectsNoLongerBeingHit();

                ClearHits();

                yield return null;
            }
        }

        private void FadeObjectsNoLongerBeingHit()
        {
            var objectsToRemove = new List<FadingObject>(ObjectsBlockingView.Count);

            foreach (var fadingObject in ObjectsBlockingView)
            {
                var objectIsBeingHit = false;
                for (var i = 0; i < Hits.Length; i++)
                {
                    var hitFadingObject = GetFadingObjectFromHit(Hits[i]);
                    if (hitFadingObject != null && fadingObject == hitFadingObject)
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }

                if (!objectIsBeingHit)
                {
                    if (RunningCoroutines.ContainsKey(fadingObject))
                    {
                        if (RunningCoroutines[fadingObject] != null)
                        {
                            StopCoroutine(RunningCoroutines[fadingObject]);
                        }

                        RunningCoroutines.Remove(fadingObject);
                    }

                    RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                    objectsToRemove.Add(fadingObject);
                }
            }

            foreach (var removeObject in objectsToRemove)
            {
                ObjectsBlockingView.Remove(removeObject);
            }
        }

        private IEnumerator FadeObjectOut(FadingObject FadingObject)
        {
            foreach (var material in FadingObject.Materials)
            {
                material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.SetInt("_Surface", 1);

                material.renderQueue = (int)RenderQueue.Transparent;

                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", RetainShadows);

                material.SetOverrideTag("RenderType", "Transparent");

                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            float time = 0;

            while (FadingObject.Materials[0].color.a > FadedAlpha)
            {
                foreach (var material in FadingObject.Materials)
                {
                    if (material.HasProperty("_Color"))
                    {
                        material.color = new Color(
                            material.color.r,
                            material.color.g,
                            material.color.b,
                            Mathf.Lerp(FadingObject.InitialAlpha, FadedAlpha, time * FadeSpeed)
                        );
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }

            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }

        private IEnumerator FadeObjectIn(FadingObject FadingObject)
        {
            float time = 0;

            while (FadingObject.Materials[0].color.a < FadingObject.InitialAlpha)
            {
                foreach (var material in FadingObject.Materials)
                {
                    if (material.HasProperty("_Color"))
                    {
                        material.color = new Color(
                            material.color.r,
                            material.color.g,
                            material.color.b,
                            Mathf.Lerp(FadedAlpha, FadingObject.InitialAlpha, time * FadeSpeed)
                        );
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }

            foreach (var material in FadingObject.Materials)
            {
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.SetInt("_Surface", 0);

                material.renderQueue = (int)RenderQueue.Geometry;

                material.SetShaderPassEnabled("DepthOnly", true);
                material.SetShaderPassEnabled("SHADOWCASTER", true);

                material.SetOverrideTag("RenderType", "Opaque");

                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }

        private void ClearHits()
        {
            Array.Clear(Hits, 0, Hits.Length);
        }

        private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
        {
            return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
        }
    }
}