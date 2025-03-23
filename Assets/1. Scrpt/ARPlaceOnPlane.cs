using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;

    GameObject spawnObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCenterObject();

        //PlaceObjectByTouch();
    }


    private void PlaceObjectByTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;

                }

            }
        }
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose placementPos = hits[0].pose;

            // 오브젝트를 활성화하고 위치를 설정
            placeObject.SetActive(true);
            placeObject.transform.position = placementPos.position;

            // 카메라의 Y축 회전만 고려하여 오브젝트가 카메라 반대 방향을 보도록 설정
            Vector3 cameraForward = Camera.current.transform.forward;
            cameraForward.y = 0; // 수평 방향만 고려

            if (cameraForward != Vector3.zero)
            {
                // 카메라의 반대 방향을 계산 (뒤쪽을 보도록)
                Vector3 cameraBackward = -cameraForward;
                placeObject.transform.rotation = Quaternion.LookRotation(cameraBackward);
            }
        }
    }
}

    



