using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class HatPickUpProgress : GrabbableEvents
    {
        public override void OnGrab(Grabber grabber)
        {
            //note: since new system doesnt have a easy head collider, we'll just assume theyve put the hat on in XX seconds, otherwise we'll justprogress them anyways
            StartCoroutine(GrabDelayProgress());

            base.OnGrab(grabber);
        }

        private IEnumerator GrabDelayProgress()
        {
            yield return new WaitForSeconds(3);

            ProgressionController.Instance.OnLoadNextScene?.Invoke(2);
        }
    }
}
