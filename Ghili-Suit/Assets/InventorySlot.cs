using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Inventory.Item? item {
        get => _item;
        set {
            _item = value;

            if(_sprite is null) {
                _sprite = GetComponent<SpriteRenderer>();
            }
            if(_item is Inventory.Item notNull) {
                _sprite.sprite = Inventory.GetSpriteForItem(notNull);
                _sprite.drawMode = SpriteDrawMode.Sliced;
                _sprite.size = new Vector2(Inventory.SLOT_SIZE, Inventory.SLOT_SIZE);
            } else {
                _sprite.sprite = null;
            }
        }
    }
    
    Inventory.Item? _item;
    SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        var circle = GetComponent<CircleCollider2D>();
        circle.isTrigger = true;
        circle.radius = 1f;
    }

    void OnMouseDown()
    {
        var inv = FindObjectOfType<Inventory>();
        if(_item is not null) {
            if(inv.GrabItem(_item)) {
                item = null;
            }
        } else {
            if(inv.HeldItem is not null) {
                item = inv.HeldItem;
                inv.GrabItem(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
