using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private float tocDoXe = 100f; // Max speed
    [SerializeField]
    private float lucReXe = 100f; // Turning force
    [SerializeField]
    private float lucPhanh = 50f; // Braking force
    [SerializeField]
    private float accelerationRate = 10f; // Rate of acceleration
    [SerializeField]
    private GameObject hieuUngPhanh; // Brake effect

    private float dauVaoDiChuyen;
    private float dauVaoRe;
    private float currentSpeed = 0f; // Current speed to gradually increase
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        dauVaoDiChuyen = Input.GetAxis("Vertical");
        dauVaoRe = Input.GetAxis("Horizontal");

        ReXe(); // Handle turning
        DiChuyenXe(); // Handle forward and backward movement

        // Apply braking if Shift key is held
        if (dauVaoDiChuyen > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            PhanhXe();
        }
    }

    public void DiChuyenXe()
    {
        // If there is input to move forward or backward, gradually increase or decrease speed
        if (dauVaoDiChuyen != 0)
        {
            // Accelerate toward the target speed (tocDoXe * dauVaoDiChuyen) gradually
            currentSpeed = Mathf.MoveTowards(currentSpeed, tocDoXe * dauVaoDiChuyen, accelerationRate * Time.deltaTime);
        }
        else
        {
            // Gradually decelerate to zero when no input is provided
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, accelerationRate * Time.deltaTime);
        }

        // Apply force based on currentSpeed
        rb.AddRelativeForce(Vector3.forward * currentSpeed);
        hieuUngPhanh.SetActive(false);
    }

    public void ReXe()
    {
        Quaternion re = Quaternion.Euler(Vector3.up * dauVaoRe * lucReXe * Time.deltaTime);
        rb.MoveRotation(rb.rotation * re);
    }

    public void PhanhXe()
    {
        if (rb.velocity.z != 0)
        {
            rb.AddRelativeForce(-Vector3.forward * lucPhanh);
            hieuUngPhanh.SetActive(true);
            currentSpeed = Mathf.Max(0, currentSpeed - lucPhanh * Time.deltaTime); // Reduce current speed gradually
        }
    }
}
