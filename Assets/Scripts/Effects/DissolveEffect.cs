using System;
using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private string _shaderNameToCheck = "Shader Graphs/DissolveShader";
    private Material _material;

    [Header("Dissolve Settings")]
    [SerializeField] private float _dissolveDuration = 1;

    private float _dissolveAmount = 0;
    private bool _isDissolving = false;

    public event Action onDissolveComplete;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;

        if (_material.shader.name != _shaderNameToCheck)
        {
            Debug.LogError("Material not using Dissolve shader.");
        }
    }

    public void StartDissolve()
    {
        if(!_isDissolving)
            StartCoroutine(DissolveCoroutine());
    }

    private IEnumerator DissolveCoroutine()
    {
        _isDissolving = true;

        while (_dissolveAmount < 1f)
        { 
            _dissolveAmount += Time.deltaTime / _dissolveDuration;
            _material.SetFloat("_DissolveAmount", _dissolveAmount);

            yield return null;
        }

        onDissolveComplete?.Invoke();
    }
}
