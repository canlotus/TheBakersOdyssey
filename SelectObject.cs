using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObject : MonoBehaviour, IPointerDownHandler
{
    private Rigidbody rb;
    private Collider col;
    private SlotManager slotManager;

    void Start()
    {
        // Rigidbody ve Collider bile�enlerine referans al�yoruz
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // SlotManager'� buluyoruz (SlotManager objesi sahnede olmal�)
        slotManager = FindObjectOfType<SlotManager>();
    }

    // IPointerDownHandler aray�z�nden gelen metot
    public void OnPointerDown(PointerEventData eventData)
    {
        // T�klanan objenin rigidbody ve collider'�n� devre d��� b�rak�yoruz
        if (rb != null)
            rb.isKinematic = true;

        if (col != null)
            col.enabled = false;

        // Objeyi SlotManager'a g�nderip bo� slota ta��t�yoruz
        if (slotManager != null)
        {
            slotManager.MoveToNextSlot(gameObject);  // Objeyi slotlara ta��r
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClickSound();
        }
    }
}
