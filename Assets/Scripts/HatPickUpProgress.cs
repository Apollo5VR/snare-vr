using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class HatPickUpProgress : GrabbableEvents
    {
        private ProgressionController progressionController;

        private void Start()
        {
            progressionController = GameObject.Find("ProgressionController").GetComponent<ProgressionController>();
        }

        public override void OnGrab(Grabber grabber)
        {
            //note: since new system doesnt have a easy head collider, we'll just assume theyve put the hat on in XX seconds, otherwise we'll justprogress them anyways
            StartCoroutine(GrabDelayProgress());

            base.OnGrab(grabber);
        }

        private IEnumerator GrabDelayProgress()
        {
            yield return new WaitForSeconds(3);

            progressionController.LoadNextScene();
        }
    }
}
