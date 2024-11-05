[System.Serializable]
public class LevelData
{
    public int level;  // Seviye numarasý
    public SpawnPointData[] spawnPoints;  // Spawnlanacak objeler ve sayýlarý
    public WinCondition winCondition;  // Kazanma koþullarý
}

[System.Serializable]
public class SpawnPointData
{
    public string prefabName;  // Prefab'ýn ismi (JSON'dan gelir)
    public int quantity;  // Spawnlanacak obje sayýsý
}

[System.Serializable]
public class WinCondition
{
    public RequiredObject[] requiredObjects;  // Hangi objeler kazanmak için gerekli
}

[System.Serializable]
public class RequiredObject
{
    public string objectName;  // Kazanmak için gerekli objenin ismi
    public int count;  // Bu objeden kaç tane toplanmasý gerektiði
}
