namespace Channels.Dialog
{
    public enum DialogType
    {
        Notify,
        NotifyToClient,
    }

    public enum DialogAction
    {
        Play,
        Stop,
        Pause,
        Resume,
        OnNext,
    }

    public enum DialogCanvasType
    { 
        Default,
        Simple,
        SimpleRemaining,
        GuideDialog,
    }


    public class DialogPayload : IBaseEventPayload
    {
        public DialogType dialogType;
        public DialogAction dialogAction;
        public DialogCanvasType canvasType = DialogCanvasType.Default;
        public string text;
        public string speaker;
        public string imageName;
        public float interval;
        public bool isPlaying;
        public bool isEnd;
        public float dialogDuration;

        public static DialogPayload Play(string text, float interval = 0.01f)
        {
            DialogPayload payload = new DialogPayload();
            payload.dialogType = DialogType.Notify;
            payload.dialogAction = DialogAction.Play;
            payload.text = text;
            payload.interval = 0.01f;

            return payload;
        }

        public static DialogPayload Stop()
        {
            DialogPayload payload = new DialogPayload();
            payload.dialogType = DialogType.Notify;
            payload.dialogAction = DialogAction.Stop;

            return payload;
        }

        public static DialogPayload Pause()
        {
            DialogPayload payload = new DialogPayload();
            payload.dialogType = DialogType.Notify;
            payload.dialogAction = DialogAction.Pause;

            return payload;
        }

        public static DialogPayload Resume()
        {
            DialogPayload payload = new DialogPayload();
            payload.dialogType = DialogType.Notify;
            payload.dialogAction = DialogAction.Resume;

            return payload;
        }

        public static DialogPayload OnNext()
        {
            DialogPayload payload = new DialogPayload();
            payload.dialogType = DialogType.Notify;
            payload.dialogAction = DialogAction.OnNext;

            return payload;
        }
    }

    public class DialogChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not DialogPayload dialogPayload)
                return;

            if (dialogPayload.dialogType == DialogType.Notify || 
                dialogPayload.dialogType == DialogType.NotifyToClient)
                Publish(payload);
        }
    }
}