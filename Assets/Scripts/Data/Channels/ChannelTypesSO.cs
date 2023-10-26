using Channels.Type;
using UnityEngine;

namespace Assets.Scripts.Data.Channels
{
    [CreateAssetMenu(fileName = "ChannelTypes", menuName = "Channel/ChannelTypes")]
    public class ChannelTypesSo : ScriptableObject
    {
        public ChannelType[] channelTypes;
    }
}