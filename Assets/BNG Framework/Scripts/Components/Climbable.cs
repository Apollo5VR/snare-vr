﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// Allows the Player to climb objects by Grabbing them
    /// </summary>
    public class Climbable : Grabbable {

        PlayerClimbing playerClimbing;

        void OnEnable() {
            // Make sure Climbable is set to dual grab
            SecondaryGrabBehavior = OtherGrabBehavior.DualGrab;

            // Make sure we don't try tp keep this in our hand
            GrabPhysics = GrabPhysics.None;

            CanBeSnappedToSnapZone = false;

            TwoHandedDropBehavior = TwoHandedDropMechanic.None;

            // Disable Break Distance entirely if default from Grabbable was used
            if(BreakDistance == 1) {
                BreakDistance = 0;
            }

            if(playerClimbing == null && player != null) {
                playerClimbing = player.gameObject.GetComponentInChildren<PlayerClimbing>();
            }
        }

        public override void GrabItem(Grabber grabbedBy) {

            // Add the climber so we can track it's position for Character movement
            if(playerClimbing) {
                playerClimbing.AddClimber(this, grabbedBy);
            }
            
            base.GrabItem(grabbedBy);        
        }

        public override void DropItem(Grabber droppedBy) {
            if(droppedBy != null && playerClimbing != null) {
                playerClimbing.RemoveClimber(droppedBy);
            }
            
            base.DropItem(droppedBy);

            //GG injection
            if (BeingHeld == false)
            {
                //TODO - optimize this, not great to be using this vague parent parent business
                if (gameObject.transform.parent.parent.gameObject != null)
                {
                    gameObject.transform.parent.parent.gameObject.SetActive(false);
                    Debug.Log("GG - WE DEACTIVATE CLIMBABLE ON DROP!");
                }
            }
        }
    }
}