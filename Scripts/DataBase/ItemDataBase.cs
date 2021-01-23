using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Items", menuName = "DataBase/Item")]
public class ItemDataBase : ScriptableObject
{
    public List<IWeapon> Weapons = new List<IWeapon>();
    public List<IArmor> Armors = new List<IArmor>();
    public List<IConsume> Consumes = new List<IConsume>();
    public List<IThrow> Throws = new List<IThrow>();
    public List<Item> Components = new List<Item>();
    public List<IAmmunition> Amunition = new List<IAmmunition>();
    public List<IRecipe> Recipes = new List<IRecipe>();
    public List<Item> KeyItems = new List<Item>();
    public List<IRune> Runes = new List<IRune>();
    public List<IAccessories> Accessories = new List<IAccessories>();
}
