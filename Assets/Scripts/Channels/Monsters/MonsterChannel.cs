using System.Collections.Generic;
using Channels;
using UnityEngine;

public class MonsterPayload : IBaseEventPayload
{
    public float RespawnTime { get; set; }
    public Transform Monster { get; set; }
    public List<int> ItemDrop { get; set; }
}

public class MonsterChannel : BaseEventChannel
{
    public override void ReceiveMessage(IBaseEventPayload payload)
    {
        Publish(payload);
    }
}