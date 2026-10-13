
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSimulation : MonoBehaviour
{
    public GameObject[] buildings;
    public ParticleSystem fireEffect;
    public ParticleSystem smokeEffect;

    public float spreadInterval = 3f;
    public float spreadDistance = 16f;

    private readonly HashSet<int> burning = new HashSet<int>();
    private readonly Dictionary<int, ParticleSystem> activeFires = new Dictionary<int, ParticleSystem>();
    private readonly Dictionary<int, ParticleSystem> activeSmoke = new Dictionary<int, ParticleSystem>();

    private Coroutine spreadRoutine;

    void Start()
    {
        if (fireEffect != null)
            fireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (smokeEffect != null)
            smokeEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartFire(0);

        if (Input.GetKeyDown(KeyCode.H))
            StopFire();

        if (Input.GetKeyDown(KeyCode.J))
            ResetFire();
    }

    public void StartFire(int buildingIndex = 0)
    {
        if (buildings == null || buildings.Length == 0)
        {
            Debug.LogError("Assign buildings in FireSimulation.");
            return;
        }

        buildingIndex = Mathf.Clamp(buildingIndex, 0, buildings.Length - 1);

        if (buildings[buildingIndex] == null)
            return;

        if (burning.Add(buildingIndex))
        {
            Debug.Log("Fire started at " + buildings[buildingIndex].name);
            CreateEffects(buildingIndex);
        }

        if (spreadRoutine == null && burning.Count < buildings.Length)
            spreadRoutine = StartCoroutine(SpreadFire());
    }

    void CreateEffects(int index)
    {
        Vector3 position = buildings[index].transform.position;

        if (fireEffect != null && !activeFires.ContainsKey(index))
        {
            ParticleSystem fire = Instantiate(
                fireEffect,
                position + Vector3.up * 2f,
                Quaternion.identity
            );

            fire.gameObject.SetActive(true);
            fire.Play(true);
            activeFires[index] = fire;
        }

        if (smokeEffect != null && !activeSmoke.ContainsKey(index))
        {
            ParticleSystem smoke = Instantiate(
                smokeEffect,
                position + Vector3.up * 3f,
                Quaternion.identity
            );

            smoke.gameObject.SetActive(true);
            smoke.Play(true);
            activeSmoke[index] = smoke;
        }
    }

    IEnumerator SpreadFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(spreadInterval);

            List<int> newFires = new List<int>();

            foreach (int sourceIndex in burning)
            {
                for (int i = 0; i < buildings.Length; i++)
                {
                    if (buildings[i] == null || burning.Contains(i))
                        continue;

                    if (Vector3.Distance(
                        buildings[sourceIndex].transform.position,
                        buildings[i].transform.position
                    ) <= spreadDistance)
                    {
                        newFires.Add(i);
                    }
                }
            }

            foreach (int index in newFires)
            {
                if (burning.Add(index))
                {
                    Debug.Log("Fire spread to " + buildings[index].name);
                    CreateEffects(index);
                }
            }

            if (burning.Count >= buildings.Length || newFires.Count == 0)
            {
                spreadRoutine = null;
                yield break;
            }
        }
    }

    public void StopFire()
    {
        if (spreadRoutine != null)
        {
            StopCoroutine(spreadRoutine);
            spreadRoutine = null;
        }

        foreach (ParticleSystem fire in activeFires.Values)
            if (fire != null) fire.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        foreach (ParticleSystem smoke in activeSmoke.Values)
            if (smoke != null) smoke.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        Debug.Log("Fire stopped.");
    }

    public void ResetFire()
    {
        StopFire();

        foreach (ParticleSystem fire in activeFires.Values)
            if (fire != null) Destroy(fire.gameObject);

        foreach (ParticleSystem smoke in activeSmoke.Values)
            if (smoke != null) Destroy(smoke.gameObject);

        activeFires.Clear();
        activeSmoke.Clear();
        burning.Clear();

        Debug.Log("Fire reset.");
    }
}
