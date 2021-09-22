using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    [SerializeField] private GameObject m_MainCamera;
    public GameObject MainCamera
    {
        get { return m_MainCamera; }
        set { m_MainCamera = value; }
    }
}
