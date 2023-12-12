using Assets.Scripts.Centers;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.Data.UI.Video
{
    [CreateAssetMenu(fileName = "VideoData", menuName = "UI/VideoData")]
    public class VideoData : ScriptableObject
    {
        [SerializeField] public VideoClip videoClip;
        [SerializeField] public SceneName playAfterScene;
        [SerializeField] public bool isLoadData;
    }
}