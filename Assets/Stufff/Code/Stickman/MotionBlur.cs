using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : MonoBehaviour
{
    public float ghostSpawnInterval = 0.1f;
    public float ghostLifeTime = 0.5f;
    public GameObject ghostPrefab;
    public SpriteRenderer spriteRenderer;
    public PauseManager PauseManager;
    private float ghostTimer = 0f;
    private bool isDestroyed = false; 
    private List<GameObject> activeGhosts = new List<GameObject>(); 

    public void HandleMotionBlur()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (isDestroyed)
            return;

        ghostTimer -= Time.deltaTime;

        if (ghostTimer <= 0)
        {
            ghostTimer = ghostSpawnInterval;
            SpawnGhost();
        }
    }

    private void SpawnGhost()
    {
        if(PauseManager.isPaused == false)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            activeGhosts.Add(ghost);
            SpriteRenderer ghostRenderer = ghost.GetComponent<SpriteRenderer>();
            SpriteRenderer playerRenderer = spriteRenderer;
            ghostRenderer.sprite = playerRenderer.sprite;
            ghostRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            ghost.transform.localScale = transform.localScale;
            Vector2 playerSpriteSize = playerRenderer.sprite.bounds.size;
            Vector2 ghostSpriteSize = ghostRenderer.sprite.bounds.size;
            float widthScale = playerSpriteSize.x / ghostSpriteSize.x;
            float heightScale = playerSpriteSize.y / ghostSpriteSize.y;
            float scaleReduceAmount = 0.8f;
            Vector3 sizeAdjustment = new Vector3(widthScale, heightScale, 1f) * scaleReduceAmount;
            ghost.transform.localScale = Vector3.Scale(ghost.transform.localScale, sizeAdjustment);
            StartCoroutine(FadeAndDestroyGhost(ghost, ghostRenderer));
        }
        
        
    }

    private IEnumerator FadeAndDestroyGhost(GameObject ghost, SpriteRenderer ghostRenderer)
    {
        float elapsedTime = 0f;

        while (elapsedTime < ghostLifeTime && !isDestroyed)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, elapsedTime / ghostLifeTime);
            ghostRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        activeGhosts.Remove(ghost);
        Destroy(ghost);
    }

    private void OnDestroy()
    {
        isDestroyed = true;
        foreach (GameObject ghost in activeGhosts)
        {
            if (ghost != null)
            {
                Destroy(ghost);
            }
        }
        activeGhosts.Clear();
    }
}
