using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public int damage;
    public string damageType;
    public Transform damagedTransform;
    public Vector3 transformOffset;

    private Vector2 offset;
    private Vector2 gravity;
    private Vector2 velocity;
    private float duration;
    private float timer;
    private RectTransform rectTransform;

    void Start()
    {
        offset = new Vector2(0f, 0f);
        gravity = new Vector2(0f, -700f);
        velocity = new Vector2(Random.Range(-200, 200), Random.Range(100, 300));
        timer = 0f;
        duration = 1f;

        rectTransform = GetComponent<RectTransform>();

        TMP_Text text = GetComponent<TMP_Text>();
        text.text = damage.ToString();

        Material clonedMaterial = new Material(text.fontMaterial);
        text.fontMaterial = clonedMaterial;
        Debug.Log(damageType);
        switch (damageType)
        {
            case DamageType.Physical:
                clonedMaterial.SetColor("_FaceColor", Color.white);
                break;
            case DamageType.Water:
                clonedMaterial.SetColor("_FaceColor", Color.blue);
                Debug.Log("Water");
                break;
            case DamageType.Fire:
                clonedMaterial.SetColor("_FaceColor", Color.red);
                Debug.Log("Fire");
                break;
            case DamageType.Lightning:
                clonedMaterial.SetColor("_FaceColor", Color.yellow);
                Debug.Log("Lightning");
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (damagedTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        velocity = velocity + gravity * Time.deltaTime;
        offset = offset + velocity * Time.deltaTime;

        Vector3 parentWorldPosition = damagedTransform.position + transformOffset;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(parentWorldPosition);
        rectTransform.position = screenPosition + offset;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
