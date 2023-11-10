using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseRange : Poolable
{
	// 관리하는 범위 오브젝트
	protected GameObject rangeObject;

	public Material DetectionMaterial { get; protected set; }

    private const float RAYCAST_OFFSET = 2.0f;
	private const float POSITION_OFFSET = 0.3f;

	public BaseRange(Transform objTransform, bool isSetParent = false)
	{
        rangeObject = new GameObject();

		if(isSetParent)
		{
			InitSetParent(objTransform);
		}
		else
		{
			Init(objTransform);
		}
	}

	private void Init(Transform objTransform)
	{
        // Ground 레이어를 가지고 있는 오브젝트를 검출하는 레이어 마스크를 생성합니다.
        int groundLayer = LayerMask.GetMask("Ground");

        // -Vector3.up 방향으로 Raycast를 발사합니다.
        RaycastHit hit;
        Vector3 checkPosition = objTransform.position + new Vector3(0, RAYCAST_OFFSET, 0);
        if (Physics.Raycast(checkPosition, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            // Raycast가 Ground 레이어를 가지고 있는 오브젝트에 맞았다면, 그 위치의 위에 오브젝트를 생성합니다.
            objTransform.position = hit.point + new Vector3(0, POSITION_OFFSET, 0);
        }

        rangeObject.transform.position = objTransform.position;
        rangeObject.transform.rotation = objTransform.rotation;
    }

	private void InitSetParent(Transform objTransform)
	{
        rangeObject.transform.SetParent(objTransform, false); // 지정된 부모 오브젝트의 로컬 좌표를 따릅니다.
        rangeObject.transform.localPosition = new Vector3(0, POSITION_OFFSET, 0); // 부모 오브젝트의 위치에 맞춥니다.
        rangeObject.transform.localRotation = Quaternion.identity; // 부모 오브젝트의 회전에 맞춥니다.
    }

	public abstract void CreateRange(RangePayload payload);
	public abstract List<Transform> CheckRange(string checkTag = null, int layerMask = -1);
}
