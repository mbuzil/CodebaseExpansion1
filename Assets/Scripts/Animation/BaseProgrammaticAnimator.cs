using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProgrammaticAnimator : MonoBehaviour {
    protected Animator Animator {
        get { return m_Animator ??= GetComponentInChildren<Animator>(); }
    }

    private Animator m_Animator;

    private readonly HashSet<int> m_ParameterExistenceSet = new HashSet<int>();

    private void Awake() {
        if (this.Animator == null) return;
        for (int i = 0; i < this.Animator.parameterCount; i++) {
            m_ParameterExistenceSet.Add(this.Animator.parameters[i].nameHash);
        }
    }

    protected bool ParameterExists(int nameHash) {
        return m_ParameterExistenceSet.Contains(nameHash);
    }
}