using System.Collections.Generic;
using Assets.Scripts.Utils;
using Channels;
using Channels.Components;
using Channels.Type;
using Channels.Utils;
using Data.Channels;
using UnityEngine;

namespace Centers
{
    /// <summary>
    /// 각 씬에 존재하는 객체들 사이의 구독 진행
    /// </summary>
    public class BaseCenter : MonoBehaviour
    {
        [SerializeField] private ChannelTypesSo channelTypesSo;

        private readonly IDictionary<ChannelType, BaseEventChannel<IBaseEventPayload>> channels =
            new Dictionary<ChannelType, BaseEventChannel<IBaseEventPayload>>();
        
        protected virtual void Init()
        {
            // !TODO: channelTypes 기반으로 씬에 필요한 채널들 생성
            // !TODO: 씬의 오브젝트들이 가지고 있는 구독 정보 기반으로 채널 구독
            InitChannels();
            SubscribeChannels();
        }

        private void InitChannels()
        {
            int length = channelTypesSo.channelTypes.Length;
            for (int i = 0; i < length; i++)
            {
                ChannelType type = channelTypesSo.channelTypes[i];
                channels[type] = ChannelUtil.MakeChannel(type);
            }
        }

        private void SubscribeChannels()
        {
            // example
            var go = new GameObject();
            var cc = go.GetOrAddComponent<ChannelComponent>();
        }
    }
}