using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    Rigidbody rb;

    private int value;

    private Dictionary<string, bool> upgrades = new Dictionary<string, bool>();

    public bool HasUpgrade(string upgradeKey)
    {
        return upgrades.ContainsKey(upgradeKey);
    }

    public Dictionary<string, bool> SetUpgrades(Dictionary<string, bool> d)
    {
        upgrades = d;
        return upgrades;
    }

    public Dictionary<string, bool> GetUpgrades()
    {
        return upgrades;
    }

    public void SetValue(int v)
    {
        value = v;
    }

    public int GetValue()
    {
        return value;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "conveyor")
        {
            Vector2 accel = other.gameObject.GetComponents<IConveyor>()[0].getAcceleration(other.GetContact(0).point) * Time.deltaTime;

            rb.velocity += new Vector3(accel.x / (Mathf.Abs(rb.velocity.x) + 5), 0, accel.y / (Mathf.Abs(rb.velocity.z) + 5));// * (1 / (rb.velocity.magnitude + 5));
        }
    }
}
