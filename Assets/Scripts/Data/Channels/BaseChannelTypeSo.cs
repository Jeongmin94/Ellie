using Channels.Type;
using UnityEngine;

namespace Data.Channels
{
    [CreateAssetMenu(fileName = "BaseChannelType", menuName = "Channel/BaseChannelType")]
    public class BaseChannelTypeSo : ScriptableObject
    {
        public ChannelType[] channelTypes;
    }
}