using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public float Acceleration = 1;
    public float Deceleration = 1;
    public float MaxSpeed = 5;

    private Vector2 m_CurrentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentSpeed = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float upAccel = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            upAccel = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            upAccel = -1.0f;
        }

        float rightAccel = 0.0f;
        if (Input.GetKey(KeyCode.D))
        {
            rightAccel = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rightAccel = -1.0f;
        }

        m_CurrentSpeed += new Vector2(rightAccel * Acceleration, upAccel * Acceleration) * Time.deltaTime;

        if (rightAccel == 0.0f)
        {
            if (m_CurrentSpeed.x > 0.0f)
            {
                m_CurrentSpeed.x -= Deceleration * Time.deltaTime;
                if (m_CurrentSpeed.x < 0)
                {
                    m_CurrentSpeed.x = 0;
                }
            }
            else if (m_CurrentSpeed.x < 0.0f)
            {
                m_CurrentSpeed.x += Deceleration * Time.deltaTime;
                if (m_CurrentSpeed.x > 0)
                {
                    m_CurrentSpeed.x = 0;
                }
            }
        }
        if (upAccel == 0.0f)
        {
            if (m_CurrentSpeed.y > 0.0f)
            {
                m_CurrentSpeed.y -= Deceleration * Time.deltaTime;
                if (m_CurrentSpeed.y < 0)
                {
                    m_CurrentSpeed.y = 0;
                }
            }
            else if (m_CurrentSpeed.y < 0.0f)
            {
                m_CurrentSpeed.y += Deceleration * Time.deltaTime;
                if (m_CurrentSpeed.y > 0)
                {
                    m_CurrentSpeed.y = 0;
                }
            }
        }

        if (m_CurrentSpeed.x > MaxSpeed)
        {
            m_CurrentSpeed.x = MaxSpeed;
        }
        else if (m_CurrentSpeed.x < -MaxSpeed)
        {
            m_CurrentSpeed.x = -MaxSpeed;
        }

        if (m_CurrentSpeed.y > MaxSpeed)
        {
            m_CurrentSpeed.y = MaxSpeed;
        }
        else if (m_CurrentSpeed.y < -MaxSpeed)
        {
            m_CurrentSpeed.y = -MaxSpeed;
        }

        Vector3 movement = new Vector3(m_CurrentSpeed.x, 0.0f, m_CurrentSpeed.y) * Time.deltaTime;
        transform.position = transform.position + Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * movement;
    }
}
