using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Inevitable
{
    public class Installer : MonoInstaller
    {
        #region Inspector fields

        

        #endregion

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<FlammableStoppedBurningSignal>().OptionalSubscriber();
        }
    }

    public class FlammableStoppedBurningSignal
    {
        public Flammable flammable;
    }
}