using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerBehavior : MonoBehaviour
{
    public enum Item
    {
        Key,
        Todd,
    }
    
    // Inventory slot with the item 
    struct Slot
    {
        Item? _item;
        GameObject _slotObject;
        SpriteRenderer _sprite;

        public Slot(Vector2 pos, Item? item = null)
        {
            _item = null;
            _slotObject = new GameObject($"InventorySlot{pos}", typeof(SpriteRenderer));
            _sprite = _slotObject.GetComponent<SpriteRenderer>();
            Transform transform = _slotObject.GetComponent<Transform>();
            transform.position = new Vector3(pos.x, pos.y, 0f);
            SetItem(item);
        }
        
        // Set the inventory item occupying this slot, forgetting the object that once occupied the space
        public void SetItem(Item? item = null) {
            _item = item;
            if(_item is Item notNull) {
                _sprite.sprite = InventoryManagerBehavior.GetSpriteForItem(notNull);
                _sprite.drawMode = SpriteDrawMode.Sliced;
                _sprite.size = new Vector2(InventoryManagerBehavior.SLOT_SIZE, InventoryManagerBehavior.SLOT_SIZE);
            } else {
                _sprite.sprite = null;
            }

        }
    }

    static Sprite[] _itemSprites;
    Slot[] _items = new Slot[5];
    
    // Size of the inventory item sprites
    static readonly float SLOT_SIZE = 2f;
    
    // Get a sprite object for the given item enumeration using the ITEM_SPRITES array
    public static Sprite GetSpriteForItem(Item item) => _itemSprites[(int)item];

    static void InitSprites()
    {
        if(_itemSprites is not null) {
            return;
        }

        var itemsArray = Enum.GetValues(typeof(Item));
        _itemSprites = new Sprite[itemsArray.Length];
        //Initialize all sprites for the inventory items
        foreach(Item item in itemsArray) {
            Debug.Log($"Loading sprite {Enum.GetName(typeof(Item), item)}...");
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{Enum.GetName(typeof(Item), item)}");
            if(sprite is null) {
                Debug.LogError($"Failed to load {item}");
            }
            _itemSprites[(int)item] = sprite;
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitSprites(); 
        Camera cam = FindObjectOfType<Camera>();
        
        // Get half extents for the camera size
        float camHeight = cam.orthographicSize;
        float camWidth  = camHeight * cam.aspect;

        float spriteHeight = camHeight - SLOT_SIZE;
        float spritePad = SLOT_SIZE / 2f;
        float spriteBegin = ((SLOT_SIZE * _items.Length) + (spritePad * _items.Length - 1)) / 2f;
        Debug.Log($"Total width is {spriteBegin}");
        
        for(int i = 0; i < _items.Length; ++i) {
            _items[i] = new Slot(new Vector2(-spriteBegin + (float)i * (SLOT_SIZE + spritePad) + SLOT_SIZE / 2f, spriteHeight), (i % 2 == 0) ? Item.Todd : null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

