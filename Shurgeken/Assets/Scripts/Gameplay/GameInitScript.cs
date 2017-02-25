﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInitScript : MonoBehaviour
{

    public static GameInitScript gis;

    [SerializeField]
    Text connectionText;

    [SerializeField]
    Transform[] spawnPoints;

    [SerializeField]
    Transform[] jailSpawnPoints;

    [SerializeField]
    Transform[] aiSpawnPoints;

    [SerializeField]
    Transform[] aiWayPoints;

    [SerializeField]
    Transform[] flagSpawns;

    [SerializeField]
    Camera sceneCamera;

    [SerializeField]
    AudioListener sceneListener;

    GameObject player;

    public GameObject redFlag;

    private NetworkManager networkManager;

    public GameObject playerTracker;

    // Use this for initialization
    void Start () {
        gis = this;
        networkManager = NetworkManager.networkManager;
	}

    

    void Update()
    {
        if (connectionText)
            connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();

        if (redFlag == null)
        {
            redFlag = GameObject.FindGameObjectWithTag("RedFlag");
        }
        if (playerTracker == null)
        {
            playerTracker = GameObject.FindGameObjectWithTag("Tracker");
        }
    }

    void OnJoinedRoom()
    {
        StartSpawnProcess(0);
    }

    public IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        player = networkManager.spawnObject("Player", spawnPoints[index], null);
        
        player.GetComponent<PlayerNetworkController>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
        sceneListener.enabled = false;
        // --target--.GetComponent<PhotonView>().RPC("asdf", PhotonTargets.All,params);
    }

    public IEnumerator SpawnPlayerInJail(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, jailSpawnPoints.Length);
        player = networkManager.spawnObject("Player", jailSpawnPoints[index], null);
        player.GetComponent<DataController>().inJail = true;
        player.GetComponent<PlayerNetworkController>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
        sceneListener.enabled = false;
    }

    public IEnumerator SpawnAI()
    {

        GameObject guard;
        int index = Random.Range(0, aiSpawnPoints.Length);
        guard = networkManager.spawnSceneObject("Guard", aiSpawnPoints[index], null);
        if (guard)
            guard.GetComponent<EnemyStatePattern>().patrolPath = new TestPath(aiWayPoints);
        yield return null;
    }

    public IEnumerator SpawnFlag()
    {
        int index = Random.Range(0, flagSpawns.Length);
        redFlag = networkManager.spawnSceneObject("Red Flag 1", flagSpawns[index], null);
        yield return null;
    }

    public IEnumerator SpawnPlayerTracker()
    {
        playerTracker = networkManager.spawnSceneObject("PlayerTracker", null);
        yield return null;
    }

    public void StartSpawnProcess(float respawnTime)
    {
        sceneCamera.enabled = true;
        StartCoroutine("SpawnPlayer", respawnTime);
        if (NetworkManager.networkManager.isMaster()) {
            StartCoroutine("SpawnAI");
            StartCoroutine("SpawnFlag");
            StartCoroutine("SpawnPlayerTracker");
        }

    }
}
