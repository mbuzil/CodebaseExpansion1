using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementEventCaster
{
    public event Action<float> OnHorizontalInputRegistered;
    public event Action OnCasted;
    public event Action<bool> OnIsGroundedChanged;
}
