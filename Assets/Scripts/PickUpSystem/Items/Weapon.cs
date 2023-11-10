using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }; // melee 근접해서 싸우다
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 후 콜라이더, 트레일 켜짐
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        
        yield return new WaitForSeconds(0.3f); // 위에 실행 후 0.3초 후  콜라이더 꺼짐
        meleeArea.enabled = false;
        
        yield return new WaitForSeconds(0.3f); // 또 0.3초 후 트레일 꺼짐
        trailEffect.enabled = false;
    }
}
