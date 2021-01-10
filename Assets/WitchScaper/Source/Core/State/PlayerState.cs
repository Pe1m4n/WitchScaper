﻿using System.Collections.Generic;
using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core.Character;
using WitchScaper.Core.UI;

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
            var newMagazine = new List<ColorType>(ColorTypeHelper.AllTypes);
            newMagazine.Shuffle();
            state.Ammo.Clear();
            state.Ammo.AddRange(newMagazine);
        }
    }
}