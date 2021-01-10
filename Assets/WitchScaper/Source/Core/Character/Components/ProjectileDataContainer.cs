using System.Collections.Generic;
using System.Linq;
using WitchScaper.Core.State;

namespace WitchScaper.Core.Character
{
    public class ProjectileDataContainer
    {
        private readonly PlayerState _gameState;
        private readonly IEnumerable<ProjectileData> _projectileDatas;

        public ProjectileDataContainer(GameState gameState, IEnumerable<ProjectileData> projectileDatas)
        {
            _gameState = gameState.PlayerState;
            _projectileDatas = projectileDatas;
        }

        public ProjectileData GetDataForColor(ColorType colorType)
        {
            return _projectileDatas.FirstOrDefault(d => d != null && 
                d.ProjectileColor == colorType);
        }
    }
}