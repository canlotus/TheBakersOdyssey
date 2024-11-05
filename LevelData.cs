[System.Serializable]
public class LevelData
{
    public int level;  // Seviye numaras�
    public SpawnPointData[] spawnPoints;  // Spawnlanacak objeler ve say�lar�
    public WinCondition winCondition;  // Kazanma ko�ullar�
}

[System.Serializable]
public class SpawnPointData
{
    public string prefabName;  // Prefab'�n ismi (JSON'dan gelir)
    public int quantity;  // Spawnlanacak obje say�s�
}

[System.Serializable]
public class WinCondition
{
    public RequiredObject[] requiredObjects;  // Hangi objeler kazanmak i�in gerekli
}

[System.Serializable]
public class RequiredObject
{
    public string objectName;  // Kazanmak i�in gerekli objenin ismi
    public int count;  // Bu objeden ka� tane toplanmas� gerekti�i
}
