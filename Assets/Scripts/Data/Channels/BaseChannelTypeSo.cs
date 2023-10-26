using Channels.Type;
using UnityEngine;

namespace Assets.Scripts.Data.Channels
{
    [CreateAssetMenu(fileName = "BaseChannelType", menuName = "Channel/BaseChannelType")]
    public class BaseChannelTypeSo : ScriptableObject
    {
        public ChannelType[] channelTypes;
    }
}