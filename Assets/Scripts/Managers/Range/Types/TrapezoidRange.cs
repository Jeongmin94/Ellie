using System.Collections.Generic;
using UnityEngine;

namespace Managers.Range.Types
{
    public class TrapezoidRange : BaseRange
    {
        public float Height { get; private set; }
        public float UpperBase { get; private set; }
        public float LowerBase { get; private set; }

        public override void Init(GameObject rangeObject, RangePayload payload)
        {
            base.Init(rangeObject, payload);

            RangeObject.name = "TrapezoidRange";
        }

        public override void CreateRange(RangePayload payload)
        {
            Height = payload.Height;
            UpperBase = payload.UpperBase;
            LowerBase = payload.LowerBase;

            DetectionMaterial = payload.DetectionMaterial;

            // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
            var meshFilter = RangeObject.AddComponent<MeshFilter>();
            var meshRenderer = RangeObject.AddComponent<MeshRenderer>();

            if (!IsShowRange)
            {
                return;
            }

            var mesh = new Mesh();

            // 사다리꼴의 네 꼭지점을 정의합니다.
            var vertices = new Vector3[4];

            vertices[0] = new Vector3(-UpperBase / 2, 0, 0); // 왼쪽 아래 꼭지점
            vertices[1] = new Vector3(UpperBase / 2, 0, 0); // 오른쪽 아래 꼭지점
            vertices[2] = new Vector3(-LowerBase / 2, 0, Height); // 왼쪽 위 꼭지점
            vertices[3] = new Vector3(LowerBase / 2, 0, Height); // 오른쪽 위 꼭지점

            // 두 개의 삼각형을 이어서 사다리꼴을 만듭니다.
            var triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            triangles[3] = 2;
            triangles[4] = 3;
            triangles[5] = 1;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;

            meshRenderer.material = DetectionMaterial;
        }

        public override List<Transform> CheckRange(string checkTag = "", int layerMask = -1)
        {
            var objTransform = RangeObject.transform;

            // 사다리꼴의 너비의 절반을 계산합니다.
            var halfWidthAtBase = UpperBase / 2f;
            var halfWidthAtTop = LowerBase / 2f;

            // 사다리꼴의 넓이를 계산합니다.
            var area = (UpperBase + LowerBase) * Height / 2f;

            // 사다리꼴의 중심과 방향을 계산합니다.
            var center = objTransform.position + objTransform.forward * (Height / 2f);
            var direction = objTransform.rotation;

            // 사다리꼴 범위 내의 모든 콜라이더를 감지합니다.
            var collidersInTrapezoid = Physics.OverlapSphere(center, Mathf.Sqrt(area), layerMask);

            var targets = new List<Transform>();
            foreach (var collider in collidersInTrapezoid)
            {
                if (checkTag == "" || collider.CompareTag(checkTag))
                {
                    // 이 콜라이더의 위치가 사다리꼴 범위 내에 있는지 확인합니다.
                    var localPoint = objTransform.InverseTransformPoint(collider.transform.position);
                    if (localPoint.z > 0 && localPoint.z < Height)
                    {
                        var halfWidthAtThisPoint = Mathf.Lerp(halfWidthAtBase, halfWidthAtTop, localPoint.z / Height);
                        if (Mathf.Abs(localPoint.x) < halfWidthAtThisPoint)
                        {
                            targets.Add(collider.transform);
                        }
                    }
                }
            }

            return targets;
        }
    }
}