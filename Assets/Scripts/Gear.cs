using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        //basic set
        name= "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //property set
        type = data.itemType;
        rate = data.damages[0];

        ApplyGear();
    }

    public void LevelUP(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch(type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUP(); break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate); 
                    break;

                default:
                    speed = 150 * Character.WeaponRate;
                    weapon.speed = 0.5f *(1f -rate);
                    break;
            }
        }

    }

    void SpeedUP()
    {
        float speed = 3*Character.Speed;
        GameManager.instance.player.speed = speed + speed*rate;
    }
}
