using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _xFollowDistance = 5f;
    [SerializeField] private float _zFollowDistance = 5f;
    [SerializeField] private float _followMoveThreshhold = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xDistance = _targetTransform.position.x - _targetTransform.position.x;
        float zDistance = _targetTransform.position.z - _targetTransform.position.z;

        Vector3 newPosition = _targetTransform.position;

        float xMoveThreshhold = Mathf.Abs(xDistance - _xFollowDistance);
        float zMoveThreshhold = Mathf.Abs(zDistance - _zFollowDistance);

        if (xMoveThreshhold > _followMoveThreshhold)
        {
            if (xDistance > _xFollowDistance)
            {
                newPosition.x -= transform.right.x;
            }
            else if (xDistance < _xFollowDistance)
            {
                newPosition.x += transform.right.x;
            }
        }

        if (zMoveThreshhold > _followMoveThreshhold)
        {
            if (zDistance > _zFollowDistance)
            {
                newPosition.z -= transform.forward.z;
            }
            else if (zDistance < _zFollowDistance)
            {
                newPosition.z += transform.forward.z;
            }
        }

        transform.position = Vector3.Lerp(_targetTransform.position, newPosition, _followSpeed * Time.deltaTime);    
    }
}
