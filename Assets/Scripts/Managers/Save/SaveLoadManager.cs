using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [ShowInInspector] [ReadOnly] private Action saveAction;
        [ShowInInspector] [ReadOnly] private Dictionary<SaveLoadType, Delegate> loadAction = new Dictionary<SaveLoadType, Delegate>();
        [ShowInInspector] [ReadOnly] private Dictionary<SaveLoadType, IBaseEventPayload> payloadTable = new Dictionary<SaveLoadType, IBaseEventPayload>();

        public Action saveDoneAction;

        private float saveloadTimeOut = 5.0f;                    // 비동기 작업이 해당 변수 이상 지속될경우 정지
        private CancellationTokenSource cancellationTokenSource; // 비동기 작업이 계속될 경우 정지시키게 설정

        private string path;
        private string filename = "EllieSaveFile";

        private bool isSaving = false;
        private bool isLoading = false;

        public bool IsLoadData { get; set; } = false;

        #region ### 초기화 메서드 ###

        public override void Awake()
        {
            base.Awake();

            path = Application.persistentDataPath + "/";
            Debug.Log($"등록 경로 :: {path}");
        }

        public void SubscribeSaveEvent(Action listener)
        {
            saveAction -= listener;
            saveAction += listener;
        }

        public void SubscribeLoadEvent(SaveLoadType eventName, Action<IBaseEventPayload> listener)
        {
            if (!loadAction.ContainsKey(eventName))
                loadAction[eventName] = null;
            loadAction[eventName] = Delegate.Combine(loadAction[eventName], listener);
        }

        public void UnsubscribeSaveEvnt(Action listener)
        {
            saveAction -= listener;
        }

        public void UnsubscribeLoadEvent<T>(SaveLoadType eventName, Action<T> listener) where T : IBaseEventPayload
        {
            if (loadAction.ContainsKey(eventName))
                loadAction[eventName] = Delegate.Remove(loadAction[eventName], listener);
        }

        public void AddPayloadTable(SaveLoadType type, IBaseEventPayload payload)
        {
            payloadTable[type] = payload;
        }

        #endregion

        #region ### 세이브, 로드 함수 ###

        public async void SaveData()
        {
            if (IsCurrentSavingOrLoading())
            {
                return;
            }

            isSaving = true;
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(saveloadTimeOut));

            // 세이브 딕셔너리 저장
            saveAction?.Invoke();

            try
            {
                var saveTasks = new List<Task>();
                for (int i = 0; i < (int)SaveLoadType.End; i++)
                {
                    saveTasks.Add(SaveDataAsync(i, cancellationTokenSource.Token));
                }

                // 타임아웃과 세이브 작업을 병렬로 실행
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cancellationTokenSource.Token);
                var savingTask = Task.WhenAll(saveTasks);
                var completedTask = await Task.WhenAny(savingTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    throw new TimeoutException("세이브 작업이 시간 초과로 취소되었습니다.");
                }
            }
            catch (TimeoutException ex)
            {
                Debug.LogError(ex.Message);
                // 필요한 추가 처리
            }
            catch (Exception ex)
            {
                Debug.LogError($"세이브 중 오류 발생: {ex.Message}");
            }
            finally
            {
                cancellationTokenSource.Cancel();
                isSaving = false;
                cancellationTokenSource = null;

                saveDoneAction?.Invoke();
            }
        }

        public async void LoadData()
        {
            if (IsCurrentSavingOrLoading())
            {
                return;
            }

            isLoading = true;
            payloadTable.Clear();
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(saveloadTimeOut));

            try
            {
                var loadTasks = new List<Task>();
                for (int i = 0; i < (int)SaveLoadType.End; i++)
                {
                    loadTasks.Add(LoadDataAsync(i, cancellationTokenSource.Token));
                }

                await Task.WhenAll(loadTasks);

                // 로드 완료 후 처리
                Debug.Log($"PayloadTable Size: {payloadTable.Values.Count}");
                foreach (var key in payloadTable.Keys)
                {
                    ((Action<IBaseEventPayload>)loadAction[key])(payloadTable[key]);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"로드 중 오류 발생: {ex.Message}");
            }
            finally
            {
                cancellationTokenSource.Cancel();
                isLoading = false;
                cancellationTokenSource = null;
            }
        }

        public async void SaveSpecificData(Action saveAction)
        {
            if (IsCurrentSavingOrLoading())
            {
                return;
            }

            isSaving = true;
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(saveloadTimeOut));

            // 단일 세이브
            saveAction?.Invoke();

            try
            {
                var saveTasks = new List<Task>();
                for (int i = 0; i < (int)SaveLoadType.End; i++)
                {
                    saveTasks.Add(SaveDataAsync(i, cancellationTokenSource.Token));
                }

                // 타임아웃과 세이브 작업을 병렬로 실행
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cancellationTokenSource.Token);
                var savingTask = Task.WhenAll(saveTasks);
                var completedTask = await Task.WhenAny(savingTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    throw new TimeoutException("세이브 작업이 시간 초과로 취소되었습니다.");
                }
            }
            catch (TimeoutException ex)
            {
                Debug.LogError(ex.Message);
                // 필요한 추가 처리
            }
            catch (Exception ex)
            {
                Debug.LogError($"세이브 중 오류 발생: {ex.Message}");
            }
            finally
            {
                cancellationTokenSource.Cancel();
                isSaving = false;
                cancellationTokenSource = null;
            }
        }

        private async Task SaveDataAsync(int index, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                SaveLoadType type = (SaveLoadType)index;

                // 경로 설정
                string filePath = path + filename + index.ToString();

                // 파일이 이미 존재하면 삭제
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // 클래스를 json 변환
                string jsonData = SaveFile(index);

                // 자동 생성 경로에 파일로 저장
                await File.WriteAllTextAsync(path + filename + index.ToString(), jsonData);

                Debug.Log($"{(SaveLoadType)index} - Save Done");
            }
            catch (Exception e)
            {
                Debug.LogError($"{(SaveLoadType)index} - Save Error : {e.Message}");
            }
        }

        private async Task LoadDataAsync(int index, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            SaveLoadType type = (SaveLoadType)index;
            string filePath = path + filename + index.ToString();

            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"{type} - 파일이 존재하지 않습니다 : {filePath}");
                    return; // 파일이 없으면 로드 작업 건너뛰기
                }

                string data = await File.ReadAllTextAsync(filePath);
                IBaseEventPayload payload = LoadFile(data, type);

                if (payload != null)
                {
                    payloadTable[type] = payload;
                    Debug.Log($"{(SaveLoadType)index} - Load Done");
                }
                else
                {
                    Debug.LogWarning($"{type} - 역직렬화 실패 : {filePath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{type} - Load Error : {e.Message}");
                // 여기서 오류 발생해도 다른 파일 로드 작업은 계속 진행
            }
        }

        #endregion

        private string SaveFile(int index)
        {
            SaveLoadType type = (SaveLoadType)index;

            switch (type)
            {
                case SaveLoadType.Inventory:
                {
                    InventorySavePayload payload = payloadTable[type] as InventorySavePayload;
                    return JsonConvert.SerializeObject(payload);
                }
                case SaveLoadType.Player:
                {
                    PlayerSavePayload payload = payloadTable[type] as PlayerSavePayload;
                    return JsonConvert.SerializeObject(payload);
                }
                case SaveLoadType.NPC:
                {
                    NPCSavePayload payload = payloadTable[type] as NPCSavePayload;
                    return JsonConvert.SerializeObject(payload);
                }
                case SaveLoadType.Boss:
                {
                    BossSavePayload payload = payloadTable[type] as BossSavePayload;
                    return JsonConvert.SerializeObject(payload);
                }
                default:
                    return null;
            }
        }

        private IBaseEventPayload LoadFile(string data, SaveLoadType type)
        {
            switch (type)
            {
                case SaveLoadType.Inventory:
                    return JsonConvert.DeserializeObject<InventorySavePayload>(data);
                case SaveLoadType.Player:
                    return JsonConvert.DeserializeObject<PlayerSavePayload>(data);
                case SaveLoadType.NPC:
                    return JsonConvert.DeserializeObject<NPCSavePayload>(data);
                case SaveLoadType.Boss:
                    return JsonConvert.DeserializeObject<BossSavePayload>(data);
                default:
                    return null;
            }
        }

        public bool IsSaveFilesExist()
        {
            bool allFilesExist = true;

            for (int i = 0; i < (int)SaveLoadType.End; i++)
            {
                SaveLoadType type = (SaveLoadType)i;
                string filePath = path + filename + i.ToString();
                if (!File.Exists(filePath))
                {
                    Debug.Log($"{type} - 파일이 존재하지 않습니다 : {filePath}");
                    allFilesExist = false;
                }
            }

            return allFilesExist; // 모든 파일이 존재하면 true, 아니면 false 반환
        }

        public bool IsCurrentSavingOrLoading()
        {
            if (isLoading)
            {
                Debug.Log("로드가 진행중입니다.");
                return true;
            }
            else if (isSaving)
            {
                Debug.Log("세이브가 진행중입니다.");
                return true;
            }

            return false;
        }

        public IEnumerator CheckIsSaveDone()
        {
            var wait = new WaitForSeconds(0.5f);
            while (isSaving)
            {
                yield return wait;
            }

            Debug.Log("데이터 세이브 완료");
        }

        public IEnumerator CheckIsLoadDone()
        {
            var wait = new WaitForSeconds(0.5f);
            while (isLoading)
            {
                yield return wait;
            }

            Debug.Log("데이터 로드 완료");
        }

        public override void ClearAction()
        {
            saveAction = null;
            loadAction = new();

            saveDoneAction = null;
        }
    }
}