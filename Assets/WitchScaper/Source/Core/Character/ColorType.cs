using System.Collections.Generic;

namespace WitchScaper.Core.Character
{
    public enum ColorType
    {
        Green,
        Red,
        Blue
    }

    public static class ColorTypeHelper
    {
        public static List<ColorType> AllTypes = new List<ColorType>() {ColorType.Green, ColorType.Blue, ColorType.Red};
    }
}