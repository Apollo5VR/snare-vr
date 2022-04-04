using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.Services.Analytics;
using System.Linq;

public class WireController : MonoBehaviour
{
    public static WireController Instance { get; private set; }
    //refernce to wire render array
    public IndieMarc.CurvedLine.CurvedLine3D curvedLineA;
    public IndieMarc.CurvedLine.CurvedLine3D curvedLineB;
    public GameObject configJointPrefab;
    //rerence to wire joint array
    public GameObject[] configJoints;
    //state or state bool - modified by triggers
    //reference to the half / fully completed models (and colliders)
    public GameObject inCompleteWire;
    public GameObject halfCompleteWire;
    public GameObject completeWire;
    public GameObject wireAroundTree;

    //private List<GameObject> thist = new List<GameObject>();

    //private int[] part1Bendables = new int[] {0,1,2};
    //private int[] part2Bendables = new int[] { 1, 2, 3 }; //just anything greater than part 1 last index

    public Action<int> OnWireSectionComplete;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        halfCompleteWire.SetActive(false);
        curvedLineB.gameObject.SetActive(false);
        completeWire.SetActive(false);
        wireAroundTree.SetActive(false);

        OnWireSectionComplete += WireManipulations;
    }

    public void WireManipulations(int id)
    {
        switch (id)
        {
            case 0:
                //half loop activate
                curvedLineA.gameObject.SetActive(false);
                inCompleteWire.SetActive(false);
                curvedLineB.gameObject.SetActive(true);
                halfCompleteWire.SetActive(true);
                //deactivate bendable section 1, enable section 2
                //TODO - refactor to not use LINQ - bad practice
                //curvedLine.paths = curvedLine.paths.Take(part1Bendables.Length).ToArray();
                //List<Transform> listCon = curvedLine.paths.ToList();
                //listCon.RemoveRange(0, part1Bendables.Length);
                //curvedLine.paths = listCon.ToArray();
                //might not need to do this if we just set the 4th one Locked from start
                /*
                ConfigurableJoint joint = configJoints[part1Bendables.Length].GetComponent<ConfigurableJoint>();
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;

                ConfigurableJoint joint2 = configJoints[part1Bendables.Length + 1].GetComponent<ConfigurableJoint>();
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
                joint.angularZMotion = ConfigurableJointMotion.Free;
                */

                /*
                for(int i = 0; i < part1Bendables.Length; i++)
                {
                    curvedLine.paths.Slice
                }
                */

                break;
            case 1:
                //complete loop activate
                curvedLineB.gameObject.SetActive(false);
                halfCompleteWire.SetActive(false);
                completeWire.SetActive(true);
                //deactivate bendable completely
                break;
            case 2:
                //tree loop activate
                completeWire.SetActive(false);
                wireAroundTree.SetActive(true);
                //deactivate bendable completely
                break;
            default:
                break;
        }
    }

    
}
