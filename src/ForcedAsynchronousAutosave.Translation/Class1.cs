namespace AsynchronousAutosave.Translation
{
    public static class MODMESSAGE
    {
        public static MODMESSAGE_CONTENT Message { get; private set; }

        public static void SetDefaultMessage() =>
            SetMessage(
                "자동저장 완료",
                "저장이 완료되었습니다.", "비동기 자동저장 모드",
                "저장이 완료되었습니다.\n이제, 게임을 종료하거나 메인메뉴로 나가거나 다른 세이브파일을 로드하실 수 있습니다.");

        public static void SetMessage(string title, string body_content, string mod_title, string tooltip_content) =>
            Message = new MODMESSAGE_CONTENT(title, body_content, mod_title, tooltip_content);
    }

    public struct MODMESSAGE_CONTENT
    {
        public string Title;
        public string BodyContent;
        public string ModTitle;
        public string TooltipContent;

        public MODMESSAGE_CONTENT(string title, string body_content, string mod_title, string tooltip_content)
        {
            Title = title;
            BodyContent = body_content;
            ModTitle = mod_title;
            TooltipContent = tooltip_content;
        }
    }
}
