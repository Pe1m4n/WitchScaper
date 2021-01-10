using System.Collections.Generic;
using WitchScaper.Core.Character;

namespace WitchScaper.Core.State
{
    public class GameState
    {
        public enum State
        {
            Battle,
            Dialogs,
            QTE
        }

        public State CurrentState;
        public PlayerState PlayerState = new PlayerState();
        public List<EnemyController> EnemiesForQTE = new List<EnemyController>();
    }
}