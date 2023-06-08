using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BroadcastSystem
{
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (var keyValuePair in _subsystems)
        {
            keyValuePair.Value.DrawGizmos();
        }
    }
#endif
}
