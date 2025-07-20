using _Project.Scripts.GameRoot.States.GameStates;
using UnityEngine;

namespace _Project.Scripts.GameRoot.LevelContexts
{
    public class BuildingLevelContext : LevelContext
    {
        public BuildingSystem buildingSystem;
        public Grid grid;
        public CircuitManager circuitManager;
        
        public override void InitLevel()
        {
            Bootstrap.Instance.sm_Game.ChangeState(new BuildingGState());
            buildingSystem.enabled = true;
            base.InitLevel();
        }

        public override void DisposeLevel()
        {
            buildingSystem.enabled = false;
            base.DisposeLevel();
        }
    }
}