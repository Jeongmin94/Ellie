using Assets.Scripts.Managers;
using Channels.Boss;
using Sirenix.OdinInspector;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;

public class BossRoomDoorKnob : MonoBehaviour
{
    [SerializeField] private Transform doorKnob;
    [SerializeField] private float openSpeedTime;
    [ReadOnly][SerializeField] private bool isChecked = false;

    private Transform golemCore;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if(isChecked == false && other.CompareTag("Stone"))
        {
            EventBus.Instance.Publish(EventBusEvents.BossRoomDoorOpen, new BossEventPayload
            {
                Sender = transform,
                TransformValue1 = other.transform,
            });
        }
    }

    public void Init(Transform golemCoreStone)
    {
        golemCoreStone.SetParent(doorKnob);
        golemCoreStone.localPosition = Vector3.zero;

        Rigidbody coreRigidbody = golemCoreStone.GetComponent<Rigidbody>();
        isChecked = true;
        coreRigidbody.isKinematic = true;
        coreRigidbody.velocity = Vector3.zero;
        coreRigidbody.useGravity = false;
    }

    public void OpenDoor(float openAngle, float openTime)
    {
        openSpeedTime = openTime;

        StartCoroutine(OpenDoorRoutine(openAngle));
    }

    private IEnumerator OpenDoorRoutine(float openAngle)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);

        float elapsed = 0.0f;
        while (elapsed < openSpeedTime)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / openSpeedTime);
            yield return null;
        }

        transform.rotation = endRotation;
    }
}
