using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerData playerData;
    private PlayerMoveForce playerMove;

    [SerializeField] WeaponsManager weaponManager;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerMove = GetComponent<PlayerMoveForce>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("ENTRANDO EN COLISION CON ->" + other.gameObject.name);
        if (other.gameObject.CompareTag("Powerups"))
        {
            Destroy(other.gameObject);
            //sumar vida
            playerData.Healing(other.gameObject.GetComponent<Pumpkin>().HealPoints);

            //SUMAS SCORE
            GameManager.Score++;
            Debug.Log(GameManager.Score);
        }

        if (other.gameObject.CompareTag("Munitions"))
        {
            Debug.Log("ENTRANDO EN COLISION CON " + other.gameObject.name);
            playerData.Damage(other.gameObject.GetComponent<Munition>().DamagePoints);
            Destroy(other.gameObject);
            if (playerData.HP <= 0)
            {
                Debug.Log("GAME OVER");
            }

            //RESTAS SCORE
            GameManager.Score--;
            Debug.Log(GameManager.Score);
        }

        if (other.gameObject.CompareTag("Floor"))
        {
            playerMove.CanJump = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //Debug.Log("SALGO DE LA COLISION ->" + other.gameObject.name);
    }

    private void OnCollisionStay(Collision other)
    {
        //Debug.Log("EN CONTACO CON ->" + other.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Trampoline"))
        {
            /*
             Cambio de velocidad instantáneo (ForceMode.VelocityChange)
             Aqui la fuerza se traduce en un cambio de velocidad,
             por lo cual el movimiento se altera significantemente.
             El cálculo Vector3.up + Vector3.forward permite al
             aplicar fuerza en "diagonal"(hacia arriba y adelante)
            */
            playerMove.MyRigidbody.AddForce((Vector3.up + Vector3.forward) * playerMove.MaxSpeed * 5f, ForceMode.VelocityChange);
        }

        if (other.gameObject.CompareTag("Weapons"))
        {
            // AGREGAR EL ARMA A LA LISTA DE ARMAS
            other.gameObject.SetActive(false);
            weaponManager.WeaponList.Add(other.gameObject);
            //COLA
            weaponManager.WeaponQueue.Enqueue(other.gameObject);
            Debug.Log("ELEMENTOS EN LA COLA " + weaponManager.WeaponQueue.Count);
            //STACK
            weaponManager.WeaponStack.Push(other.gameObject);
            Debug.Log("ELEMENTOS EN LA STACK " + weaponManager.WeaponStack.Count);
            //DIC
            if (!weaponManager.WeaponDirectory.ContainsKey(other.gameObject.name))
            {
                weaponManager.WeaponDirectory.Add(other.gameObject.name, other.gameObject);
                Debug.Log(weaponManager.WeaponDirectory[other.gameObject.name]);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {

    }
}
