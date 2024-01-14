using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // All collectible items in the game
    public enum Item
    {
        Key,
        Todd,
    }

    public Item? HeldItem {
        get => _heldItem;
    }

    Item? _heldItem;
    GameObject _cursorTracked;

    Camera _cam;
    
    // Inventory slots that will be dynamically created with the `Start` method
    GameObject[] _items = new GameObject[5];
    
    // Size of the inventory item sprites
    public static readonly float SLOT_SIZE = 2f;
    
    // Map of enum values to their loaded sprite resources
    static Sprite[] _itemSprites;
    
    // Get a sprite object for the given item enumeration using the ITEM_SPRITES array
    public static Sprite GetSpriteForItem(Item item) => _itemSprites[(int)item];
    
    // Load all sprites from their corresponding resources
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
    
    // Create `GameObjects` for each inventory slot and position them onscreen
    void CreateInventorySlots()
    {
        // Get half extents for the camera size
        float camHeight = _cam.orthographicSize;
        float camWidth  = camHeight * _cam.aspect;

        float spriteHeight = camHeight - SLOT_SIZE;
        float spritePad = SLOT_SIZE / 2f;
        float spriteBegin = ((SLOT_SIZE * _items.Length) + (spritePad * _items.Length - 1)) / 2f;
        
        for(int i = 0; i < _items.Length; ++i) {
            _items[i] = new GameObject("InventorySlot", typeof(SpriteRenderer), typeof(InventorySlot), typeof(CircleCollider2D));
            _items[i]
                .GetComponent<Transform>()
                .position = new Vector3(
                        -spriteBegin + (float)i * (SLOT_SIZE + spritePad) + SLOT_SIZE / 2f,
                        spriteHeight,
                        0f
                );

            _items[i].GetComponent<InventorySlot>().item = (i % 2 == 0) ? Item.Todd : null;
        }
    }

    // If `item` is not null, attempt to grab the given item, returning `true` if there was no item currently held and
    // `false` if the item was not picked up
    public bool GrabItem(Item? item)
    {
        if(item is Item notNull) {
            if(_heldItem is not null) { return false; }

            var sprite = _cursorTracked.GetComponent<SpriteRenderer>();
            sprite.sprite = Inventory.GetSpriteForItem(notNull);
            sprite.drawMode = SpriteDrawMode.Sliced;
            sprite.size = new Vector2(Inventory.SLOT_SIZE, Inventory.SLOT_SIZE);
            _heldItem = item;
            return true;
        } else {
            _heldItem = null;
            _cursorTracked.GetComponent<SpriteRenderer>().sprite = null;
            return true;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = FindObjectOfType<Camera>();
        InitSprites(); 
        CreateInventorySlots();

        _heldItem = null;
        _cursorTracked = new GameObject("CursorHeldItem", typeof(SpriteRenderer));
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        var mouseWorldPos = _cam.ScreenToWorldPoint(
            new Vector3(
                mousePos.x,
                mousePos.y,
                _cam.nearClipPlane + 1
            )
        );

        _cursorTracked.GetComponent<Transform>().position = mouseWorldPos;
    }
}

