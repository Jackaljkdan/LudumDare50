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

        public float secondsOverDifficultyIncrease = 60 * 5;

        #endregion

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<FlammableStoppedBurningSignal>().OptionalSubscriber();
            Container.DeclareSignal<EndgameSignal>().OptionalSubscriber();

            Container.BindInstance<float>(secondsOverDifficultyIncrease).WithId("difficulty.seconds").AsSingle();
        }
    }

    public class FlammableStoppedBurningSignal
    {
        public Flammable flammable;
    }

    public class EndgameSignal
    {

    }
}