using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject wallChunk;
    public GameObject terrainChunk;

    public Vector3 center;

    public static Dictionary<ChunkPos, TerrainChunk> chunks = new Dictionary<ChunkPos, TerrainChunk>();

    FastNoise noise = new FastNoise();

    int chunkDist = 6;

    List<TerrainChunk> pooledChunks = new List<TerrainChunk>();

    List<ChunkPos> toGenerate = new List<ChunkPos>();

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3();
        LoadChunks();
    }


    private void Update()
    {
       
    }



    void BuildChunk(int xPos, int zPos)
    {
        TerrainChunk chunk;
        if (pooledChunks.Count > 0)//look in the poo first
        {
            chunk = pooledChunks[0];
            chunk.gameObject.SetActive(true);
            pooledChunks.RemoveAt(0);
            chunk.transform.position = new Vector3(xPos, 0, zPos);
        }
        else
        {
            GameObject chunkGO = Instantiate(terrainChunk, new Vector3(xPos, 0, zPos), Quaternion.identity);
            chunk = chunkGO.GetComponent<TerrainChunk>();
        }


        for (int x = 0; x < TerrainChunk.chunkWidth + 2; x++)
            for (int z = 0; z < TerrainChunk.chunkWidth + 2; z++)
                for (int y = 0; y < TerrainChunk.chunkHeight; y++)
                {
                    //if(Mathf.PerlinNoise((xPos + x-1) * .1f, (zPos + z-1) * .1f) * 10 + y < TerrainChunk.chunkHeight * .5f)
                    chunk.blocks[x, y, z] = GetBlockType(xPos + x - 1, y, zPos + z - 1);
                }


        GenerateStructures(chunk.blocks, xPos, zPos);

    
        chunk.BuildMesh();


        WaterChunk wat = chunk.transform.GetComponentInChildren<WaterChunk>();
        wat.SetLocs(chunk.blocks);
        wat.BuildMesh();



        chunks.Add(new ChunkPos(xPos, zPos), chunk);
    }


    //get the block type at a specific coordinate
    BlockType GetBlockType(int x, int y, int z)
    {
    
        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex(x * .8f, z * .8f) * 10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 5 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = TerrainChunk.chunkHeight * .5f + heightMap;

        

        //stone layer heightmap
        float simplexStone1 = noise.GetSimplex(x * 1f, z * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex(x * 5f, z * 5f) + .5f) * 20 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = TerrainChunk.chunkHeight * .25f + stoneHeightMap;


        //float cliffThing = noise.GetSimplex(x * 1f, z * 1f, y) * 10;
        //float cliffThingMask = noise.GetSimplex(x * .4f, z * .4f) + .3f;



        BlockType blockType = BlockType.Air;

        //under the surface, dirt block
        if (y <= baseLandHeight)
        {
            blockType = BlockType.Dirt;

            //just on the surface, use a grass type
            if (y > baseLandHeight - 1 && y > WaterChunk.waterHeight - 2)
                blockType = BlockType.Grass;

            if (y <= baseStoneHeight)
                blockType = BlockType.Stone;
        }

        return blockType;
    }


    //ChunkPos curChunk = new ChunkPos(-1, -1);
    void LoadChunks()
    {
        //the current chunk the player is in
        int curChunkPosX = Mathf.FloorToInt(center.x / TerrainChunk.chunkWidth ) * TerrainChunk.chunkWidth;
        int curChunkPosZ = Mathf.FloorToInt(center.z / TerrainChunk.chunkWidth) * TerrainChunk.chunkWidth;

        
        //Generate Walls:

        int[] rangeChunkPosX = new int[2];
        int[] rangeChunkPosZ = new int[2];
        rangeChunkPosX[0] = curChunkPosX - TerrainChunk.chunkWidth * chunkDist;
        rangeChunkPosX[1] = curChunkPosX + TerrainChunk.chunkWidth * chunkDist + TerrainChunk.chunkWidth;
        rangeChunkPosZ[0] = curChunkPosZ - TerrainChunk.chunkWidth * chunkDist;
        rangeChunkPosZ[1] = curChunkPosZ + TerrainChunk.chunkWidth * chunkDist + TerrainChunk.chunkWidth;

        GameObject wallG = Instantiate(wallChunk, new Vector3(0, 0, 0), Quaternion.identity);
        WallChunk wall = wallG.GetComponent<WallChunk>();
        wall.setRange(rangeChunkPosX, rangeChunkPosZ);
        wall.BuildMesh();
        
        
        

        for (int i = curChunkPosX - TerrainChunk.chunkWidth * chunkDist; i <= curChunkPosX + TerrainChunk.chunkWidth * chunkDist; i += TerrainChunk.chunkWidth)
            for (int j = curChunkPosZ - TerrainChunk.chunkWidth * chunkDist; j <= curChunkPosZ + TerrainChunk.chunkWidth * chunkDist; j += TerrainChunk.chunkWidth)
            {
                ChunkPos cp = new ChunkPos(i, j);

                if (!chunks.ContainsKey(cp) && !toGenerate.Contains(cp))
                {
                    if (Mathf.Abs(curChunkPosX - cp.x) <= TerrainChunk.chunkWidth * 4 &&
                        Mathf.Abs(curChunkPosZ - cp.z) <= TerrainChunk.chunkWidth * 4)
                    { BuildChunk(i, j); }
                    else { toGenerate.Add(cp); }
                }


            }


        StartCoroutine(DelayBuildChunks());
       

    }




    void GenerateStructures(BlockType[,,] blocks, int x, int z)
    {
        System.Random rand = new System.Random(x * 20000 + z);
        int structureCount = Mathf.FloorToInt((float)rand.NextDouble() * 2);

        int structureHeight = 2;

        int xPos = (int)(rand.NextDouble() * 13) + 2;
        int zPos = (int)(rand.NextDouble() * 13) + 2;

        // if (xPos >= 1 || xPos < 15 || zPos >= 1 || zPos < 15) { return; }
        int y = TerrainChunk.chunkHeight - 1;
        //find the ground
        while (y > 0 && (blocks[xPos, y, zPos] == BlockType.Air || blocks[xPos, y, zPos] == BlockType.Trunk || blocks[xPos, y, zPos] == BlockType.Leaves))
        {
            y--;
        }
        y++;

        for (int i = 0; i < structureCount; i++)
        {
            for (int j = 0; j < structureHeight; j++)
            {
                for (int ix = -1; ix <= 1; ix++)
                {
                    for (int iz = -1; iz <= 1; iz++)
                    {
                        if (y + j < 64 && blocks[xPos + ix, y + j, zPos + iz] != BlockType.Trunk)
                            blocks[xPos + ix, y + j, zPos + iz] = BlockType.Brick;
                    }
                }

            }

        }
    }


    IEnumerator DelayBuildChunks()
    {
        while (toGenerate.Count > 0)
        {
            BuildChunk(toGenerate[0].x, toGenerate[0].z);
            toGenerate.RemoveAt(0);

            yield return new WaitForSeconds(.2f);

        }

    }


}


public struct ChunkPos
{
    public int x, z;
    public ChunkPos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}