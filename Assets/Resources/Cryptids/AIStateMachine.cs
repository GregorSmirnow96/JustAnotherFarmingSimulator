using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIStateMachine
{
    private Dictionary<string, List<(Func<bool>, string)>> stateTransitions;

    public string state { get; private set; }
    
    public string previousState { get; private set; }

    public AIStateMachine()
    {
        state = null;
        stateTransitions = new Dictionary<string, List<(Func<bool>, string)>>();
    }

    public void SetInitialState(string inititalState)
    {
        if (state != null)
        {
            Debug.Log($"The initial state has already been set to {state}");
            return;
        }

        previousState = state;
        state = inititalState;
    }

    public List<string> GetTransitionableStates(string startState)
    {
        return stateTransitions
            .SelectMany(kvp => kvp.Value
                .Select(tuple => tuple.Item2))
            .Distinct()
            .ToList();
    }

    public void AddState(string state)
    {
        if (stateTransitions.Keys.Contains(state))
        {
            Debug.Log($"State '{state}' already exists");
            return;
        }

        stateTransitions.Add(state, new List<(Func<bool>, string)>());
    }

    public void AddTransition(
        string startState,
        Func<bool> transitionCheck,
        string endState)
    {
        if (!stateTransitions.Keys.Contains(startState))
        {
            Debug.Log($"Start state '{startState}' does not exist");
        }
        if (!stateTransitions.Keys.Contains(endState))
        {
            Debug.Log($"End state '{endState}' does not exist");
        }

        stateTransitions[startState].Add((transitionCheck, endState));
    }

    public void UpdateState()
    {
        if (state == null)
        {
            Debug.Log($"The initial state has not been set");
        }

        previousState = state;

        List<string> validNewStates = stateTransitions[state]
            .Where(transition => transition.Item1())
            .Select(transition => transition.Item2)
            .ToList();

        if (validNewStates.Count >= 2)
        {
            string validStatesString = validNewStates
                .Aggregate((s1, s2) => $"{s1}, {s2}");
            Debug.Log($"These states were all valid transitions: {validStatesString}");

            state = validNewStates[0];
            Debug.Log("The first state was used");
        }
        else if (validNewStates.Count == 1)
        {
            state = validNewStates[0];
        }
    }
}
