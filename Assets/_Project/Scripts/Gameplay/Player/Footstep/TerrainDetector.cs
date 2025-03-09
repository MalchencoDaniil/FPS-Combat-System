using UnityEngine;

public class TerrainDetector
{
    private TerrainData _terrainData;
    private int _alphamapWidth;
    private int _alphamapHeight;
    private float[,,] _splatmapData;
    private int _numTextures;

    public TerrainDetector()
    {
        _terrainData = Terrain.activeTerrain.terrainData;
        _alphamapWidth = _terrainData.alphamapWidth;
        _alphamapHeight = _terrainData.alphamapHeight;

        _splatmapData = _terrainData.GetAlphamaps(0, 0, _alphamapWidth, _alphamapHeight);
        _numTextures = _splatmapData.Length / (_alphamapWidth * _alphamapHeight);
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 _worldPosition)
    {
        Vector3 _splatPosition = new Vector3();
        Terrain _ter = Terrain.activeTerrain;
        Vector3 _terPosition = _ter.transform.position;
        _splatPosition.x = ((_worldPosition.x - _terPosition.x) / _ter.terrainData.size.x) * _ter.terrainData.alphamapWidth;
        _splatPosition.z = ((_worldPosition.z - _terPosition.z) / _ter.terrainData.size.z) * _ter.terrainData.alphamapHeight;
        return _splatPosition;
    }

    public int GetActiveTerrainTextureIdx(Vector3 _position)
    {
        Vector3 _terrainCord = ConvertToSplatMapCoordinate(_position);
        int _activeTerrainIndex = 0;
        float _largestOpacity = 0f;

        for (int i = 0; i < _numTextures; i++)
        {
            if (_largestOpacity < _splatmapData[(int)_terrainCord.z, (int)_terrainCord.x, i])
            {
                _activeTerrainIndex = i;
                _largestOpacity = _splatmapData[(int)_terrainCord.z, (int)_terrainCord.x, i];
            }
        }

        return _activeTerrainIndex;
    }

}