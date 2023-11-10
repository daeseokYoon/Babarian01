using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        P_States p_Health = character.GetComponent<P_States>();
        if(p_Health != null)
        {
            p_Health.AddHealth((int)val);
        }
    }
}
