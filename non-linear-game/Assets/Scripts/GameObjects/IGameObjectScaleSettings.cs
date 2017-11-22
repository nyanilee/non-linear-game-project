namespace GameObjects {
    using UniRx;

    using UnityEngine;

    public interface IGameObjectScaleSettings {
        Vector3ReactiveProperty DefaultScale { get; }

        FloatReactiveProperty DefaultZDistance { get; }
    }
}
