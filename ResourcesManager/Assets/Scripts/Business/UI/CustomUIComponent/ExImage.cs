using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace C_Framework
{

    public class ExImage : Image, IPointerClickHandler
    {

        string oldBundleName;
        string OldSpriteName;
        Action action;

        public void SetSprite(string newBundleName, string NewSpriteName)
        {
            if (string.IsNullOrEmpty(newBundleName) || string.IsNullOrEmpty(NewSpriteName)) return;
            if (newBundleName.Equals(this.oldBundleName) && NewSpriteName.Equals(this.OldSpriteName)) return;
            AppFacade.instance.GetSpriteManager().SetSprite(newBundleName, NewSpriteName, (desSprite) => {
                ReduceSpriteReff(oldBundleName);
                sprite = desSprite;
                oldBundleName = newBundleName;
                OldSpriteName = NewSpriteName;
            });

        }

        private void ReduceSpriteReff(string bundleName)
        {
            if (string.IsNullOrEmpty(bundleName)) return;
            AppFacade.instance.GetSpriteManager().ReduceSpriteReff(bundleName);

        }

        protected override void OnDestroy()
        {
            ReduceSpriteReff(oldBundleName);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (action != null)
            {
                action();
            }
        }

        public void SetAction(Action action)
        {
            this.action = action;
        }
    }

}