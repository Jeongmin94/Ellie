using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monster;
using Channels;
using Channels.UI;
using UnityEngine;

public class MonsterPayload : IBaseEventPayload
{
    public float RespawnTime { get; set; }
    public Transform Monster { get; set; }
    public List<int> ItemDrop { get; set; }
}

public class MonsterChannel:BaseEventChannel
{

    public override void ReceiveMessage(IBaseEventPayload payload)
    {
        Publish(payload);
        return;
    }
}
