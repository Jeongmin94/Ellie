using Channels.Type;
using UnityEngine;

namespace Data.Channels
{
    [CreateAssetMenu(fileName = "ChannelTypes", menuName = "Channel/ChannelTypes")]
    public class ChannelTypesSo : ScriptableObject
    {
        public ChannelType[] channelTypes;
    }
}