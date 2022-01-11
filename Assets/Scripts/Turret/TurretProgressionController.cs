using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretProgressionController : MonoBehaviour
{
    public Transform dragonTransform;

    private BNG.PlayerTeleport playerTeleport;
    public Text missionText;
    public Text instructionsText;

    // Start is called before the first frame update
    void Start()
    {
        instructionsText.enabled = false;
        playerTeleport = ProgressionController.Instance.OnRequestTeleporter();

        StartCoroutine(MovePlayerToDragon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MovePlayerToDragon()
    {
        yield return new WaitForSeconds(10);

        missionText.enabled = false;
        instructionsText.enabled = true;

        StartCoroutine(playerTeleport.doTeleport(dragonTransform.localPosition, dragonTransform.localRotation, true));
    }
}
