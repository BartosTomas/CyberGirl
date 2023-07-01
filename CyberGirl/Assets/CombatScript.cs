using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject slamArea;
    private bool attacking = false;
    private bool slamming = false;
    [SerializeField] private float attackTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            StartCoroutine(Attack());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Slam());
        }
    }
    private IEnumerator Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        yield return new WaitForSeconds(attackTime);
        attacking = false;
        attackArea.SetActive(attacking);
    }
    private IEnumerator Slam()
    {
        slamming = true;
        slamArea.SetActive(slamming);
        yield return new WaitForSeconds(1);
    }
}