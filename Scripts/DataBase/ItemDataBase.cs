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
    public List<IComponent> Components = new List<IComponent>();
    public List<IRecipe> Recipes = new List<IRecipe>();
    public List<IKey> KeyItems = new List<IKey>();
    public List<IRune> Runes = new List<IRune>();
    public List<IAccessories> Accessories = new List<IAccessories>();
}
