using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{

    public Transform npc; // Reference to the NPC's transform
    public Vector3 offset; // Offset above the NPC
    public float fixedYRotation = 45f; // Desired Y-axis rotation

    private void LateUpdate()
    {
        if (npc == null)
        {
            Debug.LogError("NPC reference not assigned!");
            return;
        }

        // Update position to stay above the NPC
        transform.position = npc.position + offset;

        // Lock the Y-axis rotation and maintain other axes
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, fixedYRotation, currentRotation.z);
    }
}
