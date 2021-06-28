using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class takes an input and two custom deadzones and scales the input value according to those
 */
public class GamepadController : MonoBehaviour
{
    #region Singleton Instance
    public static GamepadController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }
    #endregion

    public Vector2 filterVector(Vector2 vector, float innerDeadZoneRadius, float outerDeadZoneRadius)
    {
        // get the absolute length of the vector without a direction
        float length = vector.magnitude;
        //float filteredLength = Mathf.Clamp(Mathf.InverseLerp(innerDeadZoneRadius, outerDeadZoneRadius, length), 0, 1);
        // remap the value to be on scale from innerDeadZoneRadius to outerDeadZoneRadius
        // all values <= innerDeadZoneRadius become 0, values > outerDeadZoneRadius become 1
        float filteredLength = Mathf.InverseLerp(innerDeadZoneRadius, outerDeadZoneRadius, length);
        // apply the correct direction of the vector again
        // the normalized vector always has a length of 1 but retains its direction
        Vector2 filteredVector = vector.normalized * filteredLength;
        return filteredVector;
    }
}