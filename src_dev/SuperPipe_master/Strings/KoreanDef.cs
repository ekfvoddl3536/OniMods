using SuperComicLib.ModONI;

namespace SuperPipe.LocalStrings
{
    internal sealed class KoreanDef : ModLocalization<SuperPipeTextsKey>
    {
        public override Localization.Language MainLanguage => Localization.Language.Korean;

        protected override void OnInitialize() =>
            texts_ = new string[]
            {

            };

        public static void InitOrSkip()
        {
            if (Default == null)
                new KoreanDef();
        }
    }
}
