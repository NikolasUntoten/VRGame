using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCity : MonoBehaviour {

	public GameObject[] blocks;

	public GameObject Spawn;

	public GameObject Ground;

	[Range(1, 100)]
	public int CitySize;

	[Range(1, 20)]
	public int MaxBuildingSize;

	[Range(1, 50)]
	public int MaxBuildingHeight;

	public Vector3 BlockSize;

	[Range(10, 20)]
	public int CityBlockSize;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < Mathf.Sqrt(CitySize); x++) {
			for (int z = 0; z < Mathf.Sqrt(CitySize); z++) {
				if (x == (int) Mathf.Sqrt(CitySize)/2 && z == (int) Mathf.Sqrt(CitySize)/2) {
					float blockScaleX = CityBlockSize;
					float blockScaleZ = CityBlockSize;
					Vector3 pos = new Vector3(
						- blockScaleX/2,
						CityBlockSize/2,
						- blockScaleZ/2);
					GameObject block = Instantiate(Spawn, pos, Quaternion.Euler(-90, 0, 0));
					block.transform.localScale = new Vector3(CityBlockSize/4, CityBlockSize/4, CityBlockSize/2);
					block.transform.SetParent(transform);

					GameObject g = GenerateGround(new Vector3(-blockScaleX / 2, 1, -blockScaleZ / 2));
					g.transform.SetParent(transform);

				} else {
					GameObject block = GenerateCityBlock();
					float blockScaleX = CityBlockSize;
					float blockScaleZ = CityBlockSize;
					block.transform.position = new Vector3(
						x * blockScaleX - blockScaleX / 2 - (int)Mathf.Sqrt(CitySize) / 2 * CityBlockSize,
						1,
						z * blockScaleZ - blockScaleZ / 2 - (int)Mathf.Sqrt(CitySize) / 2 * CityBlockSize);

					GameObject g = GenerateGround(block.transform.position);
					g.transform.SetParent(transform);
				}
				
			}
		}

		transform.localScale = BlockSize;
	}

	GameObject GenerateGround(Vector3 position) {
		GameObject g = Instantiate(Ground, position, Quaternion.identity);
		g.transform.localScale = new Vector3(CityBlockSize, 0.5f, CityBlockSize);
		return g;
	}

	GameObject GenerateCityBlock() {
		GameObject cityBlock = new GameObject("City Block");
		cityBlock.transform.localScale = new Vector3(CityBlockSize, 1, CityBlockSize);
		List<int> xSplits = GetSplits();
		List<int> zSplits = GetSplits();

		Vector2 pos = new Vector2(-CityBlockSize/2 + 1, -CityBlockSize/2 + 1);
		foreach (int x in xSplits) {
			pos.y = -CityBlockSize / 2 + 1;
			pos.x += x + 1;
			foreach (int z in zSplits) {

				GameObject building = GenerateBuilding(x, z);
				building.transform.localPosition = new Vector3(
					pos.x - building.transform.localScale.x / 2 - 1, 
					building.transform.localScale.y / 2, 
					pos.y + building.transform.localScale.z / 2);
				building.transform.SetParent(cityBlock.transform);
				
				pos.y += z + 1;
			}
		}

		cityBlock.transform.SetParent(transform);

		return cityBlock;
	}

	private List<int> GetSplits() {
		List<int> splits = new List<int>();

		int current = 1;
		while (current < CityBlockSize - 1) {
			float r = Random.Range(0f, 1f);
			int size = (int)Mathf.Ceil(r * MaxBuildingSize);
			if (current + size < CityBlockSize - 1) {
				splits.Add(size);
			} else if (size > 0) {
				splits.Add(CityBlockSize - 1 - current);
			}
			current += size + 1;
		}
		

		return splits;
	}

	GameObject GenerateBuilding(int initX, int initY, int initZ) {
		GameObject building = new GameObject("Building");
		GameObject block = blocks[Random.Range(0, blocks.Length)];
		Vector3 size = new Vector3(initX, initY, initZ);

		building.transform.localScale = new Vector3(size.x,
													size.y,
													size.z);
		Vector3 bSize = building.transform.localScale;
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				for (int z = 0; z < size.z; z++) {
					if (x == 0 || x == size.x - 1 || y == 0 || y == size.y - 1 || z == 0 || z == size.z - 1) {
						Vector3 position = new Vector3(x - bSize.x / 2, y - bSize.y / 2, z - bSize.z / 2);
						GameObject temp = Instantiate(block, position, Quaternion.identity);
						temp.transform.localScale = new Vector3(.5f, .5f, .5f);
						temp.SetActive(true);
						temp.transform.SetParent(building.transform);
						temp.transform.position = position;
					}
				}
			}
		}

		return building;
	}

	GameObject GenerateBuilding(int x, int z) {
		float r = Random.Range(0f, 1f);
		int y = (int)Mathf.Ceil(r * MaxBuildingSize);

		return GenerateBuilding(x, y, z);
	}

	GameObject GenerateBuilding() {
		float r = Random.Range(0f, 1f);
		int x = (int)Mathf.Ceil((r * r) * MaxBuildingSize);
		r = Random.Range(0f, 1f);
		int z = (int)Mathf.Ceil((r * r) * MaxBuildingSize);
		
		return GenerateBuilding(x, z);
	}
}
