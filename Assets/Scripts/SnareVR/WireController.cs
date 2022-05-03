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
    public GameObject noBuilding;

    public GameObject rabbit; //TODO - refactor to be created as object item (to fullfil multiple trap/s scenarios)

    //private List<GameObject> thist = new List<GameObject>();

    //private int[] part1Bendables = new int[] {0,1,2};
    //private int[] part2Bendables = new int[] { 1, 2, 3 }; //just anything greater than part 1 last index

    public Action<int> OnWireSectionComplete;

    public float trapTimeRemaining;
    public CommonEnums.SnareState snareState = CommonEnums.SnareState.None;


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
    private void Start()
    {
        //scene clear
        noBuilding.SetActive(false);
        halfCompleteWire.SetActive(false);
        curvedLineA.gameObject.SetActive(false);
        curvedLineB.gameObject.SetActive(false);
        completeWire.SetActive(false);
        wireAroundTree.SetActive(false);
        rabbit.SetActive(false);

        SetupTrapScene();

        OnWireSectionComplete += WireManipulations;
        ScriptsConnector.Instance.OnRabbitCaught += RabbitCaught;
        ScriptsConnector.Instance.OnTrapTriggerTimeSet += SetupTrapScene;
    }

    private void OnDestroy()
    {
        OnWireSectionComplete -= WireManipulations;

        if (ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnRabbitCaught -= RabbitCaught;
            ScriptsConnector.Instance.OnTrapTriggerTimeSet -= SetupTrapScene;
        }
    }

    //call on start to set state and save a local value to the WireController
    public async void SetupTrapScene()
    {
        try
        {
            //TODO - update this singleton to use the scriptsconnector
            float time = await CloudCodeManager.instance.CallGetTrapTimeRemainingEndpoint();

            //TODO - better idea in the future to not user a hardcoded value to determine if a trap exists (should just pull if trap exists for this scenario)
            if (time == -1)
            {
                //Users first time OR last session they checked trap, but didnt set up another one (we delete on snare check)
                snareState = CommonEnums.SnareState.Build;

                inCompleteWire.SetActive(true);
                curvedLineA.gameObject.SetActive(true);
                halfCompleteWire.SetActive(false);
                curvedLineB.gameObject.SetActive(false);
                completeWire.SetActive(false);
                wireAroundTree.SetActive(false);
                rabbit.SetActive(false);

                ScriptsConnector.Instance.OnUpdateUI("caughtResult", "BUILD A SNARE ON THE STUMP!");
            }
            else
            {
                if (time > 0)
                {
                    snareState = CommonEnums.SnareState.Unavailable;

                    noBuilding.SetActive(true);
                    inCompleteWire.SetActive(false);
                    halfCompleteWire.SetActive(false);
                    curvedLineA.gameObject.SetActive(false);
                    curvedLineB.gameObject.SetActive(false);
                    completeWire.SetActive(false);
                    rabbit.SetActive(false);

                    //TODO - check that this formatted in a readable timestamp
                    ScriptsConnector.Instance.OnUpdateUI("caughtTime", time.ToString());
                }
                else if (time <= 0)
                {
                    //Load directly into check trap mode
                    snareState = CommonEnums.SnareState.Check;

                    wireAroundTree.SetActive(true);
                    inCompleteWire.SetActive(false);
                    halfCompleteWire.SetActive(false);
                    curvedLineA.gameObject.SetActive(false);
                    curvedLineB.gameObject.SetActive(false);
                    completeWire.SetActive(false);
                    rabbit.SetActive(false);

                    //automatically check trap on game loaded
                    ScriptsConnector.Instance.OnCheckTrap?.Invoke();
                }
            }

            if (this == null) return;
        }
        catch (CloudCodeResultUnavailableException)
        {
            // Exception already handled by CloudCodeManager
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            if (this != null)
            {
                //sceneView.Enable();
            }
        }
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

                //TODO - GG - scriptconnector call to trigger trap set api call on lootboxmanager
                ScriptsConnector.Instance.OnSetTrapTriggerTime?.Invoke();

                //depreciated, just for immediate gratification testing
                //TODO - UI date and wait for 10s
                //StartCoroutine(CatchTimer(10));

                break;
            default:
                break;
        }
    }

    private void RabbitCaught(bool caught)
    {
        if(caught)
        {
            rabbit.SetActive(true);
        }

        ScriptsConnector.Instance.OnUpdateUI("caughtResult", "CAUGHT RABBIT: " + caught.ToString());
    }

    IEnumerator CatchTimer(float time)
    {
        ScriptsConnector.Instance.OnUpdateUI("caughtTime", time.ToString());

        yield return new WaitForSeconds(time);

        //TODO - mock proto of a 10s trap
        ScriptsConnector.Instance.OnCheckTrap?.Invoke();
    }
}
