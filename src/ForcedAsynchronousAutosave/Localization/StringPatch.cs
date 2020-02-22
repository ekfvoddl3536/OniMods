namespace AsynchronousAutosave.Translation
{
    internal static class StringPatch
    {
        internal static GenericMessage Message { get; private set; }

        internal static void SetMessage(MODMESSAGE_CONTENT content) =>
            Message = new GenericMessage(
                content.Title,
                content.BodyContent +
                "\n\n     === <b><color=#4785ff>" + content.ModTitle + "</b></color> / by <b>SuperComic (ekfvoddl3535@naver.com)</b> ===",
                "     === <b><color=#4785ff>" + content.ModTitle + "</b></color> / by <b>SuperComic (ekfvoddl3535@naver.com)</b> ===\n" +
                content.TooltipContent);
    }
}
