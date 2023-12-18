using Assets.Scripts.Particle;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;

public class BossRoomDoorKnob : MonoBehaviour
{
    [SerializeField][Required] private GameObject emphasizeEffect;
    [SerializeField][Required] private Transform doorKnob;
    [SerializeField][ReadOnly] private bool isChecked = false;
    [SerializeField][ReadOnly] private float openSpeedTime;

    private Action<BossRoomDoorKnob, Transform> golemCoreCheckAction;
    private ParticleController particle;

    public void SubScribeAction(Action<BossRoomDoorKnob, Transform> action)
    {
        golemCoreCheckAction -= action;
        golemCoreCheckAction += action;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isChecked == false && other.CompareTag("Stone"))
        {
            golemCoreCheckAction?.Invoke(this, other.transform);
        }
    }

    public void EmphasizedDoor()
    {
        if(!isChecked)
        {
            var temp = new ParticlePayload { Origin = doorKnob, IsFollowOrigin = true, IsLoop = true };
            particle = ParticleManager.Instance.GetParticle(emphasizeEffect, temp).GetComponent<ParticleController>();
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

        if(particle)
        {
            particle.Stop();
            particle = null;
        }
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
