using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialEvents : MonoBehaviour
{
    public GabrielController gabrielController;
    public Animator gabrielAnimator;
    public GameObject exclamationIndicator; // indicador de exclama��o gabriel
    public AudioSource audioSource; // audiosource do Gabriel
    public AudioClip dangerClip;
    public AudioClip comicClip;
    public AudioClip roarClip;
    public GameObject gabrielUIGroup;
    public InventoryUI gabrielInventoryUI;
    public GabrielInventoryManager gabrielInventory;
    public GameObject gabrielSkillUI;      
    public GameObject gabrielHealthImage;  
    public Image dialogueBox;
    public GameObject cabriolaPrefab;
    public Transform cabriolaSpawnPoint;
    public CameraFollow cameraFollow;
    public Transform gabrielTransform;
    public Transform cabriolaTransform; // s� preenchido dps do spawn


    // 1. Anima��o de queda/dano
    public void PlayFallAnimation()
    {
        StartCoroutine(FallAndDieSequence());
    }

    private IEnumerator FallAndDieSequence()
    {
        // 1. Move Gabriel para fora do ecrã (posição inicial para a queda)
        Vector3 end = gabrielTransform.position;
        Vector3 start = gabrielTransform.position + new Vector3(0, 5f, 0);
        gabrielTransform.position = start;

        // 2. Esperar até Gabriel ficar visível na câmara
        Renderer gabrielRenderer = gabrielTransform.GetComponent<Renderer>();
        while (gabrielRenderer != null && !gabrielRenderer.isVisible)
        {
            yield return null;
        }

        // 3. Agora ativa a animação "Damage" e deixa correr normalmente durante a queda
        if (gabrielAnimator != null)
        {
            gabrielAnimator.speed = 1f;
            gabrielAnimator.SetTrigger("Damage");
        }

        // 4. Fazer a queda do topo para a posição final
        float duration = 1.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            gabrielTransform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        gabrielTransform.position = end;

        // 5. Pausar no último frame da animação Damage (para não voltar a Idle)
        if (gabrielAnimator != null)
        {
            gabrielAnimator.Play("Damage", 0, 1f); // vai para o último frame de "Damage"
            gabrielAnimator.speed = 0f;
        }

        // 6. Esperar um tempo antes de tocar Die
        yield return new WaitForSeconds(1.0f);

        // 7. Tocar Die e pausar no último frame
        if (gabrielAnimator != null)
        {
            gabrielAnimator.speed = 1f;
            gabrielAnimator.SetTrigger("Die");
            yield return null;
            yield return new WaitForSeconds(0.1f);
            gabrielAnimator.Play("Die", 0, 1f); // vai para o último frame de "Die"
            gabrielAnimator.speed = 0f;
        }

    }



    // 2. Anima��o de levantar (morte invertida)
    public void PlayGetUpAnimation()
    {
        if (gabrielAnimator != null)
        {
            gabrielAnimator.speed = 1f;
            gabrielAnimator.SetTrigger("GetUp");
        }
    }

    // 3. Flip r�pido do sprite idle + som c�mico
    public void FlipConfusedLook()
    {
        if (gabrielController != null) StartCoroutine(FlipRoutine());
        if (audioSource != null && comicClip != null) audioSource.PlayOneShot(comicClip);
    }
    private IEnumerator FlipRoutine()
    {
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.35f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.35f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
    }

    // 4. Exclama��o + som de perigo
    public void ShowExclamation() => ShowExclamationCustom(1f);
    public void ShowExclamationCustom(float duration)
    {
        StartCoroutine(ShowExclamationRoutine(duration));
    }
    private IEnumerator ShowExclamationRoutine(float duration)
    {
        if (exclamationIndicator == null) yield break;
        exclamationIndicator.SetActive(true);
        if (audioSource != null && dangerClip != null) audioSource.PlayOneShot(dangerClip);
        yield return new WaitForSeconds(duration);
        exclamationIndicator.SetActive(false);
    }

    // 5. Mostrar invent�rio
    public void ShowGabrielInventory()
    {
        // Garante que o grupo está ativo
        if (gabrielUIGroup != null)
            gabrielUIGroup.SetActive(true);

        // Ativa só o inventário
        if (gabrielInventoryUI != null)
            gabrielInventoryUI.gameObject.SetActive(true);

        // Esconde os outros elementos do grupo
        if (gabrielSkillUI != null)
            gabrielSkillUI.SetActive(false);

        if (gabrielHealthImage != null)
            gabrielHealthImage.SetActive(false);

        // Adiciona a pedra "Pedra" ao inventário, se ainda não existir
        Debug.Log("ShowGabrielInventory chamado");
        if (gabrielInventory != null && !InventoryContainsPedra())
        {
            Debug.Log("Vai tentar adicionar pedra!");
            if (gabrielInventory.pedra != null)
            {
                ItemPickup pedraPickup = gabrielInventory.pedra.GetComponent<ItemPickup>();
                if (pedraPickup != null && pedraPickup.itemData != null)
                {
                    Item pedraItem = pedraPickup.itemData.GetItem();
                    Debug.Log("Item Pedra criado: " + pedraItem.itemName);
                    gabrielInventory.TryPickupItem(pedraItem);
                }
               
            }
            
        }
        // **FORÇA a atualização do UI com o estado atual dos slots**
        if (gabrielInventory != null && gabrielInventory.inventoryUI != null)
            gabrielInventory.inventoryUI.UpdateUI(gabrielInventory.slots, gabrielInventory.selectedSlot);


        SetDialogueBoxAlpha(0.5f);
    }

    public void HideGabrielInventory()
    {
        if (gabrielInventoryUI != null)
            gabrielInventoryUI.gameObject.SetActive(false);

        if (gabrielSkillUI != null)
            gabrielSkillUI.SetActive(true);

        if (gabrielHealthImage != null)
            gabrielHealthImage.SetActive(true);

        SetDialogueBoxAlpha(1f);
    }

    private bool InventoryContainsPedra()
    {
        foreach (var item in gabrielInventory.slots)
        {
            if (item != null && item.itemName == "Pedra")
                return true;
        }
        return false;
    }


    // 7. Alterar transpar�ncia da DialogueBox (Image)
    public void SetDialogueBoxAlpha(float alpha)
    {
        if (dialogueBox != null)
        {
            var color = dialogueBox.color;
            color.a = Mathf.Clamp01(alpha);
            dialogueBox.color = color;
        }
    }


    // 7. olhar para direita/esquerda (sprite flip)
    public void LookAround()
    {
        StartCoroutine(LookAroundRoutine());
    }
    private IEnumerator LookAroundRoutine()
    {
        if (gabrielController == null) yield break;
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.5f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.5f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
    }

    // 8. tocar som de rugido
    public void PlayRoarSound()
    {
        if (audioSource != null && roarClip != null)
            audioSource.PlayOneShot(roarClip);
    }

    // 9. Spawnar Cabriola
    public void SpawnCabriola()
    {
        if (cabriolaPrefab != null && cabriolaSpawnPoint != null)
        {
            GameObject cabriola = Instantiate(cabriolaPrefab, cabriolaSpawnPoint.position, Quaternion.identity);
            cabriolaTransform = cabriola.transform;
            // Se o DialogueManager também tem cabriolaTransform, atualiza lá também:
            if (FindFirstObjectByType<DialogueManager>() != null)
                FindFirstObjectByType<DialogueManager>().cabriolaTransform = cabriola.transform;
        }
    }



    // 10. Mudar c�mara para Cabriola
    public void FocusCameraOnCabriola()
    {
        if (cameraFollow != null && cabriolaTransform != null)
            cameraFollow.SetTarget(cabriolaTransform);
    }


    // 11. Voltar c�mara para Gabriel
    public void FocusCameraOnGabriel()
    {
        if (cameraFollow != null && gabrielTransform != null)
            cameraFollow.SetTarget(gabrielTransform);
    }

    // 12. Tocar som de perigo (caso precises de um evento separado)
    public void PlayDangerSound()
    {
        if (audioSource != null && dangerClip != null) audioSource.PlayOneShot(dangerClip);
    }

    public void Termina()
    {
        SceneManager.LoadScene("Level 1 - Tutorial");
    }
}

