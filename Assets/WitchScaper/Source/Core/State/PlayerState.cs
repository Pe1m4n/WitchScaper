using System.Collections.Generic;
using UnityEngine;
using WitchScaper.Core.Character;

namespace WitchScaper.Core.State
{
    public class PlayerState
    {
        public readonly List<ColorType> Ammo = new List<ColorType>() {ColorType.Green, ColorType.Blue, ColorType.Red};
        public float TimeToReload;
    }

    public static class PlayerStateExtensions
    {
        public static void UseAmmo(this PlayerState state, int index)
        {
            state.Ammo.RemoveAt(index);
            state.Ammo.Add(ColorTypeHelper.AllTypes[Random.Range(0, ColorTypeHelper.AllTypes.Count)]);
        }
    }
}