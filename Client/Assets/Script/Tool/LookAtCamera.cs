using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
    Transform m_Transform;
    Transform m_Camera;
	// Use this for initialization
	void Start () {
        this.m_Transform = this.transform;
        this.m_Camera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        LookAt();
    }
    public void LookAt()
    {
        this.m_Transform.rotation = this.m_Camera.rotation;
    }
}
