using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Player _target;
    [SerializeField] private float _delayBetweenWaves;

    private Wave _currentWave;
    private int _currentWaveIndex = -1;
    private WaitForSeconds _spawnDelay;
    private WaitForSeconds _waveDelay;
    private Coroutine _spawnCoroutine;
    private Coroutine _waveCoroutine;
    private int _spawnedCount;

    private void Awake()
    {
        _waveDelay = new WaitForSeconds(_delayBetweenWaves);
    }

    private void Start()
    {
        if (_waves.Count == 0)
            return;

        if (_waveCoroutine == null)
            _waveCoroutine = StartCoroutine(StartWaveCoroutine(_waves.First(), new WaitForSeconds(0)));
    }

    private void Update()
    {
        if (_waves.Count == 0)
            return;

        if (_waveCoroutine == null && NextWaveIsNeed() && TryGetNextWave(out Wave nextWave))
            _waveCoroutine = StartCoroutine(StartWaveCoroutine(nextWave, _waveDelay));

        if (_spawnCoroutine == null && WaveCanSpawn())
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator StartWaveCoroutine(Wave wave, WaitForSeconds delay)
    {
        yield return delay;
        StartWave(wave);

        _waveCoroutine = null;
    }

    private void StartWave(Wave wave)
    {
        _currentWave = wave;
        _currentWaveIndex++;
        _spawnDelay = new WaitForSeconds(_currentWave.Delay);
        _spawnedCount = 0;
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return _spawnDelay;
        Enemy enemy = Instantiate(_currentWave.Template, _spawnPoint);
        enemy.Initialize(_target);
        _spawnedCount++;
        _spawnCoroutine = null;
    }

    private bool WaveCanSpawn()
    {
        if (_currentWave == null)
            return false;
        else
            return _spawnedCount < _currentWave.Count;
    }

    private bool TryGetNextWave(out Wave wave)
    {
        wave = null;
        if (_currentWaveIndex < _waves.Count - 1)
            wave = _waves[_currentWaveIndex + 1];

        return wave != null;
    }

    private bool NextWaveIsNeed()
    {
        return WaveCanSpawn() == false;
    }
}

[System.Serializable]
public class Wave
{
    public Enemy Template;
    public float Delay;
    public int Count;
}
