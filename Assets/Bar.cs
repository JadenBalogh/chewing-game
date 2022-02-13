using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("col");
        GameManager.Instance.IsNoteActive = true;
        if (col.TryGetComponent<Note>(out Note note))
        {
            GameManager.Instance.CurrentNote = note.gameObject;
            note.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("col");
        GameManager.Instance.IsNoteActive = false;
        if (col.TryGetComponent<Note>(out Note note) && col.gameObject.activeSelf)
        {
            GameManager.Instance.ApplyMiss();
            Destroy(note.gameObject);
        }
    }
}
