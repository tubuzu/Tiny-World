using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedMakerProfile : MachineProfile
{
    public Slider progressBar;

    public void CraftItem(BaseItemProfileSO item, int totalCount)
    {
        StartCoroutine(StartCraftItem(item, totalCount));
    }

    IEnumerator StartCraftItem(BaseItemProfileSO item, int totalCount)
    {
        int timeNeeded = item.itemRecipe.timeNeeded;
        Collider2D targetCollider = machineCtrl.MouseTarget.TargetCollider;

        progressBar.gameObject.SetActive(true);

        for (int i = 0; i < totalCount; i++)
        {
            Vector3 spawnPosition = targetCollider.bounds.min;
            spawnPosition.x += Random.Range(0, targetCollider.bounds.size.x);
            spawnPosition.y += Random.Range(-0.2f, -0.6f);
            yield return SpawnCraftItem(item, timeNeeded, spawnPosition);
        }

        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
    }

    IEnumerator SpawnCraftItem(BaseItemProfileSO item, int timeNeeded, Vector3 pos)
    {
        for (int i = 0; i < timeNeeded; i++)
        {
            progressBar.value = (float)i / timeNeeded;
            yield return new WaitForSeconds(1f);
        }
        progressBar.value = 1;
        yield return new WaitForSeconds(0.05f);
        ItemDropSpawner.Instance.DropItem(item, 1, pos, Quaternion.identity);
    }
}
