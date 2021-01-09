namespace WitchScaper.Core.State
{
    public class GameState
    {
        public enum State
        {
            Battle,
            Dialogs
        }

        public State CurrentState;
        public PlayerState PlayerState = new PlayerState();
    }
}