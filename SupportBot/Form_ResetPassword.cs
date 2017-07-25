using System;
using Microsoft.Bot.Builder.FormFlow;

namespace SupportBot
{
    public enum LengthOptions { SixInch, FootLong };
    public enum BreadOptions { NineGrainWheat, NineGrainHoneyOat, Italian, ItalianHerbsAndCheese, Flatbread };

    [Serializable]
    public class Form_ResetPassword
    {
        public LengthOptions? Length;
        public BreadOptions? Bread;
        public static IForm<Form_ResetPassword> BuildForm()
        {
            return new FormBuilder<Form_ResetPassword>()
                    .Message("No worries, I can help with that!")
                    .Build();
        }
    }
}