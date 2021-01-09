using System.Collections.Generic;
using UnityEngine;
using WitchScaper.Core.Character;

namespace WitchScaper.Core.State
{
    public class PlayerState
    {
        public List<ColorType> HexAmmo = new List<ColorType>() {ColorType.Green, ColorType.Blue, ColorType.Red};
        public List<ColorType> DamageAmmo = new List<ColorType>() {ColorType.Green, ColorType.Blue, ColorType.Red};

        public float TimeToReloadHex;
        public float TimeToReloadAmmo;
    }

    public static class PlayerStateExtensions
    {
        public static void UseAmmo(this PlayerState state, bool isDamage, int index)
        {
            var container = isDamage ? state.DamageAmmo : state.HexAmmo;
            container.RemoveAt(index);
            container.Add(ColorTypeHelper.AllTypes[Random.Range(0, ColorTypeHelper.AllTypes.Count)]);
        }
    }
}