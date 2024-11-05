using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObject : MonoBehaviour, IPointerDownHandler
{
    private Rigidbody rb;
    private Collider col;
    private SlotManager slotManager;

    void Start()
    {
        // Rigidbody ve Collider bileþenlerine referans alýyoruz
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // SlotManager'ý buluyoruz (SlotManager objesi sahnede olmalý)
        slotManager = FindObjectOfType<SlotManager>();
    }

    // IPointerDownHandler arayüzünden gelen metot
    public void OnPointerDown(PointerEventData eventData)
    {
        // Týklanan objenin rigidbody ve collider'ýný devre dýþý býrakýyoruz
        if (rb != null)
            rb.isKinematic = true;

        if (col != null)
            col.enabled = false;

        // Objeyi SlotManager'a gönderip boþ slota taþýtýyoruz
        if (slotManager != null)
        {
            slotManager.MoveToNextSlot(gameObject);  // Objeyi slotlara taþýr
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClickSound();
        }
    }
}
