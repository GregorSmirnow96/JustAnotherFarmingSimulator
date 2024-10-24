using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatBehaviourExtension : IAttacker
{
    public void InitializeStates(AIStateMachine stateMachine);
}
