using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        public enum SoundType
        {
            Bgm,
            Sfx,
            UISfx,
            End
        }
        //풀 생성 후, 플레이하고자 하는 bgm을 딕셔너리에서 찾아서 오디오 소스에 붙여서 해당 위치로 이동시킨 후 플레이
        private Pool audioSourcePool;

        private AudioController audioControllerPrefab;
        //오디오 클립들이 담긴 딕셔너리
        private Dictionary<string, AudioClip> AudioClips = new();
        
        private Coroutine nowPlayingBgmCoroutine;
        private AudioController nowPlayingBgmController;
        private bool isBgmPlaying = false;
        private bool isBgmPaused = false;

        private Coroutine nowPlayingUISfxCoroutine;

        //volmues
        private float BGMVolume = 1.0f;
        private float SFXVolume = 1.0f;
        private float UISfxVolume = 1.0f;


        public bool IsBgmPlaying() => isBgmPlaying;

        public override void Awake()
        {
            base.Awake();
            InitAudioDict();
            InitAudioSourcePool();
        }
        private void InitAudioDict()
        {
            audioControllerPrefab = Resources.Load<AudioController>("Sounds/AudioController");
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
            foreach (AudioClip clip in clips)
            {
                AudioClips.Add(clip.name, clip);
            }
        }
        private void InitAudioSourcePool()
        {
            audioSourcePool = PoolManager.Instance.CreatePool(audioControllerPrefab.gameObject, 10);
        }

        public void PlaySound(SoundType type, string name, Vector3 playingPos = default(Vector3), float pitch = 1.0f)
        {
            switch (type)
            {
                case SoundType.Bgm:
                    if (AudioClips.TryGetValue(name, out AudioClip bgmClip))
                    {
                        AudioController audioController = audioSourcePool.Pop() as AudioController;

                        //플레이 중인 Bgm이 이미 존재한다면
                        if (isBgmPlaying && nowPlayingBgmCoroutine != null)
                        {
                            StopCoroutine(nowPlayingBgmCoroutine);
                            nowPlayingBgmController.Stop();
                            audioSourcePool.Push(nowPlayingBgmController);
                        }
                        isBgmPlaying = true;
                        nowPlayingBgmController = audioController;
                        nowPlayingBgmCoroutine = StartCoroutine(PlaySoundCoroutine(type, name, audioController, bgmClip, pitch));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
                case SoundType.Sfx:
                    if (AudioClips.TryGetValue(name, out AudioClip sfxClip))
                    {
                        AudioController audioController = audioSourcePool.Pop() as AudioController;
                        //audioController의 위치를 해당 위치로
                        audioController.Activate3DEffect();
                        audioController.transform.position = playingPos;
                        StartCoroutine(PlaySoundCoroutine(type, name, audioController, sfxClip, pitch));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
                case SoundType.UISfx:
                    //해당 사운드가 현재 재생중인지 먼저 체크
                    if (nowPlayingUISfxCoroutine != null)
                    {
                        //이미 재생중이라면 재생하지 않음
                        Debug.Log("NowPlayingUISfxCoroutine이 null이 아님");
                        return;
                    }

                    if (AudioClips.TryGetValue(name, out AudioClip uiSfxClip))
                    {
                        AudioController audioController = audioSourcePool.Pop() as AudioController;

                        nowPlayingUISfxCoroutine = StartCoroutine(PlaySoundCoroutine(type, name, audioController, uiSfxClip, pitch));
                    }
                    else
                    {
                        Debug.Log($"{name}사운드가 존재하지 않습니다.");
                        return;
                    }
                    break;
            }
        }

        public void SetVolume(SoundType type, float volume)
        {
            switch (type)
            {
                case SoundType.Bgm:
                    BGMVolume = volume;
                    break;
                case SoundType.Sfx:
                    SFXVolume = volume;
                    break;
                case SoundType.UISfx:
                    UISfxVolume = volume;
                    break;
                default:
                    break;
            }
        }

        private void StopSound(string name, AudioController nowPlayingAudioController)
        {
            nowPlayingAudioController.Deactivate3DEffect();
            audioSourcePool.Push(nowPlayingAudioController);
        }

        public void StopBgm()
        {
            if (!isBgmPlaying) return;
            isBgmPlaying = false;
            isBgmPaused = false;
            //코루틴 꺼주기
            nowPlayingBgmController.Stop();
            StopCoroutine(nowPlayingBgmCoroutine);

            audioSourcePool.Push(nowPlayingBgmController);
        }

        public void PauseBgm()
        {
            if (!isBgmPlaying) return;
            isBgmPaused = true;
        }

        public void ResumeBgm()
        {
            isBgmPaused = false;
        }
        private IEnumerator PlaySoundCoroutine(SoundType type, string name, AudioController audioController, AudioClip clip, float pitch)
        {
            audioController.SetClip(clip);
            audioController.Play(pitch, type);
            if (type != SoundType.Bgm)
            {
                yield return new WaitForSeconds(clip.length);
                if(type == SoundType.UISfx)
                {
                    nowPlayingUISfxCoroutine = null;
                }
                StopSound(name, audioController);
            }
            else
            {
                while (true)
                {
                    if(isBgmPaused && !audioController.isPaused)
                    {
                        audioController.Pause();
                    }

                    if(!isBgmPaused && audioController.isPaused)
                    {
                        audioController.Resume();
                    }

                    //bgm 실행중에는 무한루프
                    yield return null;
                }
            }
        }
    }
}
