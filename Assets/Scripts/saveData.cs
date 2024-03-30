using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class saveData {
    public static int numberOfBulletData = 50;
    public Dictionary<string, bulletData> bulletSaveData =  new Dictionary<string, bulletData>();
}

[System.Serializable]
public class bulletData {
    public List<Vector2> trajectoryData;
    public Vector2 startPositionData;
    public float costData;
    public int attributeData;
    public int kindData;
}