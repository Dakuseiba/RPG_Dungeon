using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderDataCharacter : MonoBehaviour
{
    //[SerializeField]DataCharacters character;
    public Characters character;
    public Characters GetCharacter()
    {
        return character;
    }
}

