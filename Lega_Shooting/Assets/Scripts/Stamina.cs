using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    //Tiempo que tarda para regenerar un 1% de stamina
    private WaitForSeconds regenTick = new WaitForSeconds(.04f);

    //Asignar una co-rutina para poder pararla cuando se hace el dash
    private Coroutine regen;

    //Slider con el que movemos el slider del dash
    public Slider stamina;

    public float staminaActual;
    public float staminaMaxima = 100;

    //Para poder acceder al script desde otro
    public static Stamina instance;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        staminaActual = staminaMaxima;
        stamina.maxValue = staminaMaxima;
        stamina.value = staminaMaxima;
    }

    public void UseStamina(float resta)
    {
        if (staminaActual - resta >= 0)
        {
            staminaActual -= resta;
            stamina.value = staminaActual;
            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("No hay suficiente stamina");
        }
    }

    private IEnumerator RegenStamina()
    {
        //Tiempo que tarda hasta que vuelve a recuperar stamina
        yield return new WaitForSeconds(1.5f);

        while (staminaActual < staminaMaxima)
        {
            staminaActual += staminaMaxima / 100;
            stamina.value = staminaActual;
            yield return regenTick;
        }
        regen = null;
    }
}
