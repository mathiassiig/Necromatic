using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Necromatic
{
    public class MouseInput : MonoBehaviour
    {
        // Update is called once per frame
        void FixedUpdate()
        {
            CheckRightClick();
        }

        void CheckRightClick()
        {
            if (Input.GetMouseButton(1))
            {
                // create a ray cast and set it to the mouses cursor position in game
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Debug.DrawLine(ray.origin, hit.point);
                    // if there is nothing, the user probably wants to attack the nearest enemy?
                    // if there is an enemy, attack it
                    // if there is something else, figure out what to do with it
                }
            }
        }
    }
}