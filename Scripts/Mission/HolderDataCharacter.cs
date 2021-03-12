using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderDataCharacter : MonoBehaviour
{
    [SerializeField]DataCharacters character1;
    public Characters character;
    public Characters GetCharacter()
    {
        return character1.character;
        return character;
    }
}

