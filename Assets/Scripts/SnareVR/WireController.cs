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

        //SetupSnareScene();

        ScriptsConnector.Instance.OnWireSectionComplete += WireManipulations;
        //ScriptsConnector.Instance.OnRabbitCaught += RabbitCaught;
        //ScriptsConnector.Instance.OnTrapTriggerTimeSet += SetupSnareScene; //note - we've relocated the trap set to zones, only building trap here - ie TrapController
    }

    private void OnDestroy()
    {
        if (ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnWireSectionComplete -= WireManipulations;
            //ScriptsConnector.Instance.OnRabbitCaught -= RabbitCaught;
            //ScriptsConnector.Instance.OnTrapTriggerTimeSet -= SetupSnareScene;
        }
    }

    //call on start to set state and save a local value to the WireController
    public async void SetupSnareScene()
    {
        try
        {
            float time = -1; //await CloudCodeManager.instance.CallGetTrapTimeRemainingEndpoint();

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

                ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "BUILD A SNARE ON THE STUMP!");
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
                    ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Time, time.ToString());
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
                //note: moved to trapcontroller
                //ScriptsConnector.Instance.OnSetTrapTriggerTime?.Invoke();

                //depreciated, just for immediate gratification testing
                //TODO - UI date and wait for 10s
                if(ScriptsConnector.Instance.OnGetCurrentScene?.Invoke() == 1)
                {
                    StartCoroutine(CatchTimer(10));
                }

                break;
            default:
                break;
        }
    }

    private void RabbitCaught(bool caught)
    {
        string message = "";

        if(caught)
        {
            message = "YOU CAUGHT A RABBIT! COLLECT & EAT.";
            rabbit.SetActive(true);
        }
        else
        {
            message = "SORRY, NO RABBIT. SET ANOTHER TRAP.";
        }

        ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, message);
    }

    IEnumerator CatchTimer(float time)
    {
        for(int i = 0; i < 10;  i++)
        {
            time--;
            ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "TRAP TRIGGERS IN: " + time.ToString());
        }

        yield return null;

        //TODO - mock proto of a 10s trap
        //ScriptsConnector.Instance.OnCheckTrap?.Invoke();
        rabbit.SetActive(true);
        ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "YOU CAUGHT A RABBIT!");
    }
}
