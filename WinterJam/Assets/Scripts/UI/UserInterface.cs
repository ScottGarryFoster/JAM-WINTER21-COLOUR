using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class UserInterface : MonoBehaviour
{
    [Header("Health")]
    public List<RawImage> Hearts;
    public int HeartContainers;
    public int Health;
    public Texture FullHeart;
    public Texture JustContainer;

    public int targetHealth;
    public int targetHeartContainers;

    [Header("Focus")]
    public int BarPercentage;
    public Bar BarScript;
    public TextMeshProUGUI BarText;

    private float waitHealth;
    private float waitHealthContainers;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUIHealth();
        UpdateUIFocus();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        UpdateUIHealth();
        UpdateUIFocus();
        //TickHealth();
        //TickHealthContainers();
    }

    void UpdateUIHealth()
    {
        for (int h = 0; h < Hearts.Count; ++h)
        {
            if (h < HeartContainers)
            {
                Hearts[h].enabled = true;
                if (Health > h)
                {
                    Hearts[h].canvasRenderer.SetTexture(FullHeart);
                }
                else
                {
                    Hearts[h].canvasRenderer.SetTexture(JustContainer);
                }
            }
            else
            {
                Hearts[h].enabled = false;
            }
        }
    }

    void UpdateUIFocus()
    {

        if (BarScript != null)
        {
            BarScript.Percentage = BarPercentage;
            BarText?.SetText($"{BarPercentage}%");
        }
    }

    public void AdjustFocus(int focus)
    {
        BarPercentage = focus;
    }

    public void UpdateHealth(int health)
    {
        Health = health;
    }

    public void UpdateHeartContainers(int health)
    {
        HeartContainers = health;
    }

    private void TickHealth()
    {
        if (targetHealth != Health)
        {
            waitHealth -= Time.deltaTime;
            if (waitHealth < 0)
            {
                Health = targetHealth > Health ? targetHealth + 1 : targetHealth - 1;
                waitHealth += 0.25f;
            }
        }
    }

    private void TickHealthContainers()
    {
        if (targetHeartContainers != HeartContainers)
        {
            waitHealthContainers -= Time.deltaTime;
            if (waitHealthContainers < 0)
            {
                HeartContainers = targetHeartContainers > HeartContainers ? targetHeartContainers + 1 : targetHeartContainers - 1;
                waitHealthContainers += 0.25f;
            }
        }
    }
}
